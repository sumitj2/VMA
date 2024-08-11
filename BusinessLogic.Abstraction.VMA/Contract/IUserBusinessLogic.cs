using BusinessLogic.Abstraction.VMA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Abstraction.VMA.Contract
{
    public interface IUserBusinessLogic
    {
        Task<UserModel> GetByUsername(string username);
        Task<bool> AuthenticateUser(NetworkCredential credential);
        Task<int> AddUser(UserModel userModel);
        Task<List<UserModel>> GetAllUSers();
    }
}
