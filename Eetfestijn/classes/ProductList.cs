using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Xml;
using System.IO;
using System.Collections;
using be.berghs.nils.eetfestijn.Exceptions;

namespace be.berghs.nils.eetfestijn.classes
{
    public class ProductList
    {
        public static decimal WAARDE_BON_VALUE = 2.5m;

        public ObservableCollection<Product> Products
        {
            get;
            set;
        }

        public ProductList()
        {
            Products = new ObservableCollection<Product>();
            
        }

        internal void SaveToXml(string fileName)
        {
            FileInfo f = new FileInfo(fileName);
            Directory.CreateDirectory(f.DirectoryName);

            XmlTextWriter writer = new XmlTextWriter(fileName, null);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument(true);
            writer.WriteStartElement("ProductList");
            writer.WriteAttributeString("date", DateTime.Now.Date.ToShortDateString());
            writer.WriteAttributeString("waardebon", WAARDE_BON_VALUE+"");
            writer.WriteStartElement("Products");
            foreach (Product p in Products)
                p.SaveToXml(writer);

            writer.WriteEndElement(); //end element for producs
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();


        }

        internal void ReadFromXml(string filename)
        {
            Products.Clear();
            if (!File.Exists(filename))
            {
                InitializeDefaultProducts(filename);
            }
            else
            {
                try
                {
                    ReadProductsFromXml(filename);
                }
                catch(IncompatibleProductsException)
                {
                    InitializeDefaultProducts(filename);
                }
            }

        }

        private void InitializeDefaultProducts(string filename)
        {
            FillListWithDefaultProducts();
            SaveToXml(filename);
        }

        /// <summary>
        /// Reads the products from a given file name.
        /// This function does not check on the existence of the file
        /// </summary>
        /// <param name="filename"></param>
        private void ReadProductsFromXml(string filename)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filename);
            XmlElement element = doc.DocumentElement; //de productlist
            WAARDE_BON_VALUE= decimal.Parse(element.GetAttribute("waardebon"));
            //XmlAttributeCollection attributes = element.Attributes;
            XmlNodeList list = element.GetElementsByTagName("Product");
            
            IEnumerator enumerator = list.GetEnumerator();
            while(enumerator.MoveNext())
            {
                XmlAttributeCollection product = ((XmlNode)enumerator.Current).Attributes;
                Products.Add(new Product(product));
            }
            
            

        }

        private void FillListWithDefaultProducts()
        {
            Products.Add(new Product("Zalm (groot)", 15, ProductType.Eten));
            Products.Add(new Product("Zalm (klein)", 8, ProductType.Eten));
            Products.Add(new Product("VarkensHaasje (groot)", 14, ProductType.Eten));
            Products.Add(new Product("VarkensHaasje (klein)", 8, ProductType.Eten));
            Products.Add(new Product("Balletjes (groot)", 13, ProductType.Eten));
            Products.Add(new Product("Balletjes (kinder)", 7, ProductType.Eten));
            Products.Add(new Product("Vegetarisch (groot)", 12, ProductType.Eten));
            Products.Add(new Product("Vegetarisch (klein)", 7, ProductType.Eten));
            Products.Add(new Product("Cava (glas)", 3, ProductType.Drank));
            Products.Add(new Product("Cava (fles)", 15, ProductType.Drank));
            Products.Add(new Product("Spa bruis", 1.8m, ProductType.Drank));
            Products.Add(new Product("Spa plat", 1.8m, ProductType.Drank));
            Products.Add(new Product("Fruitsap", 1.8m, ProductType.Drank));
            Products.Add(new Product("Limonade", 1.8m, ProductType.Drank));
            Products.Add(new Product("Cola", 1.8m, ProductType.Drank));
            Products.Add(new Product("Cola light", 1.8m, ProductType.Drank));
            Products.Add(new Product("Ice-Tea", 1.8m, ProductType.Drank));
            Products.Add(new Product("Pils", 1.8m, ProductType.Drank));
            Products.Add(new Product("Hoegaarden", 2m, ProductType.Drank));
            Products.Add(new Product("Leffe blond", 2.5m, ProductType.Drank));
            Products.Add(new Product("Leffe bruin", 2.5m, ProductType.Drank));
            Products.Add(new Product("Duvel", 3m, ProductType.Drank));
            Products.Add(new Product("Witte wijn (glas)", 2.5m, ProductType.Drank));
            Products.Add(new Product("Rode wijn (glas)", 2.5m, ProductType.Drank));
            Products.Add(new Product("Witte wijn (fles)", 12m, ProductType.Drank));
            Products.Add(new Product("Rode wijn (fles)", 12m, ProductType.Drank));
            Products.Add(new Product("Koffie", 1.8m, ProductType.Dessert));
            Products.Add(new Product("Verwenkoffie", 5m, ProductType.Dessert));
            Products.Add(new Product("Kinderijs", 1.5m, ProductType.Dessert));
            Products.Add(new Product("Dame blanche", 4, ProductType.Dessert));
        }

        internal Product GetProductByName(string name)
        {
            foreach (Product p in Products)
                if (p.Name.Equals(name))
                    return p;
            return null;
        }
    }
}
