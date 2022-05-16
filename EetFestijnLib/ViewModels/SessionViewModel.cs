using be.berghs.nils.EetFestijnLib.Helpers;
using be.berghs.nils.EetFestijnLib.Helpers.Dialog;
using be.berghs.nils.EetFestijnLib.Helpers.Events;
using be.berghs.nils.EetFestijnLib.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace be.berghs.nils.EetFestijnLib.ViewModels
{
    public class SessionViewModel : PageViewModel
    {
        private string SessionFileName { get; set; }

        private ProductList ProductList { get; }

        public OrderViewModel CurrentOrder { get; }

        public OrderList OrderList { get; }

        public SessionViewModel(StackViewModel<PageViewModel> stackViewModel, IDialogService dialogService, ProductList productList):base(stackViewModel, dialogService)
        {
            CreateSessionFile();

            OrderList = new OrderList();
            OrderList.OrderAdded += OrderListOrderAdded;

            ProductList = productList;
            CurrentOrder = new OrderViewModel(dialogService, ProductList, OrderList);
        }

        private void CreateSessionFile()
        {
            SessionFileName = "Session " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ".json";
            string path = FileSystemHelper.GetTempPath(SessionFileName);
            FileInfo fileInfo = new FileInfo(path);
            Directory.CreateDirectory(fileInfo.DirectoryName);
            //Todo write json
            File.WriteAllText(path, JsonConvert.SerializeObject(productList, Formatting.Indented));

        
        }

        private void OrderListOrderAdded(object sender, OrderAddedEventArgs e)
        {
            //Todo save the order to file
        }


    }
}
