using be.berghs.nils.EetFestijnLib.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace be.berghs.nils.EetFestijnLib.ViewModels
{
    public class OrderItemViewModel : ViewModelBase
    {
        private Product Product { get; }

        public string Name => Product.Name;

        public decimal Price => Product.Price;

        public string PriceText => Price.ToString("0.0 €");

        private int? _Count;
        public int? Count
        {
            get => _Count;
            set
            {
                if (SetProperty(ref _Count, value))
                    OnPropertyChanged(nameof(TotalPriceText));
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

        public string TotalPriceText
        {
            get
            {
                if (TotalPrice == null || TotalPrice == 0)
                    return "- €";
                return  TotalPrice.Value.ToString("0.0 €");
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
