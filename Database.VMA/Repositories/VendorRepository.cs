using Database.Abstraction.VMA.Contract;
using Database.VMA.Entities;
using Database.VMA.Models;
using Microsoft.EntityFrameworkCore;
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
        public void Add(Vendor userModel)
        {
            _context.AddAsync(userModel).ConfigureAwait(true);
        }
        public void Edit(Vendor userModel)
        {
            _context.Vendors.Update(userModel);
        }
        public async Task<IEnumerable<Vendor>> GetByAll()
        {
            return await _context.Vendors.ToListAsync();
        }
        public Vendor GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}
