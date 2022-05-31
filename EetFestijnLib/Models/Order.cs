using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace be.berghs.nils.EetFestijnLib.Models
{
    public class Order
    {
        public IEnumerable<OrderItem> Foods { get; set; }

        public IEnumerable<OrderItem> Beverages { get; set; }

        public IEnumerable<OrderItem> Desserts { get; set; }

        public int OrderId { get; set; }

        public decimal TotalPrice { get; }
        public Payment Payment { get; set; }

        public Order(decimal totalPrice, IEnumerable<OrderItem> foods, IEnumerable<OrderItem> beverages, IEnumerable<OrderItem> desserts)
        {
            TotalPrice = totalPrice;
            Foods = foods;
            Beverages = beverages;
            Desserts = desserts;
        }

    }
}
