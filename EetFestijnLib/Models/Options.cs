using Newtonsoft.Json;
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

        public Options(bool useVouchers, string voucherValue, bool useMobilePayments, bool showOrderSummary)
        {
            UseVouchers = useVouchers;
            if (!string.IsNullOrWhiteSpace(voucherValue))
                VoucherValue = decimal.Parse(voucherValue);
            UseMobilePayments = useMobilePayments;
            ShowOrderSummary = showOrderSummary;
        }

        public bool UseVouchers { get; set; }

        public decimal? VoucherValue { get; set; }

        public bool UseMobilePayments { get; set; }

        /// <summary>
        /// Gets or Sets if the order summary should be shown on a secondary screen
        /// </summary>
        [JsonIgnore]
        public bool ShowOrderSummary { get; set; }
    }
}
