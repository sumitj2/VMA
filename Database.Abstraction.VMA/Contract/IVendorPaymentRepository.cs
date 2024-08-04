using Database.VMA.Entities;
using Database.VMA.Entities.CustomEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Abstraction.VMA.Contract
{
    public interface IVendorPaymentRepository
    {
        public Task AddVendorPayment(VendorPayment VendorPaymentEntity);
        public Task EditUpdateVendorPayment(VendorPayment VendorPaymentEntity);
        public Task<IEnumerable<VendorPayment>> GetAllVendorPayment();
        public Task<VendorPayment?> GetVendorPaymentById(int vendorDetailId);
        public Task RemoveVendorPayment(VendorPayment VendorPaymentEntity);
        public Task<List<VendorPaymentWithService>> GetAllPaymentDetailsWithServiceDetails();
        public Task<List<VendorPaymentWithService>> GetPaymentDetailsWithServiceDetailsByVednorDetailId(int? vendorDetailId);
    }
}
