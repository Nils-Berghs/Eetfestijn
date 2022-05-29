using System;
using System.Collections.Generic;
using System.Text;

namespace be.berghs.nils.EetFestijnLib.Models
{
    public class Session
    {
        public ProductList ProductList { get; }

        public Options Options { get; }

        public OrderList OrderList { get; }

        public Session():this(new ProductList(), new Options ())
        {

        }

        public Session(ProductList products, Options options):this(products, options, new OrderList())
        {

        }

        public Session(ProductList products, Options options, OrderList orderList)
        {
            ProductList = products;
            Options = options;
            OrderList = orderList;
        }
    }
}
