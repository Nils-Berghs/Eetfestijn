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
        private Session Session { get; }
        
        public OrderViewModel CurrentOrder { get; }

        private int _OrderCount;
        public int OrderCount 
        { 
            get => _OrderCount; 
            private set => SetProperty(ref _OrderCount, value);
        }

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

            //Todo save order to temp file async
        }

        private void RecalculateTotals()
        {
            int orderCount = 0;
            int plateCount = 0;
            decimal totalIncome = 0;
            foreach (var order in Session.OrderList.Orders)
            {
                orderCount++;
                totalIncome += order.TotalPrice;
                foreach (var item in order.Foods)
                    plateCount += item.Count;
            }

            OrderCount = orderCount;
            PlateCount = plateCount;
            TotalIncome = totalIncome;
        }
    }
}
