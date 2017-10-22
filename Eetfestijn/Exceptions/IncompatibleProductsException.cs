using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace be.berghs.nils.eetfestijn.Exceptions
{
    /// <summary>
    /// Use this exception to indicate that the products that you try to read from disk are incompatible with the current version.
    /// </summary>
    class IncompatibleProductsException : Exception
    {
 
        public IncompatibleProductsException(string message, Exception e):base(message, e)
        {
            
        }

        public IncompatibleProductsException( Exception e) : base("Incompatible product", e)
        {

        }
    }
}
