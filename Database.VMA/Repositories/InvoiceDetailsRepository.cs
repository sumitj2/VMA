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
    public class InvoiceDetailsRepository: IInvoiceDetailsRepository
    {
        private readonly VendorManagementDbContext _context;
        public InvoiceDetailsRepository(VendorManagementDbContext context)
        {
                _context = context; 
        }
        public async Task AddInvoice(InvoiceDetails InvoiceDetailsEntity)
        {
            await _context.AddAsync(InvoiceDetailsEntity).ConfigureAwait(true);
            await _context.SaveChangesAsync();
        }
        public async Task EditUpdateInvoice(InvoiceDetails InvoiceDetailsEntity)
        {
            var result = await GetInvoiceById(InvoiceDetailsEntity.InvoiceId);
            if (result != null)
            {
                _context.InvoiceDetais.Update(result);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<InvoiceDetails>> GetAllInvoices()
        {
            return await _context.InvoiceDetais.Where(x => x.IsActive == true).ToListAsync();
        }
        public async Task<InvoiceDetails?> GetInvoiceById(int invoiceId)
        {
            return await _context.InvoiceDetais.Where(x => x.IsActive == true && x.InvoiceId == invoiceId).FirstOrDefaultAsync();
        }

        public async Task RemoveInvoice(InvoiceDetails invoiceDetails)
        {
            _context.InvoiceDetais.Remove(invoiceDetails);
            await _context.SaveChangesAsync();
        }
    }
}
