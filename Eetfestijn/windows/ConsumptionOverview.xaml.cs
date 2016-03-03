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
using System.Windows.Shapes;
using be.berghs.nils.eetfestijn.classes;

namespace be.berghs.nils.eetfestijn.windows
{
    /// <summary>
    /// Interaction logic for ConsumptionOverview.xaml
    /// </summary>
    public partial class ConsumptionOverview : Window
    {
        private Order mOrder;

        public ConsumptionOverview()
        {
            InitializeComponent();

            mOrder = new Order(App.mProductList.Products);
            foreach (Order order in App.mOrderList.AllOrders)
            {
                foreach (OrderItem item in order.Items)
                {
                    OrderItem total = mOrder.Items[mOrder.Items.IndexOf(item)];
                    if (total != null)
                        total.Aantal += item.Aantal;
                }
            }
            Consumption.ItemsSource = mOrder.Items;
            
        }

        
    }
}
