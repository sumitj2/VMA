using Database.VMA.Entities;
using System.Net;

namespace Database.Abstraction.VMA.Contract
{
    public interface IUserRepository
    {
        Task<bool> AuthenticateUser(NetworkCredential credential);
        Task<User> GetByUsername(string username);
        Task<List<User>> GetAllActiveUser();
        Task Add(User userModel);
    }
}
