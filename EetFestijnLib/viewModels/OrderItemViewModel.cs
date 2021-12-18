using be.berghs.nils.EetFestijnLib.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace be.berghs.nils.EetFestijnLib.ViewModels
{
    public class OrderItemViewModel:ViewModelBase
    {
        private Product Product { get; }

        public string Name => Product.Name;

        public decimal Price => Product.Price;

        private int? _Count;
        public int? Count
        {
            get => _Count;
            set
            {
                if (SetProperty(ref _Count, value))
                    OnPropertyChanged(nameof(TotalPrice));
            }
        }

        public decimal? TotalPrice
        {
            get
            {
                if (Count == null)
                    return null;
                return Count * Price;
            }
        }

        public OrderItemViewModel(Product p)
        {
            Product = p;
        }

        //internal OrderItem ToOrderItem()
        //{
        //    return new OrderItem
        //    {
        //        Count = Count.Value,
        //        Product = Product,
        //    }
        //}
    }
}
