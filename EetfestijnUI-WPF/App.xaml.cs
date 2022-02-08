using be.berghs.nils.EetFestijn.UI.WPF.Helpers.Dialog;
using be.berghs.nils.EetFestijnLib.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace EetfestijnUI_WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void ApplicationStartup(object sender, StartupEventArgs e)
        {

            MainViewModel mainViewModel = new();
            mainViewModel.PushViewModel(new StartViewModel(mainViewModel, new MaterialDesignDialogService()));

            MainWindow window = new();
            window.DataContext = mainViewModel;

            window.Show();
        }
    }
}
