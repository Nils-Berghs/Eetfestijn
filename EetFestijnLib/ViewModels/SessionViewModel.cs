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
        internal Session Session { get; }
        
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

        public SessionViewModel(StackViewModel<PageViewModel> stackViewModel, IDialogService dialogService, IWindowService windowService, Session session):base(stackViewModel, dialogService, windowService)
        {
            Session = session;
            Session.OrderAdded += SessionOrderAdded;
            
            CurrentOrder = new OrderViewModel(dialogService, windowService, Session);

        }

        private void SessionOrderAdded(object sender, EventArgs e)
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
