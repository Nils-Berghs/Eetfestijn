using be.berghs.nils.EetFestijnLib.viewModels;
using be.berghs.nils.EetFestijnWPF.Classes;
using be.berghs.nils.EetFestijnWPF.Pages;
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

namespace be.berghs.nils.EetFestijnWPF
{
    /// <summary>
    /// Interaction logic for NavigationWindow.xaml
    /// </summary>
    public partial class AppWindow : StackViewWindow
    {
        ViewFactory _ViewFactory;
        
        public AppWindow():base(new StartPage())
        {
            _ViewFactory = new ViewFactory(this);
            MainViewModel mainViewModel = new MainViewModel(_ViewFactory);
            
            CurrentPage.DataContext = mainViewModel;
            InitializeComponent();

        }
    }
}
