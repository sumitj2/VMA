using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.VMA.Entities.CustomEntities
{
    public class VendorPaymentWithService : VendorPayment
    {
        public int? NoteId { get; set; }
        public string PaymentNoteNo { get; set; } = null!;
        public int? VendorId { get; set; }
        public string? VendorName { get; set; }
        public int? VendorServiceId { get; set; }
        public string? VendorServiceName { get; set; }
        public decimal? ServiceSantionAmount { get; set; }
        public string? ServicePaymentType { get; set; }

        public int? InvoiceId { get; set; }

        public string? InvoiceNumber { get; set; }

        public DateTime? InvoiceDate { get; set; }

        public string? InvoiceParticulars { get; set; }
    }
}
