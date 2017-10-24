﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections;

namespace be.berghs.nils.eetfestijn.classes
{
    public class OrderList:INotifyPropertyChanged
    {
        private List<Order> mList = new List<Order>();

        private decimal mStartCash = 0;
        public string StartCash
        {
            get
            {
                return mStartCash + "€";
            }
            set
            {
                try
                {
                    string strVal = value.Trim().Trim('€').Trim();
                    decimal val = decimal.Parse(strVal);
                    if (mStartCash == val)
                        return;
                    mStartCash = val;
                }
                catch (Exception)
                {
                }
                OnPropertyChanged("StartCash");
                UpdateCash();
            }
        }

        public ReadOnlyCollection<Order> AllOrders
        {
            get
            {
                return mList.AsReadOnly();
            }
        }
        

        private decimal mTotalSum = 0;
        /// <summary>
        /// The total sum that we should have received, this includes the coupons but no tips
        /// </summary>
        public string TotalSum
        {
            get
            {
                return mTotalSum +"€";
            }

        }

        private decimal mReceived;
        public string Received
        {
            get
            {
                return mReceived + "€";
            }
        }

        private decimal mVoucherCount = 0;
        public string VoucherCount
        {
            get
            {
                return mVoucherCount + "";
            }
        }

        private decimal mNetIncome;
        /// <summary>
        /// This is the amount of cash that we should have received.
        /// This does not include the value of the coupons, nor any tips
        /// 
        /// </summary>
        public string NetIncome
        {
            get
            {
                return mNetIncome + "€";
            }
        }

        private decimal mBrutoIncome;
        /// <summary>
        /// This is the amount of cash that we have received.
        /// This does not include the value of the coupons, but includes tips
        /// </summary>
        public string BrutoIncome
        {
            get
            {
                return mBrutoIncome + "€";
            }
        }

        private decimal mCash;
        /// <summary>
        /// The amount of cash that we should have, including the start amount and tips
        /// </summary>
        public string Cash
        {
            get
            {
                return mCash + "€";
            }
        }

        public void AddOrder(Order order)
        {
            if (order != null)
            {
                mList.Add(order);
                mReceived += order.Betaald;
                mTotalSum += order.TotalPrice;
                mVoucherCount += order.WaardeBonCount;
               
                UpdateCash();//will also update the net income
                OnPropertyChanged("Received");
                OnPropertyChanged("TotalSum");
                OnPropertyChanged("VoucherCount");
            }
        }

        private void UpdateNetIncome()
        {
            decimal net = mTotalSum - mVoucherCount * ProductList.WAARDE_BON_VALUE;
            if (mNetIncome != net)
            {
                mNetIncome = net;
                OnPropertyChanged("NetIncome");
            }
        }

        /// <summary>
        /// Updates the total Cash, this will also update the net income as that is needed to calculate the total cash
        /// </summary>
        private void UpdateCash()
        {
            UpdateNetIncome();
            decimal cash = mStartCash + mReceived;
            if (cash != mCash)
            {
                mCash = cash;
                OnPropertyChanged("Cash");
            }
        }
        //private void

        internal void SaveToXml(string fileName)
        {
            XmlTextWriter writer = new XmlTextWriter(fileName, null);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument(true);
            writer.WriteStartElement("Eetfestijn");
            writer.WriteAttributeString("date", DateTime.Now.Date.ToShortDateString());
            writer.WriteAttributeString("orderCount", mList.Count + "");
            writer.WriteAttributeString("startKassa", mStartCash + "");
            writer.WriteAttributeString("totaal", mTotalSum + "");
            writer.WriteAttributeString("waardebonnen", mVoucherCount + "");
            writer.WriteAttributeString("inkomsten", mNetIncome + "");
            writer.WriteAttributeString("ontvangen", mReceived + "");
            writer.WriteAttributeString("eindKassa", mCash + "");
            writer.WriteStartElement("Orders");
            foreach (Order o in mList)
                o.SaveToXml(writer);

            writer.WriteEndElement(); //end element for orders
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();

        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        internal void ReadFromXml(string filename)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filename);
            XmlElement element = doc.DocumentElement; //eetfestijn

            
            //XmlAttributeCollection attributes = element.Attributes;
            XmlNodeList list = element.GetElementsByTagName("Order");
            //list.
            IEnumerator enumerator = list.GetEnumerator();
            while (enumerator.MoveNext())
            {
                XmlElement node = (XmlElement)enumerator.Current;
                AddOrder(new Order(node, App.mProductList.Products));
            }
        }
    }
}
