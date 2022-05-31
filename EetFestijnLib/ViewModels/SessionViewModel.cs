using be.berghs.nils.EetFestijnLib.Helpers;
using be.berghs.nils.EetFestijnLib.Helpers.Dialog;
using be.berghs.nils.EetFestijnLib.Helpers.Events;
using be.berghs.nils.EetFestijnLib.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace be.berghs.nils.EetFestijnLib.ViewModels
{
    public class SessionViewModel : PageViewModel
    {
        private Session Session { get; }
        
        public OrderViewModel CurrentOrder { get; }

        public int OrderCount => Session.OrderCount;
        
        private int _PlateCount;
        public int PlateCount
        {
            get => _PlateCount;
            private set => SetProperty(ref _PlateCount, value);
        }

        private decimal _TotalIncome;
        /// <summary>
        /// The theoretical income, vouchers included
        /// </summary>
        public decimal TotalIncome
        {
            get => _TotalIncome;
            private set => SetProperty(ref _TotalIncome, value);
        }

        public SessionViewModel(StackViewModel<PageViewModel> stackViewModel, IDialogService dialogService, Session session):base(stackViewModel, dialogService)
        {
            Session = session;

            Session.OrderList.OrderAdded += OrderListOrderAdded;
                        
            CurrentOrder = new OrderViewModel(dialogService, Session);

            //recalculate the totals, this is usefull when opening an existing session
            RecalculateTotals();
        }

       

        private void OrderListOrderAdded(object sender, OrderAddedEventArgs e)
        {
            RecalculateTotals();

            _ = SaveOrder(e.Order);
        }

        private async Task SaveOrder(Order order)
        {
            string orderPath = FileSystemHelper.GetSessionPath(Session, "Order-"+order.OrderId+ ".json");
            using (var sw = new StreamWriter(orderPath))
            {
                await sw.WriteAsync(JsonConvert.SerializeObject(order, Formatting.Indented));
            }
        }

        private void RecalculateTotals()
        {
            int plateCount = 0;
            decimal totalIncome = 0;
            foreach (var order in Session.OrderList.Orders)
            {
                totalIncome += order.TotalPrice;
                foreach (var item in order.Foods)
                    plateCount += item.Count;
            }

            PlateCount = plateCount;
            TotalIncome = totalIncome;

            OnPropertyChanged(nameof(OrderCount));
        }
    }
}
