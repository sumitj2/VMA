using System;
using System.Collections.Generic;

namespace BusinessLogic.Abstraction.VMA.Models;

public partial class VenderPaymentNoteModel
{
    #region Join
    public int VendorServiceId { get; set; }
    public string? VendorServiceName { get; set; }
    public int VendorId { get; set; }
    public string? VendorName { get; set; }

    #endregion
    public int? NoteId { get; set; }

    public string PaymentNoteNo { get; set; } = null!;

    public string PaymentNoteDate { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public bool? IsActive { get; set; }

    public string? LastUpdateBy { get; set; }

    public DateTime? LastUpdatedDate { get; set; }

    public int? FkVendorId { get; set; }
    public string? PaymentNoteYear { get; set; }
}
