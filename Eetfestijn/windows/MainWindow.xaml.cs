using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using be.berghs.nils.eetfestijn.classes;
using Microsoft.Win32;

namespace be.berghs.nils.eetfestijn.windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public OrderList mOrderList = null;

        public OrderList OrderList
        {
            get
            {
                return mOrderList;
            }
        }

        public ProductList mProductList = null;

        public Order CurrentOrder
        {
            get;
            set;
        }


        public MainWindow()
        {

            InitializeComponent();
            mOrderList = App.mOrderList;
            mProductList = App.mProductList;
            
            //DataContext = this;
            Producten.ItemsSource = mProductList.Products;

            //tabStatistiek.DataContext = mOrderList;

            CreateNewOrder();
            
            
        }

        private void CreateNewOrder()
        {
            CurrentOrder = new Order(mProductList.Products);
            //bestellingFood.ItemsSource = CurrentOrder.Items;
            CurrentOrderItemsControl.ItemsSource = CurrentOrder.Items;
            CurrentOrderFoodControl.ItemsSource = CurrentOrder.FoodItems;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            Payment betaling = new Payment(CurrentOrder);
            if ((bool)betaling.ShowDialog())
            {
                mOrderList.AddOrder(CurrentOrder);
                CreateNewOrder();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            CreateNewOrder();
        }

        internal void SetOrderList(OrderList orderList)
        {
            mOrderList = orderList;
        }

        private void bestellingFood_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down || e.Key == Key.Up)
            {
                //bestellingFood.CommitEdit();
                
            }
        }

        private void tabBestelling_Selected(object sender, RoutedEventArgs e)
        {
            CreateNewOrder();
        }

        private void btnOrderOverview_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnConsumptionOverview_Click(object sender, RoutedEventArgs e)
        {
            ConsumptionOverview overView = new ConsumptionOverview();
            overView.ShowDialog();
        }

        private void btnLoadData_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if ((bool)ofd.ShowDialog() == true)
            {
                mOrderList.ReadFromXml(ofd.FileName);
            }
        }

        private void btnLoadProducts_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if ((bool)ofd.ShowDialog() == true)
            {
                App.ReadProductListFromXml(ofd.FileName);
            }
        }
    }
}
