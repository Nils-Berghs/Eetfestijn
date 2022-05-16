using be.berghs.nils.EetFestijnLib.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace be.berghs.nils.EetFestijnLib.Helpers.Events
{
    public class OrderAddedEventArgs : EventArgs
    {
        public Order Order { get; }

        public OrderAddedEventArgs(Order order)
        {
            Order = order;
        }
    }
}
