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

        public SessionViewModel(StackViewModel<PageViewModel> stackViewModel, IDialogService dialogService, Session session):base(stackViewModel, dialogService)
        {
            Session = session;
            Session.OrderAdded += SessionOrderAdded;

                        
            CurrentOrder = new OrderViewModel(dialogService, Session);

        }

        private void SessionOrderAdded(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(OrderCount));
            OnPropertyChanged(nameof(PlateCount));
            OnPropertyChanged(nameof(TotalIncome));
        }
    }
}
