﻿using be.berghs.nils.EetFestijnLib.Helpers;
using be.berghs.nils.EetFestijnLib.Helpers.Dialog;
using be.berghs.nils.EetFestijnLib.Models;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace be.berghs.nils.EetFestijnLib.ViewModels
{
    public class MenuViewModel: PageViewModel
    {
        public ProductCategoryViewModel Foods { get; private set; }

        public ProductCategoryViewModel Beverages { get; private set; }

        public ProductCategoryViewModel Desserts { get; private set; }

        private bool _UseVouchers;
        public bool UseVouchers 
        { 
            get => _UseVouchers;
            set
            {
                if (SetProperty(ref _UseVouchers, value))
                    OkCommand.ChangeCanExecute();
            }
        }

        private string _VoucherValue;
        public string VoucherValue 
        { 
            get => _VoucherValue; 
            set
            {
                //get a corrected string
                string newValue = StringToDecimalHelper.CheckDecimalString(value, _VoucherValue, "0.#", true);
                //set the new price
                if (SetProperty(ref _VoucherValue, newValue))
                    OkCommand.ChangeCanExecute();
                else if (newValue != value)
                    OnPropertyChanged();
            }
        }

        public bool UseMobilePayments { get; set; }

        public Command OkCommand { get; }

        public ICommand CancelCommand { get;  }

        internal MenuViewModel(StackViewModel<PageViewModel> stackViewModel, IDialogService dialogService) : base(stackViewModel, dialogService)
        {
            ProductList productList = ReadProductList();
            Foods = new ProductCategoryViewModel(productList.Foods, DialogService, "Eten");
            Beverages = new ProductCategoryViewModel(productList.Beverages, DialogService, "Drank");
            Desserts = new ProductCategoryViewModel(productList.Desserts, DialogService, "Dessert");

            OkCommand = new Command(Confirm, CanConfirm);
            CancelCommand = new Command(Cancel);
            
        }

        private void Cancel()
        {
            StackViewModel.PopViewModel();
        }

        private bool CanConfirm()
        {
            if (Foods.Products.Count == 0 && Beverages.Products.Count == 0 && Desserts.Products.Count == 0)
                return false;
            if (UseVouchers && (string.IsNullOrWhiteSpace(VoucherValue) || decimal.Parse(VoucherValue) <= 0))
                return false;
            return true;
        }

        private void Confirm()
        {
            ProductList productList = new ProductList(Foods.GetProducts(), Beverages.GetProducts(), Desserts.GetProducts());
            Options options = new Options(UseVouchers, VoucherValue, UseMobilePayments);
            SaveProductList(productList);
            StackViewModel.PushViewModel(new SessionViewModel(StackViewModel, DialogService, productList));
        }

        /// <summary>
        /// Saves the product list to appData
        /// </summary>
        private void SaveProductList(ProductList productList)
        {
            string path = GetTempMenuPath();
            FileInfo fileInfo = new FileInfo(path);
            Directory.CreateDirectory(fileInfo.DirectoryName);

            File.WriteAllText(path, JsonConvert.SerializeObject(productList, Formatting.Indented));

        }

        /// <summary>
        /// This function reads a product list from a temporary file from AppData
        /// </summary>
        private ProductList ReadProductList()
        {
            string path = GetTempMenuPath();
            try
            {
                if (File.Exists(path))
                {
                    return JsonConvert.DeserializeObject<ProductList>(File.ReadAllText(path));
                }
                else
                {
                    return new ProductList();
                }
            }
            catch
            {
                return new ProductList();
            }
        }

        /// <summary>
        /// Gets the path to temporarily save the menu.
        /// This is AppData/Local/Eetfestijn
        /// </summary>
        /// <returns></returns>
        private string GetTempMenuPath()
        {
            return FileSystemHelper.GetTempPath("Menu.json");
        }
    }
}
