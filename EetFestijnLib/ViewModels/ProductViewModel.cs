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

        /// <summary>
        /// An event to listen for initialized of the viewmodel.
        /// Initialized meaning that both name and price have been set
        /// </summary>
        internal event EventHandler ProductViewModelInitializedEvent;

        private string _Name;
        public string Name 
        { 
            get =>_Name; 
            set
            {
                if (SetProperty(ref _Name, value))
                    CheckRaiseInitializedEvent();
            }
        }

        private string _Price;
        public string Price 
        { 
            get => _Price; 
            set
            {
                //get a corrected string
                string newValue = StringToDecimalHelper.CheckDecimalString(value, _Price);
                //set the new price
                if (SetProperty(ref _Price, newValue))
                {
                    CheckRaiseInitializedEvent();
                }
                else
                {
                    //the old price as number was not different from the new price,
                    //but if the newValue (as string) was different from the old value, the property changed must still be raised manually
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

        private void CheckRaiseInitializedEvent()
        {
            if(!string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Price))
                ProductViewModelInitializedEvent?.Invoke(this, EventArgs.Empty);
        }



    }
}
