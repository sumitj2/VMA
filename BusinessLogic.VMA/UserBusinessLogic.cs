
using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.Abstraction.VMA.Models;
using Database.Abstraction.VMA.Contract;
using Database.VMA.Entities;
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
            UserModel userModel=new();
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
            return userModel;

        }
        public async Task<int> AddUser(UserModel userModel)
        {
            User userEntity=new User()
            {
                Email=userModel.Email,
                IsActive=true,
                LastName=userModel.LastName,    
                Name= userModel.Name,
                Password=userModel.Password,
                Username=userModel.Username
            };
            return await _userRepository.Add(userEntity);
        }

        public async Task<List<UserModel>> GetAllUSers()
        {
            List<UserModel> userModels = new();
            var users = await _userRepository.GetAllActiveUser().ConfigureAwait(false);

            if (users != null && users.Count > 0) 
            {
                foreach (var user in users) 
                {
                    userModels.Add(new UserModel()
                    {
                        Email = user.Email,
                        LastName= user.LastName,
                        Name = user.Name,
                        Username= user.Username,
                        Id=user.Id,
                        Password=user.Password
                    });
                }
            }
            return userModels;

        }
    }

}
