using BusinessLogic.Abstraction.VMA.Contract;
using Database.Abstraction.VMA.Contract;

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
