using System;
using System.Collections.Generic;

namespace Database.VMA.Entities;

public partial class VendorPaymentNote
{
    public int NoteId { get; set; }

    public string PaymentNoteNo { get; set; } = null!;

    public DateTime PaymentNoteDate { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public bool? IsActive { get; set; }

    public string? LastUpdateBy { get; set; }

    public DateTime? LastUpdatedDate { get; set; }

    public int FkVendorId { get; set; }
}
