using be.berghs.nils.EetFestijnLib.Helpers;
using be.berghs.nils.EetFestijnLib.Helpers.Dialog;
using be.berghs.nils.EetFestijnLib.Helpers.Events;
using be.berghs.nils.EetFestijnLib.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace be.berghs.nils.EetFestijnLib.ViewModels
{
    public class SessionViewModel : PageViewModel
    {

        private ProductList ProductList { get; }

        private Options Options { get; }

        public OrderViewModel CurrentOrder { get; }

        public OrderList OrderList { get; }

        public SessionViewModel(StackViewModel<PageViewModel> stackViewModel, IDialogService dialogService, ProductList productList, Options options):base(stackViewModel, dialogService)
        {
            ProductList = productList;
            Options = Options;

            OrderList = new OrderList();
            OrderList.OrderAdded += OrderListOrderAdded;
                        
            CurrentOrder = new OrderViewModel(dialogService, ProductList, OrderList);
        }

       

        private void OrderListOrderAdded(object sender, OrderAddedEventArgs e)
        {
            //Todo recalc everything
        }


    }
}
