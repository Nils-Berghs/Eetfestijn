using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml;

namespace be.berghs.nils.eetfestijn.classes
{
    public class OrderItem: INotifyPropertyChanged
    {
        

        private Product Product { get; set; }

        [DisplayName("Product")]
        public string ProductName
        {
            get
            {
                return Product.ToString();
            }
        }

        public ProductType ProductType
        {
            get
            {
                return Product.ProductType;
            }
        }

        private int mAantal = 0;
        public int Aantal 
        {
            get
            {
                return mAantal;
            }
            set
            {
                if (mAantal != value)
                {
                    mAantal = value;
                    UpdateTotal();
                }
            }

        }

        [DisplayName("Prijs")]
        public decimal Price
        {
            get
            {
                return Product.Price;
            }
        }


        private decimal mTotalPrice;
        [DisplayName("Totaal")]
        public decimal TotalPrice
        {
            get
            {
                return mTotalPrice;
            }
            private set
            {
                if (mTotalPrice != value)
                {
                    mTotalPrice = value;
                    OnPropertyChanged("TotalPrice");
                }
            }
        }

        public OrderItem(Product product)
        {
            Aantal = 0;
            Product = product;
        }

        

        private void UpdateTotal()
        {
            TotalPrice = (decimal)Aantal * Product.Price;
        }

        public override bool Equals(object obj)
        {
            if (obj is OrderItem)
            {
                return Product.Equals(((OrderItem)obj).Product);
            }
            if (obj is string)
            {
                return Product.Equals(obj.ToString());
            }
            return false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

        }

        internal void SaveToXml(XmlTextWriter writer)
        {
            if (Aantal == 0)
                return; //do not write empty items to xml
            writer.WriteStartElement("Item");
            writer.WriteAttributeString("product", ProductName);
            writer.WriteAttributeString("aantal", Aantal+"");
            writer.WriteAttributeString("totaal", TotalPrice+"");
            writer.WriteEndElement();
        }
    }
}
