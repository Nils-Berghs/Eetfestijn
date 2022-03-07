using be.berghs.nils.EetFestijnLib.Helpers;
using be.berghs.nils.EetFestijnLib.Helpers.Dialog;
using be.berghs.nils.EetFestijnLib.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace be.berghs.nils.EetFestijnLib.ViewModels
{
    public class OrderViewModel : ViewModelBase
    {
        private IDialogService DialogService { get; }

        public OrderCategoryViewModel Foods { get;  }

        public OrderCategoryViewModel Beverages { get; }

        public OrderCategoryViewModel Desserts { get; }

        private decimal _TotalPrice;
        public decimal TotalPrice
        {
            get => _TotalPrice;
            set => SetProperty(ref _TotalPrice, value); 
        }

        public ICommand OkCommand { get; }

        public ICommand CancelCommand { get;  }

        public OrderViewModel(IDialogService dialogService, ProductList products)
        {
            DialogService = dialogService;
            Foods = new OrderCategoryViewModel(products.Foods, "Eten");
            Beverages = new OrderCategoryViewModel(products.Beverages, "Drank");
            Desserts = new OrderCategoryViewModel(products.Desserts, "Dessert");

            Foods.PropertyChanged += OrderCategoryPropertyChanged;
            Beverages.PropertyChanged += OrderCategoryPropertyChanged;
            Desserts.PropertyChanged += OrderCategoryPropertyChanged;

            OkCommand = new Command(ConfirmOrder, CanConfirmOrder);

        }

        private void OrderCategoryPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(OrderCategoryViewModel.TotalPrice))
            {
                TotalPrice = Foods.TotalPrice + Beverages.TotalPrice + Desserts.TotalPrice;
                ((Command)OkCommand).ChangeCanExecute();
            }
        }

        private void ConfirmOrder()
        {
            Order order = new Order(Foods.GetOrderItems(), Beverages.GetOrderItems(), Desserts.GetOrderItems());
            
        }

        private bool CanConfirmOrder()
        {
            return TotalPrice > 0;
        }

    }
}
