using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.VMA.Entities.CustomEntities
{
    public class PaymentNotesWithInvoiceServiceDetails : VenderPaymentNote
    {

        public int VendorServiceId { get; set; }
        public string? VendorServiceName { get; set; }
        public int VendorId { get; set; }
        public string? VendorName { get; set; }

    }
}
