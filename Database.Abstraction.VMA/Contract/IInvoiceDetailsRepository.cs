using Database.VMA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Abstraction.VMA.Contract
{
    public interface IInvoiceDetailsRepository
    {
        public Task<int> AddInvoice(InvoiceDetails InvoiceDetailsEntity);
        public Task EditUpdateInvoice(InvoiceDetails InvoiceDetailsEntity);
        public Task<IEnumerable<InvoiceDetails>> GetAllInvoices();
        public Task<InvoiceDetails?> GetInvoiceById(int invoiceId);
        public Task RemoveInvoice(InvoiceDetails invoiceDetails);
    }
}
