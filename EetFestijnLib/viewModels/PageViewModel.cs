using be.berghs.nils.EetFestijnLib.interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace be.berghs.nils.EetFestijnLib.viewModels
{
    public abstract class PageViewModel:ViewModelBase
    {
        protected IViewFactory ViewFactory { get; private set; }
        
        protected PageViewModel(IViewFactory viewFactory)
        {
            ViewFactory = viewFactory;
        }
    }
}
