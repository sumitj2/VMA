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
    public class VendorPaymentRepository: IVendorPaymentRepository
    {
        private readonly VendorManagementDbContext _context;
        public VendorPaymentRepository(VendorManagementDbContext context)
        {
            _context = context;
        }

        public async Task AddVendorPayment(VendorPayment VendorPaymentEntity)
        {
            await _context.AddAsync(VendorPaymentEntity).ConfigureAwait(true);
            await _context.SaveChangesAsync();
        }
        public async Task EditUpdateVendorPayment(VendorPayment VendorPaymentEntity)
        {
            var result = await GetVendorPaymentById(VendorPaymentEntity.VendorPaymentId);
            if (result != null)
            {
                _context.VendorPayments.Update(result);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<VendorPayment>> GetAllVendorPayment()
        {
            return await _context.VendorPayments.Where(x => x.IsActive == true).ToListAsync();
        }
        public async Task<VendorPayment?> GetVendorPaymentById(int vendorDetailId)
        {
            return await _context.VendorPayments.Where(x => x.IsActive == true && x.VendorPaymentId == vendorDetailId).FirstOrDefaultAsync();
        }

        public async Task RemoveVendorPayment(VendorPayment VendorPaymentEntity)
        {
            _context.VendorPayments.Remove(VendorPaymentEntity);
            await _context.SaveChangesAsync();
        }
    }
}
