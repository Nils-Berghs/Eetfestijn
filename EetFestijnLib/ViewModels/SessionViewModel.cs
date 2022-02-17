using be.berghs.nils.EetFestijnLib.Helpers.Dialog;
using be.berghs.nils.EetFestijnLib.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace be.berghs.nils.EetFestijnLib.ViewModels
{
    public class SessionViewModel : PageViewModel
    {
        private ProductList ProductList { get; }

        public SessionViewModel(StackViewModel<PageViewModel> stackViewModel, IDialogService dialogService, ProductList productList):base(stackViewModel, dialogService)
        {
            ProductList = productList;
        }
    }
}
