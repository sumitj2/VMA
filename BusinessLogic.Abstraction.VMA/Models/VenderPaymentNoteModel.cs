using System;
using System.Collections.Generic;

namespace BusinessLogic.Abstraction.VMA.Models;

public partial class VenderPaymentNoteModel
{
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
