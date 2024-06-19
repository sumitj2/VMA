using System;
using System.Collections.Generic;

namespace Database.VMA.Models;

public partial class VendorPayment
{
    public int VendorPaymentId { get; set; }

    public string? VendorPaymentYear { get; set; }

    public DateTime? VendorPaymentDate { get; set; }

    public DateTime? VendorPaymentAmount { get; set; }

    public bool? VendorPaymentIsGst { get; set; }

    public decimal? VendorPaymentCgst { get; set; }

    public decimal? VendorPaymentSgst { get; set; }

    public int? VendorPaymentTotalAmountPaid { get; set; }

    public int? VendorPaymentUtrnumber { get; set; }

    public decimal? VendorPaymentRtgsAmount { get; set; }

    public DateOnly? VendorPaymentRtgsDate { get; set; }

    public decimal? VendorPaymentTdsamount { get; set; }

    public string? VendorPaymentNotesDetails { get; set; }

    public string? BankBranchName { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public bool? IsActive { get; set; }

    public string? LastUpdateBy { get; set; }

    public DateTime? LastUpdatedDate { get; set; }

    public int? FkVendorDetailId { get; set; }
}
