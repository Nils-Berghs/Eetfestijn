using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace be.berghs.nils.EetFestijnLib.Models
{
    public class Session
    {
        public DateTime CreatedDateTime { get; }

        public string SessionName => "Session-" + CreatedDateTime.ToString("yyyy-MM-dd HH:mm");

        public int OrderCount => OrderList.Orders.Count();

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
            CreatedDateTime = DateTime.Now;
            ProductList = products;
            Options = options;
            OrderList = orderList;
        }

        /// <summary>
        /// Gets a name that is also a valid directoryName
        /// </summary>
        /// <returns></returns>
        internal string GetSessionPathName()
        {
            return "Session-" + CreatedDateTime.ToString("yyyyMMdd_HHmm");
        }
                
    }
}
