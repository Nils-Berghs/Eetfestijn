using System;
using System.Collections.Generic;
using System.Text;

namespace be.berghs.nils.EetFestijnLib.Models
{
    /// <summary>
    /// A product represents an item on the menu (the ProductList)
    /// </summary>
    public class Product
    {
        /// <summary>
        /// The name of the product
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The price of the product
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// The type of product
        /// </summary>
        public ProductTypes ProductType { get; set; }

        /// <summary>
        /// No args constructor for Json
        /// </summary>
        public Product()
        {

        }
    }
}
