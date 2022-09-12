using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EetfestijnUI_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //probably this is the error after closing the dialog the window obtains focus..
            //we must somehow revert this

            Debug.WriteLine("Window Focus changed to "+e.NewValue);
        }

        private void Window_IsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine("Window Focus Within changed to "+e.NewValue);
        }
    }
}
