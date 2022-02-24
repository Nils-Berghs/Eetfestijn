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

        public decimal TotalPrice
        {
            get
            {
                //TODO
                return 0; // GetTotalPrice(Foods) + GetTotalPrice(Beverages) + GetTotalPrice(Desserts);
            }
        }

        public ICommand OkCommand { get; }

        public ICommand CancelCommand { get;  }

        public OrderViewModel(IDialogService dialogService, ProductList products) 
        {
            DialogService = dialogService;
            Foods = new OrderCategoryViewModel(products.Foods, "Eten");
            Beverages = new OrderCategoryViewModel(products.Beverages, "Drank");
            Desserts = new OrderCategoryViewModel(products.Desserts, "Dessert");

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
