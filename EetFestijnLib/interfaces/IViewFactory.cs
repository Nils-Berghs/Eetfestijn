using be.berghs.nils.EetFestijnLib.viewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace be.berghs.nils.EetFestijnLib.interfaces
{
    public interface IViewFactory
    {
        /// <summary>
        /// Creates a view for a given baseViewModel implementation
        /// </summary>
        /// <param name="viewModel"></param>
        void CreateView(ViewModelBase viewModel);

        void PopView();
    }
}
