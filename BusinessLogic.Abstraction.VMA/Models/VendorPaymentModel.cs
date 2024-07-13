using System;
using System.Collections.Generic;

namespace BusinessLogic.Abstraction.VMA.Models;

public partial class VendorPaymentModel
{
    public int VendorPaymentId { get; set; }

    public string? PaymentCode { get; set; }

    public string PaymentYear { get; set; } = null!;

    public DateTime VendorPaymentDate { get; set; }

    public string VendorPaymentAmount { get; set; } = null!;

    public bool? VendorPaymentIsGst { get; set; }

    public decimal? VendorPaymentCgst { get; set; }

    public decimal? VendorPaymentSgst { get; set; }

    public int? VendorPaymentTotalAmountPaid { get; set; }

    public string? VendorPaymentUtrnumber { get; set; }

    public decimal? VendorPaymentRtgsAmount { get; set; }

    public DateOnly? VendorPaymentRtgsDate { get; set; }

    public bool? VendorPaymentIsTdsapplicable { get; set; }

    public decimal? VendorPaymentTdsamount { get; set; }

    public string? Notes { get; set; }

    public bool? IsPaymentForBranch { get; set; }

    public string? BankBranchName { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public bool IsActive { get; set; }

    public string? LastUpdateBy { get; set; }

    public DateTime? LastUpdatedDate { get; set; }

    public int FkVendorDetailId { get; set; }

    public int FkInvoiceId { get; set; }

    public int FkNoteId { get; set; }
}
