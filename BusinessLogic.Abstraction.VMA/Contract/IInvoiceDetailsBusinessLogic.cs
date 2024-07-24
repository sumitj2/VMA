using BusinessLogic.Abstraction.VMA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Abstraction.VMA.Contract
{
    public interface IInvoiceDetailsBusinessLogic
    {
        public Task<int?> AddInvoice(InvoiceDetailsModel InvoiceDetailModel);
        public Task EditUpdateInvoice(InvoiceDetailsModel InvoiceDetailModel);
        public Task<IEnumerable<InvoiceDetailsModel>> GetAllInvoices();
        public Task<InvoiceDetailsModel?> GetInvoiceById(int? invoiceId);
        public Task RemoveInvoice(InvoiceDetailsModel InvoiceDetailModel);
    }
}
