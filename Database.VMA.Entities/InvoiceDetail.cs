using System;
using System.Collections.Generic;

namespace Database.VMA.Entities;

public partial class InvoiceDetail
{
    public int? InvoiceId { get; set; }

    public string? InvoiceNumber { get; set; }

    public DateTime? InvoiceDate { get; set; }

    public string? InvoiceParticulars { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public bool? IsActive { get; set; }

    public string? LastUpdateBy { get; set; }

    public DateTime? LastUpdatedDate { get; set; }
}
