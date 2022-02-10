using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace be.berghs.nils.EetFestijnLib.Helpers
{
    internal static class StringToDecimalHelper
    {
        internal static string CheckDecimalString(string value, string fallback, string format = "N1", bool allowTrailingDecimalSeparator = false)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            string decimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;

            //replace dot and comma by decimal separator
            string newText = value.Trim().Replace(",", decimalSeparator).Replace(".", decimalSeparator);

            //if the string if a valid text return it else return the fallback
            if (decimal.TryParse(newText, out decimal d))
            {
                //if the text ends with a decimal separator allow that
                if (allowTrailingDecimalSeparator && newText.EndsWith(decimalSeparator))
                    return newText;
                return d.ToString(format);
            }
            return fallback;
            
        }
    }
}
