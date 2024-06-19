using Database.Abstraction.VMA.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.VMA.Repositories
{
    public class VendorDetailsRepository : IVendorDetailsRepository
    {
        private readonly VendorManagementDbContext _context;
        public VendorDetailsRepository(VendorManagementDbContext context)
        {
                _context = context;
        }
    }
}
