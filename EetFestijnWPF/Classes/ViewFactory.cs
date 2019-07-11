using be.berghs.nils.EetFestijnLib.interfaces;
using be.berghs.nils.EetFestijnLib.viewModels;
using be.berghs.nils.EetFestijnWPF.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace be.berghs.nils.EetFestijnWPF.Classes
{
    class ViewFactory : IViewFactory
    {
        private StackViewWindow _StackViewWindow;

        internal ViewFactory(StackViewWindow navigationWindow)
        {
            _StackViewWindow = navigationWindow;
        }

        public void CreateView(BaseViewModel viewModel)
        {
            
            _StackViewWindow.PushPage(new MenuPage());
           // throw new NotImplementedException();
            //_NavigationWindow
           // _NavigationWindow.NavigationService.Navigate
        }
    }
}
