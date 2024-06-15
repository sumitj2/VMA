using Database.Abstraction.VMA.Contract;
using Database.VMA.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Database.VMA.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly VendorManagementDbContext _context;
        public UserRepository(VendorManagementDbContext context)
        {
            _context = context;
        }
        
        public async Task<bool> AuthenticateUser(NetworkCredential credential)
        {
            var result = await _context.Users.FirstOrDefaultAsync(x => x.Username == credential.UserName && x.Password == credential.Password).ConfigureAwait(false);
            if (result  != null)
            {
                return true;
            };
            return false;
        }

        public async Task<User> GetByUsername(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username).ConfigureAwait(false);
            return user!;
        }

        public void Add(User userModel)
        {
            throw new NotImplementedException();
        }
        public void Edit(User userModel)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<User> GetByAll()
        {
            throw new NotImplementedException();
        }
        public User GetById(int id)
        {
            throw new NotImplementedException();
        }
        
        public void Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}
