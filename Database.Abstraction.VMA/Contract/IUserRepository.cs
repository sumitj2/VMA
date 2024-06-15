using Database.VMA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Database.Abstraction.VMA.Contract
{
    public interface IUserRepository
    {
        Task<bool> AuthenticateUser(NetworkCredential credential);
        Task<User> GetByUsername(string username);
    }
}
