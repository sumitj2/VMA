
using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.Abstraction.VMA.Models;
using Database.Abstraction.VMA.Contract;
using System.Net;

namespace BusinessLogic.VMA
{
    public class UserBusinessLogic : IUserBusinessLogic
    {
        private readonly IUserRepository _userRepository;
        public UserBusinessLogic(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> AuthenticateUser(NetworkCredential credential)
        {
            return await _userRepository.AuthenticateUser(credential).ConfigureAwait(false);
        }

        public async Task<UserModel> GetByUsername(string username)
        {
            UserModel userModel;
            var res = await _userRepository.GetByUsername(username).ConfigureAwait(false);

            if (res != null)
            {
                userModel = new UserModel()
                {
                    Email = res.Email,
                    LastName = res.LastName,
                    Name = res.Name,
                    Username = res.Username
                };
            }
            return new UserModel();

        }
    }

}
