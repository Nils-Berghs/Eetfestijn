using be.berghs.nils.EetFestijnLib.Helpers;
using be.berghs.nils.EetFestijnLib.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace be.berghs.nils.EetFestijnLib.ViewModels
{
    public class EditProductViewModel: DialogViewModelBase
    {
        internal Product Product { get; }

        private string _Name;
        public string Name 
        { 
            get => _Name;
            set
            {
                if (SetProperty(ref _Name, value))
                    ((Command)OkCommand).ChangeCanExecute();
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
                    ((Command)OkCommand).ChangeCanExecute();
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

        public EditProductViewModel()
        {
            Product = new Product();
        }

        public EditProductViewModel(Product product)
        {
            Product = product;
            Name = Product.Name;
            Price = Product.Price.ToString("N1");
        }

        protected override bool CanConfirm()
        {
            return !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Price);
        }

        protected override Task Confirm()
        {
            Product.Price = decimal.Parse(Price);
            Product.Name = Name;
            return Task.CompletedTask;
        }

    }
}
