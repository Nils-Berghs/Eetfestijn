using be.berghs.nils.EetFestijnLib.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace be.berghs.nils.EetFestijnLib.ViewModels
{
    public class PaymentViewModel : DialogViewModelBase
    {
        private Order Order { get; }

        public decimal TotalPrice 

        public PaymentViewModel(Order order)
        {
            Order = order;
        }

        
    }
}
