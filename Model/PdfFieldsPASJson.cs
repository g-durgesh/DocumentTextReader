using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentTextReader.Model
{
    public class PdfFieldsPASJson
    {
        public string AddressOfShareHolder { get; set; }
        public string NationalityOfShareHolder { get; set; }
        public string NumberOfShares { get; set; }
        public string PercentShareHolding { get; set; }
        public string TotalAmountPaidIncludingPremium { get; set; }
    }
}
