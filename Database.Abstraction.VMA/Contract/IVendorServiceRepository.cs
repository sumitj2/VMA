using Database.VMA.Entities;

namespace Database.Abstraction.VMA.Contract
{
    public interface IVendorServiceRepository
    {
        public Task AddVendorService(VendorService vendorService);
        public Task EditUpdateVendorService(VendorService vendorService);
        public Task<IEnumerable<VendorService>> GetAllVendorServices();
        public Task<VendorService?> GetVendorServiceById(int vendorId);
        public Task RemoveVendorService(VendorService vendorService);
    }

}
