using be.berghs.nils.EetFestijnLib.Helpers;
using be.berghs.nils.EetFestijnLib.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace be.berghs.nils.EetFestijnLib.ViewModels
{
    public class PaymentViewModel : DialogViewModelBase
    {

        private Order Order { get; }

        private decimal VoucherValue { get; }

        public decimal TotalPrice => Order.TotalPrice;

        private int? _VoucherCount;
        public int? VoucherCount
        {
            get => _VoucherCount;
            set
            {
                if (SetProperty(ref _VoucherCount, value))
                    OnPropertyChanged(nameof(NettoPrice));
            }
        }

        public decimal NettoPrice
        {
            get
            {
                if (VoucherCount == null)
                    return TotalPrice;
                return TotalPrice - VoucherCount.Value * VoucherValue;
            }
        }

        private decimal? _Payed;
        /// <summary>
        /// The amount payed by the customer
        /// </summary>
        public decimal? Payed
        {
            get => _Payed;
            set
            {
                if (SetProperty(ref _Payed, value))
                {
                    OnPropertyChanged(nameof(Change));
                    OkCommand.ChangeCanExecute();
                }
            }
        }

        /// <summary>
        /// The change the customer gets
        /// </summary>
        public decimal? Change => Payed - NettoPrice;

        /// <summary>
        /// True if the customer says we can keep the change
        /// </summary>
        public bool KeepChange { get; set; }

        public PaymentViewModel(Order order, decimal voucherValue)
        {
            Order = order;
            VoucherValue = voucherValue;
        }

        protected override bool CanConfirm()
        {
            return Payed.HasValue && Payed.Value >= NettoPrice;
        }
    }
}
