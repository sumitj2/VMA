using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.VMA.Entities.CustomEntities
{
    public class YearlyReportData
    {
        public string? VendorName { get; set; }
        public string? VendorServiceName { get; set; }
        public string? ServicePaymentType { get; set; }
        public decimal? ServiceSanctionAmount { get; set; }
        public int NumberOfTerms { get; set; }
        public decimal? TotalPaymentsMade { get; set; }
        public int RemainingTerms { get; set; }
        public decimal? TotalVendorPaymentAmount { get; set; }
        public decimal? RemainingAmount { get; set; }
    }
}
