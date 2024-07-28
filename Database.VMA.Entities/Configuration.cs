using System;
using System.Collections.Generic;

namespace Database.VMA.Entities;

public partial class Configuration
{
    public int Id { get; set; }

    public string Cfgkey { get; set; } = null!;

    public string? Cfgvalue { get; set; }
}
