﻿using be.berghs.nils.EetFestijnLib.Models;
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
                        totalPrice+= item.Price;
                }
                TotalPrice = totalPrice;

            }
                
        }

        internal IEnumerable<OrderItem> GetOrderItems()
        {
            return Items.Where(i => i.Count > 0).Select(i => i.GetOrderItem());
        }
    }
}