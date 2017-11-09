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
        public int? Aantal 
        {
            get
            {
                if (mAantal == 0)
                    return null;
                return mAantal;
            }
            set
            {
                if (Aantal != value)
                {
                    if (value == null)
                        mAantal = 0;
                    else
                        mAantal = value.Value;
                    UpdateTotal();
                    OnPropertyChanged("AantalString");
                }
            }

        }

        public string AantalString
        {
            get
            {
                if (Aantal.HasValue)
                    return Aantal + "";
                return "";
            }
        }

        /// <summary>
        /// This is a helper method to do maths with the nullable int
        /// </summary>
        /// <param name="aantal"></param>
        internal void AddToAantal(int? aantal)
        {
            if (!aantal.HasValue)
                return;
            if (Aantal.HasValue)
                Aantal += aantal;
            else
                Aantal = aantal;
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
            if (Aantal == null)
                TotalPrice = 0;
            else
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
            if (!Aantal.HasValue)
                return; //do not write empty items to xml
            writer.WriteStartElement("Item");
            writer.WriteAttributeString("product", ProductName);
            writer.WriteAttributeString("aantal", Aantal + "");
            writer.WriteAttributeString("totaal", TotalPrice+"");
            writer.WriteEndElement();
        }
    }
}
