using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.VMA.Entities.CustomEntities
{
    public class VendorsWithServices :VendorService
    {
        public int VendorId { get; set; }

        public string? VendorCode { get; set; }

        public string? VendorName { get; set; }

    }
}
