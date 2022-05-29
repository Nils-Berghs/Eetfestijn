using be.berghs.nils.EetFestijnLib.Helpers;
using be.berghs.nils.EetFestijnLib.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace be.berghs.nils.EetFestijnLib.ViewModels
{
    public class PaymentViewModel : DialogViewModelBase
    {

        private Order Order { get; }

        private Options Options { get; }
                
        public bool UseVouchers => Options.UseVouchers;

        public bool UseMobilePayments => Options.UseMobilePayments;

        public decimal TotalPrice => Order.TotalPrice;

        private int? _VoucherCount;
        public int? VoucherCount
        {
            get => _VoucherCount;
            set
            {
                if (SetProperty(ref _VoucherCount, value))
                {
                    OnPropertyChanged(nameof(VoucherDiscount));
                    OnPropertyChanged(nameof(NettoPrice));
                }
            }
        }

        public decimal? VoucherDiscount
        {
            get
            {
                if (VoucherCount == null)
                    return null;
                return VoucherCount.Value * Options.VoucherValue.Value;
            }

        }

        public decimal NettoPrice
        {
            get
            {
                if (VoucherDiscount == null)
                    return TotalPrice;
                return TotalPrice - VoucherDiscount.Value;
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

        /// <summary>
        /// Indicates if the order is payed with mobile
        /// </summary>
        private bool _IsMobilePayment;
        public bool IsMobilePayment 
        {
            get => _IsMobilePayment;
            set
            {
                if (SetProperty(ref _IsMobilePayment, value))
                {
                    Payed = null;
                    OkCommand.ChangeCanExecute();
                }
            }
        }

        public PaymentViewModel(Order order, Options options)
        {
            Order = order;
            Options = options;
        }

        protected override bool CanConfirm()
        {
            return (Payed.HasValue && Payed.Value >= NettoPrice) || IsMobilePayment;
        }

        protected override Task Confirm()
        {
            Order.Payment = new Payment
            {
                TotalPrice = TotalPrice,
                VoucherCount = (decimal)VoucherCount,
                NettoPrice = NettoPrice,
                MobilePayment = IsMobilePayment,
                Tip = KeepChange ? (decimal)Change : 0
            };
            return base.Confirm();
        }
    }
}
