using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections;
using System.ComponentModel;
using Newtonsoft.Json;

namespace be.berghs.nils.eetfestijn.classes
{
    public class Order: PropertyChangedNotifier 
    {
        
        public int BestellingId
        {
            get;
            set;
        }

        public int WaardeBonCount 
        { 
            get; 
            set; 
        }

        public List<OrderItem> Items
        {
            get;
            set;
        }

        [JsonIgnore]
        public List<OrderItem> FoodItems
        {
            get { return GetItemsWithType(ProductType.Eten); }
        }

        [JsonIgnore]
        public List<OrderItem> DrinkItems
        {
            get { return GetItemsWithType(ProductType.Drank); }
        }

        [JsonIgnore]
        public List<OrderItem> DessertItems
        {
            get { return GetItemsWithType(ProductType.Dessert); }
        }

       /* public List<OrderItem> CurrentItems
        {
            get
            {
                List<OrderItem> items = new List<OrderItem>();
                AddOrderedItems(FoodItems, items);
                AddOrderedItems(DessertItems, items);
                AddOrderedItems(DrinkItems, items);
                return items;
            }
        }

        private void AddOrderedItems(List<OrderItem> fromItems, List<OrderItem> toItems)
        {
            foreach (OrderItem oi in fromItems)
            {
                if (oi.Aantal.HasValue)
                    toItems.Add(oi);
            }
        }*/

        private List<OrderItem> GetItemsWithType(ProductType productType)
        {
            List<OrderItem> items = new List<OrderItem>();
            foreach (OrderItem oi in Items)
                if (oi.ProductType == productType)
                    items.Add(oi);
            return items;

        }

        [JsonIgnore]
        public decimal TotalPrice 
        {
            get
            {
                decimal total = 0;
                foreach (OrderItem bi in Items)
                    total += bi.TotalPrice;
                return total;
            }
        }

        [JsonIgnore]
        public string TotalPriceString
        {
            get { return TotalPrice + " €"; }
        }

        /// <summary>
        /// This value contains the amount that was actually payed
        /// Normally this is the TotalPrice - (WaardebonCount* WaardebonValue)
        /// In some cases people give a tip ('T is just) in such cases this amount is higher
        /// </summary>
        public decimal Betaald
        {
            get;
            set;
        }

        public bool MobilePay
        {
            get;
            set;
        }

        public Order()
        {
            Items = new List<OrderItem>();
        }

        public Order(IEnumerable<Product> availableProducts):this()
        {
            

            foreach (Product p in availableProducts)
            {
                var oi = new OrderItem(p);
                //oi.CountChangedEvent += Oi_CountChangedEvent;
                Items.Add(oi);
            }
        }

        private void Oi_CountChangedEvent()
        {
            OnPropertyChanged("TotalPriceString");
        }

        public Order(XmlElement orderElement, IEnumerable<Product> availableProducts)
            :this(availableProducts)
        {
            WaardeBonCount = int.Parse(orderElement.GetAttribute("waardebonCount"));
            Betaald = decimal.Parse(orderElement.GetAttribute("betaald"));
            MobilePay = bool.Parse(orderElement.GetAttribute("mobilepay"));
            XmlNodeList list = orderElement.GetElementsByTagName("Item");
            
            IEnumerator enumerator = list.GetEnumerator();
            while (enumerator.MoveNext())
            {
                XmlAttributeCollection item = ((XmlNode)enumerator.Current).Attributes;
                string name = item.GetNamedItem("product").Value.Trim();
                Product p = App.mProductList.GetProductByName(name);

                OrderItem oi = Items[Items.IndexOf(new OrderItem(p))];
                if (oi != null)
                {
                    oi.Aantal = int.Parse(item.GetNamedItem("aantal").Value);
                }
                            
                //TotalPrice
            }
            
        }
               

        internal decimal GetTeBetalen()
        {
            return (TotalPrice - WaardeBonCount * ProductList.WAARDE_BON_VALUE);
        }

        internal void SaveToXml(XmlTextWriter writer)
        {
            writer.WriteStartElement("Order");
            writer.WriteAttributeString("id", BestellingId+"");
            writer.WriteAttributeString("waardebonCount", WaardeBonCount+"");
            writer.WriteAttributeString("totaal", TotalPrice + "");
            writer.WriteAttributeString("betaald", Betaald + "");
            writer.WriteAttributeString("mobilepay", MobilePay + "");
            writer.WriteStartElement("Items");
            foreach (OrderItem oi in Items)
            {
                oi.SaveToXml(writer);
            }
            writer.WriteEndElement();
            writer.WriteEndElement();
        }

    }
}
