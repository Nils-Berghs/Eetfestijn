using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.ComponentModel;
using be.berghs.nils.eetfestijn.Exceptions;

namespace be.berghs.nils.eetfestijn.classes
{
    public class Product:INotifyPropertyChanged
    {
        //Products ProductType { get; set; }

        public string Name { get; set; }

        private decimal mPrice = 0;
        public decimal Price 
        {
            get
            {
                return mPrice;
            }
            set
            {
                if (mPrice != value)
                {
                    mPrice = value;

                }
            }
        }

        public ProductType ProductType { get; set; }

        public Product()
        {
            //Dummy constructor for databinding
        }

        public Product(string name, decimal price, ProductType productType)
        {
            ProductType = productType;
            Name = name;
            Price = price;
        }

        public Product(XmlAttributeCollection xmlElement)
        {
            try
            {
                Price = decimal.Parse(xmlElement.GetNamedItem("prijs").Value);
                Name = xmlElement.GetNamedItem("naam").Value;
                ProductType = (ProductType)Enum.Parse(typeof(ProductType),xmlElement.GetNamedItem("type").Value);
            }
            catch(Exception e)
            {
                throw new IncompatibleProductsException(e);
            }
        }

        public override string ToString()
        {
            if (Name == null)
                return "";
            return Name;
        }

        public override bool Equals(object obj)
        {
            if (obj is Product)
                return obj.ToString().Equals(Name);
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }



        internal void SaveToXml(XmlTextWriter writer)
        {
            writer.WriteStartElement("Product");
            writer.WriteAttributeString("naam", Name);
            writer.WriteAttributeString("prijs", Price + "");
            writer.WriteAttributeString("type", ProductType.ToString());
            writer.WriteEndElement();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
