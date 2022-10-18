using be.berghs.nils.EetFestijnLib.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace be.berghs.nils.EetFestijnLib.Helpers.Dialog
{
    public interface IWindowService
    {
        /// <summary>
        /// Shows a none model window with the given viewmodel as datacontext
        /// </summary>
        /// <param name="viewModel"></param>
        void ShowWindow(ViewModelBase viewModel);
    }
}
