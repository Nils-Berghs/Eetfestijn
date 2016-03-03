﻿using System;
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
