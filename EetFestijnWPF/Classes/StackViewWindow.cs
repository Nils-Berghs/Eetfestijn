using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace be.berghs.nils.EetFestijnWPF.Classes
{
    /// <summary>
    /// This class is created as WPF lacks something like a navigation controller on which you can push and pop views
    /// </summary>
    public class StackViewWindow: NavigationWindow
    {
        private readonly Stack<Page> _Pages;
        public Page CurrentPage
        {
            get
            {
                if (_Pages == null || _Pages.Count <= 0)
                    return null;
                return _Pages.Peek();
            }
        }

        public StackViewWindow()
        {
            ShowsNavigationUI = false;
            _Pages = new Stack<Page>();
        }

        public StackViewWindow(Page initialPage):this()
        {
            PushPage(initialPage);
        }

        public void PushPage(Page page)
        {
            _Pages.Push(page);
            base.Navigate(page);
        }

        public void PopPage()
        {
            _Pages.Pop();
            base.NavigationService.Navigate(_Pages.Peek());
        }
    }
}
