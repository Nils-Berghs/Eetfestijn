using be.berghs.nils.EetFestijnLib.Helpers;
using be.berghs.nils.EetFestijnLib.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace be.berghs.nils.EetFestijnLib.ViewModels
{
    public class OrderViewModel : PageViewModel
    {
        public ObservableCollection<OrderItemViewModel> Foods { get;  }

        public ObservableCollection<OrderItemViewModel> Beverages { get; }

        public ObservableCollection<OrderItemViewModel> Desserts { get; }

        public decimal TotalPrice
        {
            get
            {
                return GetTotalPrice(Foods) + GetTotalPrice(Beverages) + GetTotalPrice(Desserts);
            }
        }

        public ICommand OkCommand { get; }

        public ICommand CancelCommand { get;  }

        public OrderViewModel(StackViewModel<PageViewModel> stackViewModel, ProductList products) : base(stackViewModel)
        {
            Foods = CreateOrderItemViewModels(products.Foods);
            Beverages = CreateOrderItemViewModels(products.Beverages);
            Desserts = CreateOrderItemViewModels(products.Desserts);

            OkCommand = new Command(ConfirmOrder, CanConfirmOrder);

        }

        private void ConfirmOrder()
        {
            Order order = new Order();
            
        }

        private bool CanConfirmOrder()
        {
            return TotalPrice > 0;
        }

        private ObservableCollection<OrderItemViewModel> CreateOrderItemViewModels(IEnumerable<Product> products)
        {
            var result = new ObservableCollection<OrderItemViewModel>();
            foreach(var product in products)
            {
                var oivm = new OrderItemViewModel(product);
                result.Add(oivm);

                oivm.PropertyChanged += OrderItemViewModelPropertyChanged;
            }
            return result;
        }

        private void OrderItemViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(OrderItemViewModel.TotalPrice))
                OnPropertyChanged(nameof(TotalPrice));
        }

        private decimal GetTotalPrice(IEnumerable<OrderItemViewModel> orderItemViewModels)
        {
            decimal total = 0;
            foreach (var oivm in orderItemViewModels)
            {
                if (oivm.TotalPrice.HasValue)
                    total += oivm.TotalPrice.Value;
            }
            return total;
        }
    }
}
