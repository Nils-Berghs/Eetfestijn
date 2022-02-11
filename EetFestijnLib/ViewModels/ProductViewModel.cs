using be.berghs.nils.EetFestijnLib.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace be.berghs.nils.EetFestijnLib.ViewModels
{
    public class ProductViewModel:ViewModelBase
    {
        internal Product Product { get; }

        public string Name => Product.Name;

        public string Price => Product.Price.ToString("N1");

        public ProductViewModel(Product product)
        {
            Product = product;
        }

        internal void RaisePropertyChanged()
        {
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Price));
        }
    }
}
