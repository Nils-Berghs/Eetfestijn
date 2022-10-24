using be.berghs.nils.EetFestijnLib.Helpers;
using be.berghs.nils.EetFestijnLib.Helpers.Events;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace be.berghs.nils.EetFestijnLib.Models
{
    public class Session
    {
        public event EventHandler<OrderAddedEventArgs> OrderAdded;

        public event EventHandler<OrdersAddedEventArgs> OrdersAdded;

        public DateTime CreatedDateTime { get; set; }

        public string SessionName => "Session-" + CreatedDateTime.ToString("yyyy-MM-dd HH:mm");

        
        //setter can nop be private for json deserialization
        public int OrderCount { get; set; }

        public ProductList ProductList { get; }

        public Options Options { get; }

        [JsonIgnore]
        public OrderList OrderList { get; }

        /// <summary>
        /// The theoretical income, vouchers included
        /// </summary>
        public decimal TotalIncome { get; set; }

        /// <summary>
        /// The net income without vouches
        /// </summary>
        public decimal NetIncome { get; set; }

        /// <summary>
        /// The cash income
        /// </summary>
        public decimal CashIncome { get; set; }

        /// <summary>
        /// The income via mobile payments
        /// </summary>
        public decimal MobileIncome { get; set; }

        /// <summary>
        /// Total amount of tips
        /// </summary>
        public decimal Tips { get; set; }

        
        public int PlateCount { get; set; }

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
            orderList.OrderAdded += OrderListOrderAdded;
            orderList.OrdersAdded += OrderListOrdersAdded;
        }

        private void OrderListOrdersAdded(object sender, OrdersAddedEventArgs e)
        {
            RecalculateTotals();
            OrdersAdded?.Invoke(this, e);
        }

        private void OrderListOrderAdded(object sender, OrderAddedEventArgs e)
        {
            RecalculateTotals();
            OrderAdded?.Invoke(this, e);
            _ = FileSystemHelper.SaveSessionAndOrder(this, e.Order);
        }

        private void RecalculateTotals()
        {
            int plateCount = 0;
            decimal totalIncome = 0;
            decimal mobileIncome = 0;
            decimal cashIncome = 0;
            decimal netIncome = 0;
            decimal tips = 0;
            foreach (var order in OrderList.Orders)
            {
                totalIncome += order.TotalPrice;
                netIncome +=order.Payment.NettoPrice;
                tips += order.Payment.Tip;

                if (order.Payment.MobilePayment)
                    mobileIncome += order.Payment.NettoPrice;
                else
                    cashIncome += order.Payment.NettoPrice;



                foreach (var item in order.Foods)
                    plateCount += item.Count;
            }
            OrderCount = OrderList.Orders.Count();
            PlateCount = plateCount;
            TotalIncome = totalIncome;
            MobileIncome = mobileIncome;
            CashIncome = cashIncome;
            NetIncome = netIncome;
            Tips = tips;
        }

        internal void ExportMenu(string fileName)
        {
            FileSystemHelper.ExportMenu(this, fileName);
        }

        internal void Export(string fileName)
        {
            FileSystemHelper.Export(this, fileName);
        }

        internal void Import(string fileName)
        {
            Session session = FileSystemHelper.Import(fileName);
            Todo implement
        }
    }
}
