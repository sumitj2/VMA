using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.VMA.Entities.CustomEntities
{
    public class CreateWordDocumentPaymentNote : ExportPaymentNoteData
    {
        public string? RatePerUnit { get; set; }

        public int? QuantityOfUnit { get; set; }

        public string? CreatiionDate { get; set; }
        public DateOnly? SantionedDate { get; set; }
        public string? UTRNo { get; set; }      

        public decimal? TotalGST { get; set; }

        public decimal? TotalAmountPaid { get; set; }
        public decimal? VendorPaymentCgst { get; set; }

        public decimal? VendorPaymentSgst { get; set; }

        public decimal? VendorPaymentIgst { get; set; }
        public int? PaymentNoteId { get; set; }
    }
}
