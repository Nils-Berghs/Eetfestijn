using be.berghs.nils.EetFestijnLib.Helpers;
using be.berghs.nils.EetFestijnLib.Helpers.Dialog;
using be.berghs.nils.EetFestijnLib.Models;
using System;
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

        private Session Session { get; }

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

        public OrderViewModel(IDialogService dialogService, Session session)
        {
            DialogService = dialogService;
            Session = session;

            Foods = new OrderCategoryViewModel(Session.ProductList.Foods, "Eten");
            Beverages = new OrderCategoryViewModel(Session.ProductList.Beverages, "Drank");
            Desserts = new OrderCategoryViewModel(session.ProductList.Desserts, "Dessert");

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
            PaymentViewModel paymentViewModel = new PaymentViewModel(order, Session.Options);

            await DialogService.ShowDialog(paymentViewModel);

            if (paymentViewModel.IsConfirmed)
            {
                Session.OrderList.AddOrder(order);
                ClearCurrentOrder();
            }
            
        }

        private bool CanConfirmOrder()
        {
            return TotalPrice > 0;
        }

        private void Cancel()
        {
            ClearCurrentOrder();
        }

        private void ClearCurrentOrder()
        {
            Foods.ClearOrder();
            Beverages.ClearOrder();
            Desserts.ClearOrder();
        }
    }
}
