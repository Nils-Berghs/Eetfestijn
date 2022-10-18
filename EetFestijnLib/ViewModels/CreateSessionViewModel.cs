using be.berghs.nils.EetFestijnLib.Helpers;
using be.berghs.nils.EetFestijnLib.Helpers.Dialog;
using be.berghs.nils.EetFestijnLib.Models;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace be.berghs.nils.EetFestijnLib.ViewModels
{
    public class CreateSessionViewModel: PageViewModel
    {
        private IWindowService WindowService { get; }

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

        public bool UseSecondaryScreen { get; set; }

        public Command OkCommand { get; }

        public ICommand CancelCommand { get;  }

        internal CreateSessionViewModel(StackViewModel<PageViewModel> stackViewModel, IDialogService dialogService, IWindowService windowService) : base(stackViewModel, dialogService)
        {
            WindowService = windowService;
            Session session = FileSystemHelper.ReadGlobalSession();
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
            _ =SaveSession(session);
           
            StackViewModel.PushViewModel(new SessionViewModel(StackViewModel, DialogService, session));
            if (UseSecondaryScreen)
            {
                WindowService.ShowWindow(new OrderSummaryViewModel());
            }
        }

        /// <summary>
        /// Saves the session to appData
        /// </summary>
        private async Task SaveSession(Session session)
        {
            //save the general setting file
            FileSystemHelper.SaveGlobalSession(session);
            
            //save the session to its own directory
            await FileSystemHelper.SaveSession(session);
            
        }

        

      

    }
}
