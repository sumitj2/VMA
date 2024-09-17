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
            _context.VendorDetails.Update(VendorDetailEntity);
            await _context.SaveChangesAsync();

        }
        public async Task<IEnumerable<VendorDetail>> GetAllVendorDetails()
        {
            return await _context.VendorDetails.Where(x => x.IsActive == true).ToListAsync();
        }

        public async Task<List<VendorDetailsWithService>> GetVendorDetailsWithService()
        {
            var productsWithVendors = from vendorDetail in _context.VendorDetails
                                      join service in _context.VendorServices
                                      on vendorDetail.FkVendorServiceId equals service.VendorServiceId
                                      join vendor in _context.Vendors
                                      on vendorDetail.FkVendorId equals vendor.VendorId
                                      where service.IsActive == true
                                      select new VendorDetailsWithService
                                      {
                                          CreatedBy = vendorDetail.CreatedBy,
                                          CreatedDate = vendorDetail.CreatedDate,
                                          IsActive = vendorDetail.IsActive,
                                          LastUpdateBy = vendorDetail.LastUpdateBy,
                                          LastUpdatedDate = vendorDetail.LastUpdatedDate,
                                          VendorServiceId = service.VendorServiceId,
                                          VendorServiceName = service.VendorServiceName,
                                          FkVendorServiceId = vendorDetail.FkVendorServiceId,
                                          QuantityOfUnit = vendorDetail.QuantityOfUnit,
                                          RatePerUnit = vendorDetail.RatePerUnit,
                                          ServiceEndDate = vendorDetail.ServiceEndDate,
                                          ServicePaymentType = vendorDetail.ServicePaymentType,
                                          ServiceSantionAmount = vendorDetail.ServiceSantionAmount,
                                          ServiceSantionedBy = vendorDetail.ServiceSantionedBy,
                                          ServiceStartDate = vendorDetail.ServiceStartDate,
                                          ServiceType = vendorDetail.ServiceType,
                                          VendorDetailCategory = vendorDetail.VendorDetailCategory,
                                          VendorDetailId = vendorDetail.VendorDetailId,
                                          FkVendorId = vendorDetail.FkVendorId,
                                          DetailsYear = vendorDetail.DetailsYear,
                                          IsAmc = vendorDetail.IsAmc,
                                          SantionedDate = vendorDetail.SantionedDate,
                                          SantionedNoteNo = vendorDetail.SantionedNoteNo,
                                          SantionedType = vendorDetail.SantionedType,
                                          VendorCode = vendor.VendorCode,
                                          VendorId = vendor.VendorId,
                                          VendorName = vendor.VendorName,
                                          SlaexpireDate = vendorDetail.SlaexpireDate,
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
