using Database.Abstraction.VMA.Contract;
using Database.VMA.Entities;
using Microsoft.EntityFrameworkCore;
using System.Net;

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
            try
            {
                var result = await _context.Users.FirstOrDefaultAsync(x => x.Username == credential.UserName && x.Password == credential.Password).ConfigureAwait(false);
                if (result != null)
                {
                    return true;
                };
                return false;

            }
            catch (Exception ex)
            {
                return false;

            }
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
