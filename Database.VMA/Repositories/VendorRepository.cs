using Database.Abstraction.VMA.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.VMA.Repositories
{
    public class VendorRepository : IVendorRepository
    {
        private readonly VendorManagementDbContext _context;

        public VendorRepository(VendorManagementDbContext context)
        {
            _context = context;
        }
    }
}
