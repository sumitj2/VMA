using BusinessLogic.Abstraction.VMA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Abstraction.VMA.Contract
{
    public interface IVendorPaymentBusinessLogic
    {
        Task AddVendorPayment(VendorPaymentModel VendorPaymentModel);
        Task EditUpdateVendorPayment(VendorPaymentModel VendorPaymentEntity);
        Task<IEnumerable<VendorPaymentModel>> GetAllVendorPayment();
        Task<VendorPaymentModel?> GetVendorPaymentById(int vendorDetailId);
        Task RemoveVendorPayment(VendorPaymentModel VendorPaymentEntity);

        Task<string> GeneratePaymentCode(VendorDetailModel? vendorDetailModel);
    }
}
