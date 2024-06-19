using Database.Abstraction.VMA.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.VMA.Repositories
{
    public class VendorServiceRepository : IVendorServiceRepository
    {
        private readonly VendorManagementDbContext _context;

        public VendorServiceRepository(VendorManagementDbContext context)
        {
            _context = context;
        }
    }
}
