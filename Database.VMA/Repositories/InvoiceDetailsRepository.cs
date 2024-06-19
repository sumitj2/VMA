using Database.Abstraction.VMA.Contract;
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
    }
}
