using be.berghs.nils.EetFestijnLib.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private bool _Focused;
        public bool Focused 
        {   
            get => _Focused;
            set
            {
                Debug.WriteLine("OrderItemViewModel, Setting Focused for {0} from {1} to {2}", Name, _Focused, value);
                SetProperty(ref _Focused, value);
            }
        }

        public bool HasValue => Count.HasValue && Count.Value > 0;

        private int? _Count;
        public int? Count
        {
            get => _Count;
            set
            {
                if (SetProperty(ref _Count, value))
                {
                    TotalPrice = _Count * Price;
                    OnPropertyChanged(nameof(HasValue));
                }
                    
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
