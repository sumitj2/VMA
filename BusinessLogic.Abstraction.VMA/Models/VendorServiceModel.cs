using System;
using System.Collections.Generic;

namespace BusinessLogic.Abstraction.VMA.Models;

public partial class VendorServiceModel
{
    public int VendorId { get; set; }

    public string? VendorCode { get; set; }

    public string? VendorName { get; set; }
  
    public int VendorServiceId { get; set; }

    public string? VendorServiceName { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public bool? IsActive { get; set; }

    public string? LastUpdateBy { get; set; }

    public DateTime? LastUpdatedDate { get; set; }

    public int FkVendorId { get; set; }
}
