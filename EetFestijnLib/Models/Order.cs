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

        public Decimal TotalPrice { get; }

        public Order(decimal totalPrice, IEnumerable<OrderItem> foods, IEnumerable<OrderItem> beverages, IEnumerable<OrderItem> desserts)
        {
            Foods = foods;
            Beverages = beverages;
            Desserts = desserts;
        }

    }
}
