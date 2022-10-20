using be.berghs.nils.EetFestijn.UI.WPF.Helpers.Dialog;
using be.berghs.nils.EetFestijnLib.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace EetfestijnUI_WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            //WPF is by default culture unaware, and than falls back the US culture.... this line of code makes each and every control in the app aware of the current culture
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            MainViewModel mainViewModel = new();
            mainViewModel.PushViewModel(new StartViewModel(mainViewModel, new MaterialDesignDialogService(), new WpfWindowService()));

            MainWindow window = new();
            window.DataContext = mainViewModel;

            window.Show();
        }
    }
}
