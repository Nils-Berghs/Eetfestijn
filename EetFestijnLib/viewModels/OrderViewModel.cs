using be.berghs.nils.EetFestijnLib.Helpers;
using be.berghs.nils.EetFestijnLib.Helpers.Dialog;
using be.berghs.nils.EetFestijnLib.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace be.berghs.nils.EetFestijnLib.ViewModels
{
    public class OrderViewModel : ViewModelBase
    {
        private IDialogService DialogService { get; }

        private ProductList ProductList { get; }

        public OrderCategoryViewModel Foods { get;  }

        public OrderCategoryViewModel Beverages { get; }

        public OrderCategoryViewModel Desserts { get; }

        private decimal _TotalPrice;
        public decimal TotalPrice
        {
            get => _TotalPrice;
            set => SetProperty(ref _TotalPrice, value); 
        }

        public Command OkCommand { get; }

        public Command CancelCommand { get;  }

        public OrderViewModel(IDialogService dialogService, ProductList products)
        {
            DialogService = dialogService;
            ProductList = products;

            Foods = new OrderCategoryViewModel(products.Foods, "Eten");
            Beverages = new OrderCategoryViewModel(products.Beverages, "Drank");
            Desserts = new OrderCategoryViewModel(products.Desserts, "Dessert");

            Foods.PropertyChanged += OrderCategoryPropertyChanged;
            Beverages.PropertyChanged += OrderCategoryPropertyChanged;
            Desserts.PropertyChanged += OrderCategoryPropertyChanged;

            OkCommand = new Command(async ()=> await ConfirmOrder(), CanConfirmOrder);
            CancelCommand = new Command(Cancel);

        }

        private void OrderCategoryPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(OrderCategoryViewModel.TotalPrice))
            {
                TotalPrice = Foods.TotalPrice + Beverages.TotalPrice + Desserts.TotalPrice;
                OkCommand.ChangeCanExecute();
            }
        }

        private async Task ConfirmOrder()
        {
            Order order = new Order(TotalPrice, Foods.GetOrderItems(), Beverages.GetOrderItems(), Desserts.GetOrderItems());
            PaymentViewModel paymentViewModel = new PaymentViewModel(order, ProductList.VoucherValue);

            await DialogService.ShowDialog(paymentViewModel);

            if (paymentViewModel.IsConfirmed)
            {
                //TODO save somewhere
                //TODO clear the currenct order
            }
            
        }

        private bool CanConfirmOrder()
        {
            return TotalPrice > 0;
        }

        private void Cancel()
        {
            //Todo clear current order
        }

    }
}
