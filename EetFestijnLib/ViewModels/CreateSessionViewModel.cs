using be.berghs.nils.EetFestijnLib.Helpers;
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
    public class CreateSessionViewModel: PageViewModel
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
                {
                    OkCommand?.ChangeCanExecute();

                    //if not using voucher, clear the voucher value
                    if (!value)
                        VoucherValue = null;
                }
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
                    OkCommand?.ChangeCanExecute();
                else if (newValue != value)
                    OnPropertyChanged();
            }
        }

        public bool UseMobilePayments { get; set; }

        public Command OkCommand { get; }

        public ICommand CancelCommand { get;  }

        internal CreateSessionViewModel(StackViewModel<PageViewModel> stackViewModel, IDialogService dialogService) : base(stackViewModel, dialogService)
        {
            Session session = ReadSession();
            Foods = new ProductCategoryViewModel(session.ProductList.Foods, DialogService, "Eten");
            Beverages = new ProductCategoryViewModel(session.ProductList.Beverages, DialogService, "Drank");
            Desserts = new ProductCategoryViewModel(session.ProductList.Desserts, DialogService, "Dessert");

            UseVouchers = session.Options.UseVouchers;
            VoucherValue = session.Options.VoucherValue?.ToString("0.#");
            UseMobilePayments = session.Options.UseMobilePayments;

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
            Session session = new Session(productList, options);
            SaveSession(session);
           
            StackViewModel.PushViewModel(new SessionViewModel(StackViewModel, DialogService, session));
        }

        /// <summary>
        /// Saves the session to appData
        /// </summary>
        private void SaveSession(Session session)
        {
            string path = GetTempSessionPath();
            FileInfo fileInfo = new FileInfo(path);
            Directory.CreateDirectory(fileInfo.DirectoryName);

            File.WriteAllText(path, JsonConvert.SerializeObject(session, Formatting.Indented));

        }

        /// <summary>
        /// This function reads a product list from a temporary file from AppData
        /// </summary>
        private Session ReadSession()
        {
            string path = GetTempSessionPath();
            try
            {
                if (File.Exists(path))
                    return JsonConvert.DeserializeObject<Session>(File.ReadAllText(path));
            }
            catch
            {
            }
            //fall back to a new product list
            return new Session();

        }

        /// <summary>
        /// Gets the path to temporarily save the session.
        /// This is AppData/Local/Eetfestijn
        /// </summary>
        /// <returns></returns>
        private string GetTempSessionPath()
        {
            return FileSystemHelper.GetTempPath("Session.json");
        }

    }
}
