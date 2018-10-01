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
    /// Interaction logic for OrderSummary.xaml
    /// </summary>
    public partial class OrderSummary : Window
    {
        private Order _CurrentOrder;

        public OrderSummary()
        {
            InitializeComponent();
        }

        internal void SetOrder(Order currentOrder)
        {
            _CurrentOrder = currentOrder;
            this.DataContext = _CurrentOrder;
            SetPayment(null);
            CurrentOrderFoodControl.ItemsSource = currentOrder.FoodItems;
            CurrentOrderDessertControl.ItemsSource = currentOrder.DessertItems;
            CurrentOrderDrinksControl.ItemsSource = currentOrder.DrinkItems;
        }

        internal void SetPayment(Payment betaling)
        {
            if (betaling == null)
                SummaryGrid.Visibility = Visibility.Hidden;
            else
                SummaryGrid.Visibility = Visibility.Visible;
            SummaryGrid.DataContext = betaling;
        }
    }
}
