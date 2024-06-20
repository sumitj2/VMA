using System;
using System.Collections.Generic;

namespace Database.VMA.Entities;

public partial class GstcalculationMaster
{
    public int SrNo { get; set; }

    public int? CgstPercentage { get; set; }

    public int? SgstPercentage { get; set; }

    public int? IgstPercentage { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public bool? IsActive { get; set; }

    public string? LastUpdateBy { get; set; }

    public DateTime? LastUpdatedDate { get; set; }
}
