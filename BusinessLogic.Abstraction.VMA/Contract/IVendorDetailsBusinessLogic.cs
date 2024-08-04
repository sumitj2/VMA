using BusinessLogic.Abstraction.VMA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Abstraction.VMA.Contract
{
    public interface IVendorDetailsBusinessLogic
    {
        public Task AddVendorDetails(VendorDetailModel vendorDetailModel);
        public Task EditUpdateVendorDetails(VendorDetailModel vendorDetailModel);
        public Task<IEnumerable<VendorDetailModel>> GetAllVendorDetails();
        public Task<VendorDetailModel?> GetVendorDetailsById(int vendorDetailId);
        Task RemoveVendorDetails(VendorDetailModel VendorPaymentModel);

    }
}
