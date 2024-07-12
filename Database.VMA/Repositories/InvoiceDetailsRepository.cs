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
    public class InvoiceDetailsRepository : IInvoiceDetailsRepository
    {
        private readonly VendorManagementDbContext _context;
        public InvoiceDetailsRepository(VendorManagementDbContext context)
        {
            _context = context;
        }
        public async Task<int> AddInvoice(InvoiceDetail InvoiceDetailsEntity)
        {
            await _context.AddAsync(InvoiceDetailsEntity).ConfigureAwait(true);
            await _context.SaveChangesAsync();
            return InvoiceDetailsEntity.InvoiceId;
        }
        public async Task EditUpdateInvoice(InvoiceDetail InvoiceDetailsEntity)
        {
            var existingEntity = _context.InvoiceDetails.Find(InvoiceDetailsEntity.InvoiceId);
            if (existingEntity == null)
            {
                _context.Attach(InvoiceDetailsEntity);
            }
            else
            {
                _context.Entry(existingEntity).CurrentValues.SetValues(InvoiceDetailsEntity);
            }
            await _context.SaveChangesAsync();

        }
        public async Task<IEnumerable<InvoiceDetail>> GetAllInvoices()
        {
            return await _context.InvoiceDetails.Where(x => x.IsActive == true).ToListAsync();
        }
        public async Task<InvoiceDetail?> GetInvoiceById(int invoiceId)
        {
            return await _context.InvoiceDetails.Where(x => x.IsActive == true && x.InvoiceId == invoiceId).FirstOrDefaultAsync();
        }

        public async Task RemoveInvoice(InvoiceDetail invoiceDetails)
        {
            _context.InvoiceDetails.Remove(invoiceDetails);
            await _context.SaveChangesAsync();
        }
    }
}
