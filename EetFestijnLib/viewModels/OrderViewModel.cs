using be.berghs.nils.EetFestijnLib.Helpers;
using be.berghs.nils.EetFestijnLib.Helpers.Dialog;
using be.berghs.nils.EetFestijnLib.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Linq;

namespace be.berghs.nils.EetFestijnLib.ViewModels
{
    public class OrderViewModel : ViewModelBase
    {
        private IDialogService DialogService { get; }

        private IWindowService WindowService { get; }

        private Session Session { get; }

        private PaymentViewModel _PaymentViewModel;
        public PaymentViewModel PaymentViewModel 
        { 
            get => _PaymentViewModel;
            set => SetProperty(ref _PaymentViewModel, value); 
        }

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

        public OrderViewModel(IDialogService dialogService, IWindowService windowService, Session session)
        {
            DialogService = dialogService;
            WindowService = windowService;
            Session = session;

            Foods = new OrderCategoryViewModel(Session.ProductList.Foods, "Eten");
            Beverages = new OrderCategoryViewModel(Session.ProductList.Beverages, "Drank");
            Desserts = new OrderCategoryViewModel(session.ProductList.Desserts, "Dessert");

            Foods.PropertyChanged += OrderCategoryPropertyChanged;
            Beverages.PropertyChanged += OrderCategoryPropertyChanged;
            Desserts.PropertyChanged += OrderCategoryPropertyChanged;

            OkCommand = new Command(async ()=> await ConfirmOrder(), CanConfirmOrder);
            CancelCommand = new Command(Cancel);

            if (session.Options.ShowOrderSummary)
                ShowOrderSummary();

        }

        private void ShowOrderSummary()
        {
            //OrderSummaryViewModel = new OrderSummaryViewModel(this);
            WindowService.ShowWindow(this);
            
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
            PaymentViewModel = new PaymentViewModel(order, Session.Options);

            await DialogService.ShowDialog(PaymentViewModel);

            if (PaymentViewModel.IsConfirmed)
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
            PaymentViewModel = null;

            _ = FocusFirst();
            
        }

        private async Task FocusFirst()
        {
            /* Important: due to the nature of the Material Design in Xaml popups the window gets focus after the popup is closed
             * This focussing is somehow delayed untill after the 'ClearCurrentOrders is called.
             * By delaying the focusing of the first food item by 100ms this is fixed, but even then I still have to set focus to false
             * before being able to visually focus the element
             * */

            await Task.Delay(100);
            Foods.Items.First().Focused = false;
            Foods.Items.First().Focused = true;
        }
    }
}
