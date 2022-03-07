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
                    TotalPrice = _Count * Price;
                    
            }
        }

        public decimal? _TotalPrice;
        public decimal? TotalPrice
        {
            get => _TotalPrice;
            set 
            {
                if (SetProperty(ref _TotalPrice, value))
                    OnPropertyChanged(nameof(TotalPriceText));
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

        internal OrderItem GetOrderItem()
        {
            return new OrderItem(Product, Count.Value);
        }
    }
}
