using Database.Abstraction.VMA.Contract;
using Database.VMA.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.VMA.Repositories
{
    public class VendorServiceRepository : IVendorServiceRepository
    {
        private readonly VendorManagementDbContext _context;

        public VendorServiceRepository(VendorManagementDbContext context)
        {
            _context = context;
        }
        public async Task AddVendorService(VendorService vendorService)
        {
            await _context.AddAsync(vendorService).ConfigureAwait(true);
            await _context.SaveChangesAsync();
        }
        public async Task EditUpdateVendorService(VendorService vendorService)
        {
            var result = await GetVendorServiceById(vendorService.VendorServiceId);
            if (result != null)
            {
                _context.VendorServices.Update(result);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<VendorService>> GetAllVendorServices()
        {
            return await _context.VendorServices.Where(x => x.IsActive == true).ToListAsync();
        }
        public async Task<VendorService?> GetVendorServiceById(int vendorId)
        {
            return await _context.VendorServices.Where(x => x.IsActive == true && x.VendorServiceId == vendorId).FirstOrDefaultAsync();
        }

        public async Task RemoveVendorService(VendorService vendorService)
        {
            _context.VendorServices.Remove(vendorService);
            await _context.SaveChangesAsync();
        }
    }
}
