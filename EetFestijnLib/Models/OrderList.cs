using be.berghs.nils.EetFestijnLib.Helpers.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace be.berghs.nils.EetFestijnLib.Models
{
    public class OrderList
    {
        public event EventHandler<OrderAddedEventArgs> OrderAdded;
        public event EventHandler<OrdersAddedEventArgs> OrdersAdded;

        public IEnumerable<Order> Orders => OrdersList;

        private List<Order> OrdersList { get; }

        internal OrderList()
        {
            OrdersList = new List<Order>();
        }

        internal void AddOrder(Order order)
        {
            OrdersList.Add(order);
            order.OrderId = OrdersList.Count;

            OrderAdded?.Invoke(this, new OrderAddedEventArgs(order));
        }

        /// <summary>
        /// Use this function for reading existing orders (open session)
        /// </summary>
        /// <param name="orders"></param>
        internal void AddOrders(IEnumerable<Order> orders)
        {
            foreach (var order in orders)
                OrdersList.Add(order);
            
            OrdersAdded?.Invoke(this, new OrdersAddedEventArgs(orders));

        }

        /// <summary>
        /// Use this function for importing existing orders
        /// </summary>
        /// <param name="orders"></param>
        internal void ImportOrders(IEnumerable<Order> orders)
        {
            foreach (var order in orders)
            {
                OrdersList.Add(order);
                order.OrderId = OrdersList.Count;
            }


            OrdersAdded?.Invoke(this, new OrdersAddedEventArgs(orders));
        }
    }
}
