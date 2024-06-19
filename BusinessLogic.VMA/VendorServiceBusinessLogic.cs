using BusinessLogic.Abstraction.VMA.Contract;
using Database.Abstraction.VMA.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.VMA.Repositories
{
    public class VendorServiceBusinessLogic : IVendorServiceBusinessLogic
    {
        private IVendorServiceRepository _vendorServiceRepository;
        public VendorServiceBusinessLogic(IVendorServiceRepository vendorServiceRepository)
        {
            _vendorServiceRepository=vendorServiceRepository;
        }
    }
}
