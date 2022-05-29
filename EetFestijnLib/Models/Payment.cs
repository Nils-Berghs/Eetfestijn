using System;
using System.Collections.Generic;
using System.Text;

namespace be.berghs.nils.EetFestijnLib.Models
{
    public class Payment
    {
        public decimal TotalPrice { get; set; }

        public decimal VoucherCount { get; set; }

        public decimal NettoPrice { get; set; }

        public bool MobilePayment { get; set; }

        /// <summary>
        /// A tip with 't is just'
        /// </summary>
        public decimal Tip { get; set; }
    }
}
