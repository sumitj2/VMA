using System;
using System.Collections.Generic;

namespace BusinessLogic.Abstraction.VMA.Models;

public partial class VendorDetailModel
{
    #region  Join
    public int VendorServiceId { get; set; }
    public string? VendorServiceName { get; set; }
    public int VendorId { get; set; }
    public string VendorCode { get; set; } = null!;
    public string VendorName { get; set; } = null!;

    #endregion

    public int VendorDetailId { get; set; }

    public string? VendorDetailCategory { get; set; }

    public string DetailsYear { get; set; } = null!;

    public string? RatePerUnit { get; set; }

    public int? QuantityOfUnit { get; set; }

    public string? ServiceSantionAmount { get; set; }

    public DateOnly? SantionedDate { get; set; }

    public string? SantionedNoteNo { get; set; }

    public DateOnly? ServiceStartDate { get; set; }

    public DateOnly? ServiceEndDate { get; set; }

    public string? ServiceSantionedBy { get; set; }

    public string? ServiceType { get; set; }

    public string? ServicePaymentType { get; set; }

    public string? SantionedType { get; set; }

    public bool? IsAmc { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public bool? IsActive { get; set; }

    public string? LastUpdateBy { get; set; }

    public DateTime? LastUpdatedDate { get; set; }

    public int FkVendorServiceId { get; set; }

    public int FkVendorId { get; set; }
}
