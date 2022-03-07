using System;
using System.Collections.Generic;
using System.Text;

namespace be.berghs.nils.EetFestijnLib.Models
{
    /// <summary>
    /// An order item is part of an order, it contains a Product and a count
    /// </summary>
    public class OrderItem
    {
        /// <summary>
        /// The product this OrderItem referst to
        /// </summary>
        public Product Product { get; set; }

        /// <summary>
        /// The number of times this product is ordered. 
        /// </summary>
        public int Count { get; set; }

        public decimal TotalPrice => Product.Price * Count;

        /// <summary>
        /// No args constructor for Json
        /// </summary>
        public OrderItem(Product product, int count)
        {
            Product = product;
            Count = count;
        }
    }
}
