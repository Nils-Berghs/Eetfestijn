using be.berghs.nils.EetFestijnLib.Helpers.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace be.berghs.nils.EetFestijnLib.Models
{
    public class OrderList
    {
        public event EventHandler<OrderAddedEventArgs> OrderAdded;

        public IEnumerable<Order> Orders => OrdersList;

        private List<Order> OrdersList { get; }

        internal OrderList()
        {
            OrdersList = new List<Order>();
        }

        internal void AddOrder(Order order)
        {
            OrdersList.Add(order);

            OrderAdded?.Invoke(this, new OrderAddedEventArgs(order));
        }

    }
}
