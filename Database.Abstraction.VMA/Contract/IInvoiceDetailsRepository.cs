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
        public Task<int?> AddInvoice(InvoiceDetail InvoiceDetailsEntity);
        public Task EditUpdateInvoice(InvoiceDetail InvoiceDetailsEntity);
        public Task<IEnumerable<InvoiceDetail>> GetAllInvoices();
        public Task<InvoiceDetail?> GetInvoiceById(int? invoiceId);
        public Task RemoveInvoice(InvoiceDetail invoiceDetails);
    }
}
