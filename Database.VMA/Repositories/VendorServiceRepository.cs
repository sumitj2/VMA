using Database.Abstraction.VMA.Contract;
using Database.VMA.Entities;
using Database.VMA.Entities.CustomEntities;
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

        public async Task<List<VendorsWithServices>> GetVendorWithService() 
        {
            var productsWithVendors = from service in _context.VendorServices
                                      join vendor in _context.Vendors
                                      on service.FkVendorId equals vendor.VendorId 
                                      where service.IsActive==true                                     
                                      select new VendorsWithServices
                                      {
                                         FkVendorId = vendor.VendorId,
                                          CreatedBy = service.CreatedBy,
                                          CreatedDate = service.CreatedDate,
                                          IsActive = service.IsActive,
                                          LastUpdateBy = service.LastUpdateBy,  
                                          LastUpdatedDate = service.LastUpdatedDate,
                                          VendorServiceId  =service.VendorServiceId,
                                          VendorServiceName = service.VendorServiceName,
                                          VendorCode=vendor.VendorCode,
                                          VendorId=vendor.VendorId,
                                          VendorName=vendor.VendorName, 
                                      };
            return await productsWithVendors.ToListAsync();
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
