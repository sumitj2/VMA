using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Abstraction.VMA.Models
{
    public partial class ConfigurationModel
    {
        public int Id { get; set; }

        public string Cfgkey { get; set; }

        public string CfgValue { get; set; }
    }
}
