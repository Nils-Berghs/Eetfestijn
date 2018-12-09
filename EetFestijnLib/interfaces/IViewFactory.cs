using be.berghs.nils.EetFestijnLib.viewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace be.berghs.nils.EetFestijnLib.interfaces
{
    public interface IViewFactory
    {
        void CreateView(BaseViewModel viewModel);
    }
}
