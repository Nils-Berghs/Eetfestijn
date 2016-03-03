using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using be.berghs.nils.eetfestijn.classes;
using Microsoft.Win32;

namespace be.berghs.nils.eetfestijn
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public static OrderList mOrderList = new OrderList();
        public static ProductList mProductList = new ProductList();
        [STAThread]
        public static void Main()
        {
            mProductList.ReadFromXml(GetProductListFileName());
            var application = new App();
            application.InitializeComponent();
            application.Run();
            SaveData();
        }

        private static string GetProductListFileName()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\H6-Eetfestijn\\Products.xml";
        }
        
       
        private static void SaveData()
        {
            mProductList.SaveToXml(GetProductListFileName());

            while (true)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                DateTime dt = DateTime.Now;

                sfd.FileName = "Eetfestijn-" + dt.Year + "-" + dt.Month.ToString("00") + "-" + dt.Day.ToString("00") + ".xml";
                if ((bool)sfd.ShowDialog() == true)
                {
                    mOrderList.SaveToXml(sfd.FileName);
                    break;
                }
                if (MessageBox.Show("Afsluiten zonder resultaten op te slaan?", "Afsluiten?", MessageBoxButton.OKCancel, MessageBoxImage.Question, MessageBoxResult.Cancel)== MessageBoxResult.OK)
                    break;
            }
        }

        
    }
}
