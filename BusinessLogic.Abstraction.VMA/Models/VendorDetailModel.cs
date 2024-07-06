using System;
using System.Collections.Generic;

namespace BusinessLogic.Abstraction.VMA.Models;

public partial class VendorDetailModel
{
    public int VendorServiceId { get; set; }
    public string? VendorServiceName { get; set; }
    public int VendorDetailId { get; set; }

    public string? VendorDetailCategory { get; set; }

    public string? RatePerUnit { get; set; }

    public int? QuantityOfUnit { get; set; }

    public string? ServiceSantionAmount { get; set; }

    public DateOnly? ServiceStartDate { get; set; }

    public DateOnly? ServiceEndDate { get; set; }

    public string? ServiceSantionedBy { get; set; }

    public string? ServiceType { get; set; }

    public string? ServicePaymentType { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public bool? IsActive { get; set; }

    public string? LastUpdateBy { get; set; }

    public DateTime? LastUpdatedDate { get; set; }

    public int? FkVendorServiceId { get; set; }
}
