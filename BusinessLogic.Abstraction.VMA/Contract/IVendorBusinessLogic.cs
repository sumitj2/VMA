using BusinessLogic.Abstraction.VMA.Models;

namespace BusinessLogic.Abstraction.VMA.Contract
{
    public interface IVendorBusinessLogic
    {
        public Task AddVendor(VendorModel vendorModel);
        public Task EditUpdateVendor(VendorModel vendorModel);
        public Task<IEnumerable<VendorModel>> GetAllVendor();
        public Task<VendorModel?> GetVendorById(int vendorId);
        public Task RemoveVendorService(VendorModel serviceModel);
    }

}
