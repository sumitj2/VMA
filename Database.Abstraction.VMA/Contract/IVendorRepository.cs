using Database.VMA.Entities;
using System.Data;

namespace Database.Abstraction.VMA.Contract
{
    public interface IVendorRepository
    {
        public Task AddVendors(Vendor VendorEntity);
        public Task EditUpdateVendors(Vendor VendorEntity);

        public Task<IEnumerable<Vendor>> GetAllVendors();
        public Task<Vendor?> GetVendorsById(int vendorId);

        public Task RemoveVendor(Vendor VendorEntity);
        public int SaveImportedVendorsToDatabase(DataTable dataTable);
    }

}
