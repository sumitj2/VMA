using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Abstraction.VMA.Models
{
    public class PDFYearlyData
    {
        public int? SrNo { get; set; }
        public string? VendorName { get; set; }
        public string? ServiceName { get; set; }
        public decimal? SanctionedAmount { get; set; }
        public decimal? PaidAmount { get; set; }
        public decimal? PendingAmount { get; set; }

    }
}
