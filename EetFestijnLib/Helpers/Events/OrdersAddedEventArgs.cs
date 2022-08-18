using be.berghs.nils.EetFestijnLib.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace be.berghs.nils.EetFestijnLib.Helpers.Events
{
    /// <summary>
    /// An event arg for multiple orders
    /// </summary>
    public class OrdersAddedEventArgs: EventArgs
    {
        public IEnumerable<Order> Orders { get; }

        public OrdersAddedEventArgs(IEnumerable<Order> orders)
        {
            Orders = orders;
        }
    }
}
