using System;
using System.Collections.Generic;
using System.Text;

namespace be.berghs.nils.EetFestijnLib.Models
{
    public class Options
    {
        public Options()
        {
        }

        public Options(bool useVouchers, string voucherValue, bool useMobilePayments)
        {
            UseVouchers = useVouchers;
            if (!string.IsNullOrWhiteSpace(voucherValue))
                VoucherValue = decimal.Parse(voucherValue);
            UseMobilePayments = useMobilePayments;
        }

        public bool UseVouchers { get; set; }

        public decimal? VoucherValue { get; set; }

        public bool UseMobilePayments { get; set; }
    }
}
