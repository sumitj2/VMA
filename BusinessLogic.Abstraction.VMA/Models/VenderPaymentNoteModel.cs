using System;
using System.Collections.Generic;

namespace BusinessLogic.Abstraction.VMA.Models;

public partial class VenderPaymentNoteModel
{
    public int InvoiceId { get; set; }
    public string? InvoiceNumber { get; set; }
    public DateTime? InvoiceDate { get; set; }
    public string? InvoiceParticulars { get; set; }
    public int VendorServiceId { get; set; }
    public string? VendorServiceName { get; set; }

    public int VendorPaymentId { get; set; }

    public string? PaymentCode { get; set; }
    public int NoteId { get; set; }

    public string? PaymentNoteNo { get; set; }

    public DateTime? PaymentNoteDate { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public bool? IsActive { get; set; }

    public string? LastUpdateBy { get; set; }

    public DateTime? LastUpdatedDate { get; set; }

    public int? FkVendorPaymentId { get; set; }

    public int? FkInvoiceId { get; set; }
}
