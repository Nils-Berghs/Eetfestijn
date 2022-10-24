using be.berghs.nils.EetFestijnLib.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace be.berghs.nils.EetFestijnLib.ViewModels
{
    public class ConsumptionItemViewModel:ViewModelBase
    {
        private Product Product { get; }

        public string Name => Product.Name;

        private int _Consumption;
        public int Consumption 
        { 
            get => _Consumption; 
            internal set => SetProperty(ref _Consumption, value); 
        }

        public ConsumptionItemViewModel(Product product)
        {
            Product = product;
        }
    }
}
