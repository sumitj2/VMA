using BusinessLogic.Abstraction.VMA.Models;

namespace BusinessLogic.Abstraction.VMA.Contract
{
    public interface IVendorServiceBusinessLogic
    {
        public Task AddVendorService(VendorServiceModel vendorServiceModel);
        public Task EditUpdateVendorService(VendorServiceModel serviceModel);
        public Task<IEnumerable<VendorServiceModel>> GetAllVendorServices();
        public Task<VendorServiceModel?> GetVendorServiceById(int vendorId);
        public Task RemoveVendorService(VendorServiceModel serviceModel);

    }

}
