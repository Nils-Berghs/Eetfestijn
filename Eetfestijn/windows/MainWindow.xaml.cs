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
        private OrderSummary mOrderSummaryWindow;

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

            mOrderSummaryWindow = new OrderSummary();
            mOrderList = App.mOrderList;
            mProductList = App.mProductList;
            InitializeComponent();
            //DataContext = this;
            Producten.ItemsSource = mProductList.Products;

            tabStatistiek.DataContext = this;

            CreateNewOrder();
            
            
        }

        private void CreateNewOrder()
        {
            CurrentOrder = new Order(mProductList.Products);
            //bestellingFood.ItemsSource = CurrentOrder.Items;
            CurrentOrderDrinksControl.ItemsSource = CurrentOrder.DrinkItems;
            CurrentOrderFoodControl.ItemsSource = CurrentOrder.FoodItems;
            CurrentOrderDessertControl.ItemsSource = CurrentOrder.DessertItems;

            mOrderSummaryWindow.SetOrder(CurrentOrder);
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            btnOK.Focus(); //make sure a leave is initiated from the last field
            if (!IsValid(this))
                return;
            Payment betaling = new Payment(CurrentOrder);
            if ((bool)betaling.ShowDialog())
            {
                
                mOrderList.AddOrder(CurrentOrder);
                CreateNewOrder();
            }
        }

        public static bool IsValid(DependencyObject parent)
        {
            if (Validation.GetHasError(parent))
                return false;

            // Validate all the bindings on the children
            for (int i = 0; i != VisualTreeHelper.GetChildrenCount(parent); ++i)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (!IsValid(child)) { return false; }
            }

            return true;
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

        private void thisWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //System.Windows.Forms.SystemInformation.MonitorCount
            var screens = System.Windows.Forms.Screen.AllScreens;
            if (screens.Length > 1)
            {
                System.Windows.Forms.Screen s2 = screens[1];
                System.Drawing.Rectangle r2 = s2.WorkingArea;
                mOrderSummaryWindow.Top = r2.Top;
                mOrderSummaryWindow.Left = r2.Left;

                mOrderSummaryWindow.Show();
            }
            /*else
            {
                mOrderSummaryWindow.WindowState = WindowState.Normal;
                System.Windows.Forms.Screen s1 = screens[0];
                System.Drawing.Rectangle r1 = s1.WorkingArea;
                mOrderSummaryWindow.Top = r1.Top;
                mOrderSummaryWindow.Left = r1.Width/2;
                mOrderSummaryWindow.Show();
            }*/

       
        

        }

        private void thisWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mOrderSummaryWindow.Close();
        }
    }
}
