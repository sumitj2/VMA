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
    public class VendorDetailsRepository : IVendorDetailsRepository
    {
        private readonly VendorManagementDbContext _context;
        public VendorDetailsRepository(VendorManagementDbContext context)
        {
            _context = context;
        }
        public async Task AddVendorDetails(VendorDetail VendorDetailEntity)
        {
            await _context.AddAsync(VendorDetailEntity).ConfigureAwait(true);
            await _context.SaveChangesAsync();
        }
        public async Task EditUpdateVendorDetail(VendorDetail VendorDetailEntity)
        {
            var result = await GetVendorDetailsId(VendorDetailEntity.VendorDetailId);
            if (result != null)
            {
                _context.VendorDetails.Update(result);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<VendorDetail>> GetAllVendorDetails()
        {
            return await _context.VendorDetails.Where(x => x.IsActive == true).ToListAsync();
        }

        public async Task<List<VendorDetailsWithService>> GetVendorDetailsWithService()
        {
            var productsWithVendors = from service in _context.VendorServices
                                      join vendor in _context.VendorDetails
                                      on service.VendorServiceId equals vendor.FkVendorServiceId
                                      where service.IsActive == true
                                      select new VendorDetailsWithService
                                      {                                          
                                          CreatedBy = vendor.CreatedBy,
                                          CreatedDate = vendor.CreatedDate,
                                          IsActive = vendor.IsActive,
                                          LastUpdateBy = vendor.LastUpdateBy,
                                          LastUpdatedDate = vendor.LastUpdatedDate,
                                          VendorServiceId = service.VendorServiceId,
                                          VendorServiceName = service.VendorServiceName,
                                          FkVendorServiceId=vendor.FkVendorServiceId,
                                          QuantityOfUnit=vendor.QuantityOfUnit,
                                          RatePerUnit=vendor.RatePerUnit,
                                          ServiceEndDate = vendor.ServiceEndDate,
                                          ServicePaymentType=vendor.ServicePaymentType,
                                          ServiceSantionAmount=vendor.ServiceSantionAmount,
                                          ServiceSantionedBy=vendor.ServiceSantionedBy,
                                          ServiceStartDate=vendor.ServiceStartDate, 
                                          ServiceType=vendor.ServiceType,
                                          VendorDetailCategory = vendor.VendorDetailCategory,
                                          VendorDetailId=vendor.VendorDetailId,
                                         
                                      };
            return await productsWithVendors.ToListAsync();
        }
        public async Task<VendorDetail?> GetVendorDetailsId(int vendorDetailId)
        {
            return await _context.VendorDetails.Where(x => x.IsActive == true && x.VendorDetailId == vendorDetailId).FirstOrDefaultAsync();
        }

        public async Task RemoveVendorDetails(VendorDetail VendorDetailEntity)
        {
            _context.VendorDetails.Remove(VendorDetailEntity);
            await _context.SaveChangesAsync();
        }
    }
}
