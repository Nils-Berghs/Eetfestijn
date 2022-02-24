using be.berghs.nils.EetFestijnLib.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace be.berghs.nils.EetFestijnLib.ViewModels
{
    public class OrderCategoryViewModel:ViewModelBase
    {
        public ObservableCollection<OrderItemViewModel> Items { get; }

        public string Title { get; }

        internal OrderCategoryViewModel(IEnumerable<Product> products, string title)
        {
            Title = title;
            Items = new ObservableCollection<OrderItemViewModel>();
            foreach(var product in products)
            {
                var oivm = new OrderItemViewModel(product);
                Items.Add(oivm);

                oivm.PropertyChanged += OrderItemViewModelPropertyChanged;
            }
        }

        private void OrderItemViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //TODO
            //if (e.PropertyName == nameof(OrderItemViewModel.TotalPrice))
            //    OnPropertyChanged(nameof(TotalPrice));
        }
    }
}
