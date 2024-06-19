using BusinessLogic.Abstraction.VMA.Contract;
using Database.Abstraction.VMA.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.VMA.Repositories
{
    public class InvoiceDetailsBusinessLogic : IInvoiceDetailsBusinessLogic
    {
        private readonly IInvoiceDetailsRepository _invoiceDetailsRepository;
        public InvoiceDetailsBusinessLogic(IInvoiceDetailsRepository invoiceDetailsRepository)
        {
            _invoiceDetailsRepository = invoiceDetailsRepository;
        }
    }
}
