using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.VMA.Entities.CustomEntities
{
    public class PaymentNotesWithInvoiceServiceDetails:VenderPaymentNote
    {
        public int InvoiceId { get; set; }
        public string? InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string? InvoiceParticulars { get; set; }
        public int VendorServiceId { get; set; }
        public string? VendorServiceName { get; set; }

        public int VendorPaymentId { get; set; }

        public string? PaymentCode { get; set; }
    }
}
