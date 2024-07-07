using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.VMA.Entities.CustomEntities
{
    public class VendorPaymentWithService : VendorPayment
    {
        public int VendorServiceId { get; set; }
        public string? VendorServiceName { get; set; }
        public string? ServiceSantionAmount { get; set; }
        public string? ServicePaymentType { get; set; }
    }
}
