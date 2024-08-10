using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.Abstraction.VMA.Models;
using BusinessLogic.VMA;
using Database.VMA.Entities;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace VMA.MVVM.ViewModels
{
    public class SuperAdminViewModel : ViewModelBase
    {
        private ObservableCollection<UserModel?> _userModel;
        public ObservableCollection<UserModel?> UserModels
        {
            get { return _userModel; }
            set
            {
                _userModel = value;
                OnPropertyChanged(nameof(UserModels));
            }
        }
        private string _UserName;
        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }

        private string _Password;
        public string Password
        {
            get { return _Password; }
            set { _Password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        private string _FirstName;
        public string FirstName
        {
            get { return _FirstName; }
            set { _FirstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        private string _LastName;
        public string LastName
        {
            get { return _LastName; }
            set { _LastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        private string _EmailId;
        public string EmailId
        {
            get { return _EmailId; }
            set
            {
                _EmailId = value;
                OnPropertyChanged(nameof(EmailId));
            }
        }

        public ICommand SubmitCommand { get; }

        private readonly IUserBusinessLogic _userBusinessLogic;
        public SuperAdminViewModel(IUserBusinessLogic userBusinessLogic)
        {
            SubmitCommand = new ViewModelCommand(SaveUser,ValidateUser);
            _userBusinessLogic = userBusinessLogic;
            _ = GetUsers();
        }

        private bool ValidateUser(object obj)
        {
            return true;
        }

        private void SaveUser(object obj)
        {
            UserModel model = new UserModel()
            {
                Password = Password,
                LastName = LastName,
                Email = EmailId,
                Name = FirstName,
                Username = UserName
            };
            _userBusinessLogic.AddUser(model);
            MessageBox.Show("User Add Successfully");
            _ = GetUsers();
            ClearForm();
        }

        private void ClearForm()
        {
            UserName = "";
            Password = "";
            FirstName = "";
            LastName = "";
            EmailId = "";
        }

        private async Task GetUsers()
        {
            try
            {
                Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Getting Users", this.GetType().Name, MethodBase.GetCurrentMethod().Name));

                var usersList = await _userBusinessLogic.GetAllUSers().ConfigureAwait(true);
                UserModels = new ObservableCollection<UserModel?>(usersList);

                Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Retrieved Users", this.GetType().Name, MethodBase.GetCurrentMethod().Name));
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, string.Format("Class: {0}, Method: {1} - Failed to get Users", this.GetType().Name, MethodBase.GetCurrentMethod().Name));
            }
        }
    }
}
