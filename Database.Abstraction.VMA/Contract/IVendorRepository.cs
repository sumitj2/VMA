using Database.VMA.Entities;

namespace Database.Abstraction.VMA.Contract
{
    public interface IVendorRepository
    {
        public Task AddVendors(Vendor VendorEntity);
        public Task EditUpdateVendors(Vendor VendorEntity);

        public Task<IEnumerable<Vendor>> GetAllVendors();
        public Task<Vendor?> GetVendorsById(int vendorId);

        public Task RemoveVendor(Vendor VendorEntity);
    }

}
