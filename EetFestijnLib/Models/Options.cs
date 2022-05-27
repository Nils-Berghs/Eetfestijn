using System;
using System.Collections.Generic;
using System.Text;

namespace be.berghs.nils.EetFestijnLib.Models
{
    internal class Options
    {
        public bool UseVouchers { get; set; }

        public decimal VoucherValue { get; set; }

        public bool UseMobilePayments { get; set; }
    }
}
