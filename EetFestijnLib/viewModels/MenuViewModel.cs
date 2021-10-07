﻿using be.berghs.nils.EetFestijnLib.classes;
using be.berghs.nils.EetFestijnLib.interfaces;
using be.berghs.nils.EetFestijnLib.models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Windows.Input;

namespace be.berghs.nils.EetFestijnLib.viewModels
{
    public class MenuViewModel: PageViewModel
    {
        public ObservableCollection<Product> Foods { get; }

        public ObservableCollection<Product> Beverages { get; }

        public ObservableCollection<Product> Desserts { get;  }

        public ICommand OkCommand { get; }

        public ICommand CancelCommand { get;  }

        internal MenuViewModel(IViewFactory viewFactory): base(viewFactory)
        {
            Foods = new ObservableCollection<Product>();
            Foods.CollectionChanged += Products_CollectionChanged;
            Beverages = new ObservableCollection<Product>();
            Beverages.CollectionChanged += Products_CollectionChanged;
            Desserts = new ObservableCollection<Product>();
            Desserts.CollectionChanged += Products_CollectionChanged;

            OkCommand = new Command(Confirm, CanConfirm);
            CancelCommand = new Command(Cancel);
        }

        private void Products_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //if the number of items in our collection is updated, reevaluate the change can execute
            ((Command)OkCommand).ChangeCanExecute();
        }

        private void Cancel()
        {
            ViewFactory.PopView();
        }

        private bool CanConfirm()
        {
            return Foods.Count > 0 || Beverages.Count > 0 || Desserts.Count > 0;
        }

        private void Confirm()
        {
            ProductList productList = new ProductList(Foods, Beverages, Desserts);
            SaveProductList(productList);
            ViewFactory.CreateView(new OrderViewModel(ViewFactory, productList));
        }

        /// <summary>
        /// Saves the product list to appData
        /// </summary>
        private void SaveProductList(ProductList productList)
        {
            string path = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            path = Path.Combine(path, "EetFestijn");
            Directory.CreateDirectory(path);
            path = Path.Combine(path, "Menu.json");
            string text = JsonConvert.SerializeObject(productList);
            File.WriteAllText(path, JsonConvert.SerializeObject(productList, Formatting.Indented));

        }
    }
}
