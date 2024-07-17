using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Abstraction.VMA.Models
{
    public class VendorPayments
    {
        public decimal TotalPaymentNotTaxable { get; set; }
        public string? Meassage { get; set; } 
    }
}
