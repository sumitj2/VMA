using Database.Abstraction.VMA.Contract;
using Database.VMA.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database.VMA.Repositories
{
    public class VendorRepository : IVendorRepository
    {
        private readonly VendorManagementDbContext _context;

        public VendorRepository(VendorManagementDbContext context)
        {
            _context = context;
        }
        public async Task AddVendors(Vendor VendorEntity)
        {
            await _context.AddAsync(VendorEntity).ConfigureAwait(true);
            await _context.SaveChangesAsync();
        }
        public async Task EditUpdateVendors(Vendor VendorEntity)
        {
            var result = await GetVendorsById(VendorEntity.VendorId);
            if (result != null)
            {
                _context.Vendors.Update(result);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Vendor>> GetAllVendors()
        {
            return await _context.Vendors.Where(x => x.IsActive == true).ToListAsync();
        }
        public async Task<Vendor?> GetVendorsById(int vendorId)
        {
            return await _context.Vendors.Where(x => x.IsActive == true && x.VendorId == vendorId).FirstOrDefaultAsync();
        }

        public async Task RemoveVendor(Vendor VendorEntity)
        {
            _context.Vendors.Remove(VendorEntity);
            await _context.SaveChangesAsync();
        }
    }
}
