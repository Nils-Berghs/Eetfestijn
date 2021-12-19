using be.berghs.nils.EetFestijnLib.Helpers;
using be.berghs.nils.EetFestijnLib.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace be.berghs.nils.EetFestijnLib.ViewModels
{
    public class ProductViewModel: ViewModelBase
    {
        private Product Product { get; }

        public string Name { get; set; }

        private string _Price;
        public string Price 
        { 
            get => _Price; 
            set
            {
                //get a corrected string
                string newValue = StringToDecimalHelper.CheckDecimalString(value, _Price);
                //set the new price
                if (!SetProperty(ref _Price, newValue))
                {
                    //the price was not different from the new value, if the newValue was different from the value, raise the property changed manually
                    if (newValue != value)
                        OnPropertyChanged();
                }
            }
        }

        public ProductViewModel()
        {

        }

        public ProductViewModel(Product product)
        {
            Product = product;
            Name = Product.Name;
            Price = Product.Price.ToString("N1");
        }

        


    }
}
