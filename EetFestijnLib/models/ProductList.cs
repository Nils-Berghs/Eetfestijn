using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace be.berghs.nils.EetFestijnLib.models
{
    public class ProductList
    {
        public ObservableCollection<Product> Products { get; set; }

        /// <summary>
        /// No args constructor for Json
        /// </summary>
        public ProductList()
        {
            Products = new ObservableCollection<Product>();

        }
    }
}
