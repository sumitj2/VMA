using BusinessLogic.Abstraction.VMA.Contract;
using Database.Abstraction.VMA.Contract;
using Database.VMA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.VMA.Repositories
{
    public class VendorPaymentBusinessLogic: IVendorPaymentBusinessLogic
    {
        private IVendorPaymentRepository _vendorPaymentRepository;
        public VendorPaymentBusinessLogic(IVendorPaymentRepository vendorPaymentRepository)
        {
                _vendorPaymentRepository = vendorPaymentRepository;
        }
    }
}
