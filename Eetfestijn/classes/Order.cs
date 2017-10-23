using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections;

namespace be.berghs.nils.eetfestijn.classes
{
    public class Order
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

        public List<OrderItem> FoodItems
        {
            get { return GetItemsWithType(ProductType.Eten); }
        }

        public List<OrderItem> DrinkItems
        {
            get { return GetItemsWithType(ProductType.Drank); }
        }

        public List<OrderItem> DessertItems
        {
            get { return GetItemsWithType(ProductType.Dessert); }
        }

        private List<OrderItem> GetItemsWithType(ProductType productType)
        {
            List<OrderItem> items = new List<OrderItem>();
            foreach (OrderItem oi in Items)
                if (oi.ProductType == productType)
                    items.Add(oi);
            return items;

        }

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

        public Order(IEnumerable<Product> availableProducts)
        {
            Items = new List<OrderItem>();
            
            foreach(Product p in availableProducts)
                Items.Add(new OrderItem(p));
        }

        public Order(XmlElement orderElement, IEnumerable<Product> availableProducts)
            :this(availableProducts)
        {
            WaardeBonCount = int.Parse(orderElement.GetAttribute("waardebonCount"));
            XmlNodeList list = orderElement.GetElementsByTagName("Item");
            
            IEnumerator enumerator = list.GetEnumerator();
            while (enumerator.MoveNext())
            {
                XmlAttributeCollection item = ((XmlNode)enumerator.Current).Attributes;
                string name = item.GetNamedItem("product").Value;
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
