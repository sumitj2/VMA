using Database.VMA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Abstraction.VMA.Contract
{
    public interface IVendorDetailsRepository
    {
        public Task AddVendorDetails(VendorDetail VendorDetailEntity);
        public Task EditUpdateVendorDetail(VendorDetail VendorDetailEntity);
        public Task<IEnumerable<VendorDetail>> GetAllVendorDetails();
        public Task<VendorDetail?> GetVendorDetailsId(int vendorDetailId);
        public Task RemoveVendorDetails(VendorDetail VendorDetailEntity);
    }
}
