using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace be.berghs.nils.EetFestijnLib.Models
{
    public class Order
    {
        public ObservableCollection<OrderItem> OrderItems { get; set; }
    }
}
