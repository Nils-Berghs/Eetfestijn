using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace be.berghs.nils.EetFestijnLib.Models
{
    public class ProductList
    {
        public IEnumerable<Product> Foods { get; private set; }

        public IEnumerable<Product> Beverages { get; private set; }

        public IEnumerable<Product> Desserts { get; private set; }

        /// <summary>
        /// No args constructor for Json
        /// </summary>
        public ProductList()
        {
            Foods = new List<Product>();
            Beverages = new List<Product>();
            Desserts = new List<Product>();

        }

        public ProductList(IEnumerable<Product> foods, IEnumerable<Product> beverages, IEnumerable<Product> desserts)
        {
            Foods = foods;
            Beverages = beverages;
            Desserts = desserts;
        }
    }
}
