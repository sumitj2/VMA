using BusinessLogic.Abstraction.VMA.Contract;
using Database.Abstraction.VMA.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.VMA.Repositories
{
    public class VendorBusinessLogic : IVendorBusinessLogic
    {

        private readonly IVendorRepository _vendorRepository;
        public VendorBusinessLogic(IVendorRepository vendorRepository)
        {
            _vendorRepository = vendorRepository;

        }
    }
}
