using System;
using System.Collections.Generic;

namespace BusinessLogic.Abstraction.VMA.Models;

public partial class VendorModel
{
    public int VendorId { get; set; }

    public string? VendorCode { get; set; }

    public string? VendorName { get; set; }
    public string? VendorPan { get; set; }

    public string? VendorAddress { get; set; }

    public string? VendorPinCode { get; set; }

    public string? VendorPhoneNo { get; set; }

    public string? VendorEmailId { get; set; }

    public string? VendorBankName { get; set; }

    public string? VendorAccountNumber { get; set; }

    public string? VendorIfsccode { get; set; }

    public string? VendorGstnumber { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public bool? IsActive { get; set; }

    public string? LastUpdateBy { get; set; }

    public DateTime? LastUpdatedDate { get; set; }
}
