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
using System.Windows.Input;

namespace be.berghs.nils.EetFestijnLib.ViewModels
{
    public class SessionViewModel : PageViewModel
    {
        internal Session Session { get; }

        public ICommand ExportSessionCommand { get; }

        public ICommand ExportMenuCommand { get; }

        public ICommand ImportSessionCommand { get; }

        public ICommand ShowOrderSummaryCommand => ShowOrderSummaryCommandImplementation;

        private Command ShowOrderSummaryCommandImplementation { get; }

        public OrderViewModel CurrentOrder { get; }

        public int OrderCount => Session.OrderCount;

        public DateTime StartDate => Session.CreatedDateTime;
                
        public int PlateCount => Session.PlateCount;

        /// <summary>
        /// The theoretical income, vouchers included
        /// </summary>
        public decimal TotalIncome => Session.TotalIncome;

        public decimal NettoIncome => Session.NetIncome;

        public decimal CashIncome => Session.CashIncome;

        public decimal MobileIncome => Session.MobileIncome;

        private decimal _StartCash;
        public decimal StartCash 
        { 
            get => _StartCash; 
            set
            {
                if (SetProperty(ref _StartCash, value))
                    OnPropertyChanged(nameof(TotalCash));
            }
        }

        public decimal Tips => Session.Tips;

        public decimal TotalCash => CashIncome + Tips + StartCash;

        public ConsumptionViewModel ConsumptionViewModel { get; }

        public SessionViewModel(StackViewModel<PageViewModel> stackViewModel, IDialogService dialogService, IWindowService windowService, Session session):base(stackViewModel, dialogService, windowService)
        {
            Session = session;
            Session.OrderAdded += SessionOrderAdded;
            Session.OrdersAdded += SessionOrdersAdded;
            ConsumptionViewModel = new ConsumptionViewModel(session);
            CurrentOrder = new OrderViewModel(dialogService, windowService, Session);
            CurrentOrder.SecondScreenClosed += OrderViewModelSecondScreenClosed;

            ShowOrderSummaryCommandImplementation = new Command(ShowOrderSummary, CanShowOrderSummary);
            ExportMenuCommand = new Command(ExportMenu);
            ExportSessionCommand = new Command(ExportSession);
        }

        private void ExportMenu()
        {
            var exportOptions = new ExportOptions("menu", "JSON file|*.json");
            DialogService.ShowSaveFileDialog(exportOptions);
            if (exportOptions.IsConfirmed)
            {
                Session.ExportMenu(exportOptions.FileName);
            }
        }

        private void ExportSession()
        {
            var exportOptions = new ExportOptions(Session.SessionName, "Session file|*.session");
            DialogService.ShowSaveFileDialog(exportOptions);
            if(exportOptions.IsConfirmed)
            {
                Session.Export(exportOptions.FileName);
            }
        }

        private void OrderViewModelSecondScreenClosed(object sender, EventArgs e)
        {
            ShowOrderSummaryCommandImplementation.ChangeCanExecute();
        }

        private void ShowOrderSummary()
        {
            Session.Options.ShowOrderSummary = true;
            ShowOrderSummaryCommandImplementation.ChangeCanExecute();
            CurrentOrder.ShowOrderSummary();
        }

        private bool CanShowOrderSummary()
        {
            return !Session.Options.ShowOrderSummary;
        }

        private void SessionOrdersAdded(object sender, OrdersAddedEventArgs e)
        {
            RaisePropertyChanges();
            foreach (Order o in e.Orders)
                ConsumptionViewModel.AddOrder(o);
        }

        private void SessionOrderAdded(object sender, OrderAddedEventArgs e)
        {
            RaisePropertyChanges();

            ConsumptionViewModel.AddOrder(e.Order);
        }

        private void RaisePropertyChanges()
        {
            OnPropertyChanged(nameof(OrderCount));
            OnPropertyChanged(nameof(PlateCount));
            OnPropertyChanged(nameof(TotalIncome));
            OnPropertyChanged(nameof(NettoIncome));
            OnPropertyChanged(nameof(CashIncome));
            OnPropertyChanged(nameof(MobileIncome));
            OnPropertyChanged(nameof(Tips));
            OnPropertyChanged(nameof(TotalCash));
        }
    }
}
