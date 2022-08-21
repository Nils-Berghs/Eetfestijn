﻿using be.berghs.nils.EetFestijnLib.Helpers;
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
        public event EventHandler OrderAdded;

        public DateTime CreatedDateTime { get; set; }

        public string SessionName => "Session-" + CreatedDateTime.ToString("yyyy-MM-dd HH:mm");

        public int OrderCount { get; set; }

        public ProductList ProductList { get; }

        public Options Options { get; }

        [JsonIgnore]
        public OrderList OrderList { get; }

        /// <summary>
        /// The theoretical income, vouchers included
        /// </summary>
        public decimal TotalIncome { get; set; }

        
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
            OrderAdded?.Invoke(this, EventArgs.Empty);
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
            foreach (var order in OrderList.Orders)
            {
                totalIncome += order.TotalPrice;
                foreach (var item in order.Foods)
                    plateCount += item.Count;
            }
            OrderCount = OrderList.Orders.Count();
            PlateCount = plateCount;
            TotalIncome = totalIncome;

        }
                
    }
}
