using System;
using System.Collections.Generic;
using System.Text;

namespace be.berghs.nils.EetFestijnLib.Models
{
    public class Session
    {
        public ProductList Products { get; }

        public Options Options { get; }

        public OrderList OrderList { get; }

        public Session(ProductList products, Options options):this(products, options, new OrderList())
        {

        }

        public Session(ProductList products, Options options, OrderList orderList)
        {
            Products = products;
            Options = options;
            OrderList = orderList;
        }
    }
}
