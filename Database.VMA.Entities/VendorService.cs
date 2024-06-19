using System;
using System.Collections.Generic;

namespace Database.VMA.Models;

public partial class VendorService
{
    public int VendorServiceId { get; set; }

    public string? VendorServiceName { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public bool? IsActive { get; set; }

    public string? LastUpdateBy { get; set; }

    public DateTime? LastUpdatedDate { get; set; }

    public int? FkVendorId { get; set; }
}
