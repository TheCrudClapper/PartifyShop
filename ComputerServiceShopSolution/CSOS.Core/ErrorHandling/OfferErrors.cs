using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSOS.Core.ErrorHandling
{
    public class OfferErrors
    {
        public static readonly Error OfferIsNull = new Error(
             "Offer.OfferIsNull", "Given offer is null");

        public static readonly Error OfferNotFound = new Error(
            "Offer.OfferNotFound", "Offer not found");
    }
}
