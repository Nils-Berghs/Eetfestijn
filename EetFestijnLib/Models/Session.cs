using be.berghs.nils.EetFestijnLib.Helpers;
using be.berghs.nils.EetFestijnLib.Helpers.Events;
using be.berghs.nils.EetFestijnLib.Helpers.Exceptions;
using ClosedXML.Excel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Collections.Specialized.BitVector32;

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
            //first load the othes session in memory
            Session session = FileSystemHelper.Import(fileName);

            //check if the sessions menu is compatible
            if (!CheckProductListsCompatible(session))
                throw new IncompatibleMenuException();

            OrderList.AddOrders(session.OrderList.Orders);

            _ = FileSystemHelper.SaveSessionAndOrders(this, session.OrderList.Orders);
        }

        private bool CheckProductListsCompatible(Session session)
        {
            if (!CheckProductListsCompatible(session.ProductList.Foods, ProductList.Foods))
                return false;
            if (!CheckProductListsCompatible(session.ProductList.Beverages, ProductList.Beverages))
                return false;
            if (!CheckProductListsCompatible(session.ProductList.Desserts, ProductList.Desserts))
                return false;

            return true;
        }

        private bool CheckProductListsCompatible(IEnumerable<Product> importList, IEnumerable<Product> currentList)
        {
            foreach (var importItem in importList)
            {
                Product currentItem = currentList.Where(c => string.Equals(importItem.Name, c.Name, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                if (currentItem == null || currentItem.Price != importItem.Price)
                    return false;
            }
            return true;
        }

        internal void ExportToExcel(string fileName)
        {
            using (var workbook = new XLWorkbook())
            {
                var workSheet = workbook.Worksheets.Add("Consumptie");
                int rowIndex = 2;
                int firstColIndex = 2;
                int colIndex = firstColIndex;

                //first get all the dates
                var dates = OrderList.Orders.Select(o => o.OrderDate).Distinct().OrderBy(o => o);

                //build a list for each product by day
                var consumptionByDate = AddConsumptions(dates);

                workSheet.Cell(rowIndex, colIndex).Value = "Product";
                foreach (var date in dates)
                {
                    colIndex++;
                    workSheet.Cell(rowIndex, colIndex).Value = date.Value.ToString("ddd");
                    
                }
                colIndex++;
                workSheet.Cell(rowIndex, colIndex).Value = "Totaal";
                                
                foreach(var productConsumption in consumptionByDate)
                {
                    rowIndex++;
                    colIndex = firstColIndex;
                    workSheet.Cell(rowIndex, colIndex).Value = productConsumption.Key;
                    foreach(var item in productConsumption.Value)
                    {
                        colIndex++;
                        workSheet.Cell(rowIndex, colIndex).Value = item.Value;
                    }
                    colIndex++;
                    workSheet.Cell(rowIndex, colIndex).FormulaR1C1 = "SUM(RC[-" +productConsumption.Value.Count +"]:RC[-1])";
                }

                workbook.SaveAs(fileName);
            }
        }

        private Dictionary<string, Dictionary<DateTime, int>> AddConsumptions(IOrderedEnumerable<DateTime?> dates)
        {
            var consumptionByDate = new Dictionary<string, Dictionary<DateTime, int>>();
            AddConsumptions(consumptionByDate, dates, ProductList.Foods);
            AddConsumptions(consumptionByDate, dates, ProductList.Beverages);
            AddConsumptions(consumptionByDate, dates, ProductList.Desserts);
            
            foreach (var order in OrderList.Orders)
            {
                foreach (var food in order.Foods)
                    consumptionByDate[food.Product.Name][order.OrderDate.Value]+= food.Count;
                foreach (var food in order.Beverages)
                    consumptionByDate[food.Product.Name][order.OrderDate.Value] += food.Count;
                foreach (var food in order.Desserts)
                    consumptionByDate[food.Product.Name][order.OrderDate.Value] += food.Count;
            }
            return consumptionByDate;
        }

        private void AddConsumptions(Dictionary<string, Dictionary<DateTime, int>> consumptionByDate, IOrderedEnumerable<DateTime?> dates, IEnumerable<Product> products)
        {
            foreach (var product in products)
            {
                var productConsumptionByDate = new Dictionary<DateTime, int>();
                foreach (var date in dates)
                {
                    if (date.HasValue)
                        productConsumptionByDate.Add(date.Value, 0);
                }
                consumptionByDate.Add(product.Name, productConsumptionByDate);
            }
        }
    }
}
