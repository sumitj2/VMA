using BusinessLogic.Abstraction.VMA.Contract;
using Database.Abstraction.VMA.Contract;

namespace Database.VMA.Repositories
{
    public class VendorDetailsBusinessLogic : IVendorDetailsBusinessLogic
    {

        private IVendorDetailsRepository _vendorDetailsRepository;
        public VendorDetailsBusinessLogic(IVendorDetailsRepository vendorDetailsRepository)
        {
            _vendorDetailsRepository=vendorDetailsRepository;
        }
    }
}
