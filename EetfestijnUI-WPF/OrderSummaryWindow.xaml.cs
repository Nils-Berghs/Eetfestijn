using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace be.berghs.nils.EetFestijn.UI.WPF
{
    /// <summary>
    /// Interaction logic for OrderSummaryWindow.xaml
    /// </summary>
    public partial class OrderSummaryWindow : Window
    {
        public OrderSummaryWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.Manual;
            
            /* This shows the summary window on the secondary screen
             * 
             */
            Loaded += WindowLoaded;
            var secondaryScreenLeft = Math.Abs(SystemParameters.PrimaryScreenWidth - SystemParameters.VirtualScreenWidth);
            Left = secondaryScreenLeft;
            Top = 0;
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            
            WindowState = WindowState.Maximized;
            
        }
                
    }
}
