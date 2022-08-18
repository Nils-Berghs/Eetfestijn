using be.berghs.nils.EetFestijnLib.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace be.berghs.nils.EetFestijnLib.ViewModels
{
    public class OrderCategoryViewModel:ViewModelBase
    {

        public ObservableCollection<OrderItemViewModel> Items { get; }

        public string Title { get; }

        private decimal _TotalPrice;
        public decimal TotalPrice
        {
            get => _TotalPrice;
            set => SetProperty(ref _TotalPrice, value);
        }

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
            if (e.PropertyName == nameof(OrderItemViewModel.TotalPrice))
            {
                decimal totalPrice = 0;
                foreach(var item in Items)
                {
                    if (item.TotalPrice != null)
                        totalPrice+= item.TotalPrice.Value;
                }
                TotalPrice = totalPrice;

            }
                
        }

        internal IEnumerable<OrderItem> GetOrderItems()
        {
            //we must call ToList here, to avoid that the enumarable is already cleared by ClearOrder before it is used
            return Items.Where(i => i.Count > 0).Select(i => i.GetOrderItem()).ToList();
        }

        /// <summary>
        /// Clears the order items in this category
        /// </summary>
        internal void ClearOrder()
        {
            foreach(var item in Items)
                item.Count = null;
        }
    }
}
