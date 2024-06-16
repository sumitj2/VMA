using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VMA.MVVM.Models;
using BusinessLogic.Abstraction.VMA.Contract;
using Microsoft.VisualBasic;

namespace VMA.MVVM.ViewModels.Login
{
    public class LoginViewModel: ViewModelBase
    {
        private readonly IUserBusinessLogic _userBusinessLogic;

        //Fields
        private string _username;
        private SecureString _password;
        private string _errorMessage;
        private bool _isViewVisible = true;

        private IUserRepository userRepository;

        //Properties
        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public SecureString Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }

            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public bool IsViewVisible
        {
            get
            {
                return _isViewVisible;
            }
            set
            {
                _isViewVisible = value;
                OnPropertyChanged(nameof(IsViewVisible));
            }
        }

        //-> Commands
        private ViewModelCommand loginCommand;
        public ICommand LoginCommand
        {
            get
            {
                if (this.loginCommand == null)
                {
                    this.loginCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteLoginCommand);

                }
                return this.loginCommand;
            }
        }

        private ViewModelCommand recoverPasswordCommand;
        public ICommand RecoverPasswordCommand
        {
            get
            {
                if (this.recoverPasswordCommand == null)
                {
                    this.recoverPasswordCommand = new ViewModelCommand(p => ExecuteRecoverPassCommand("", ""));

                }
                return this.recoverPasswordCommand;
            }
        }
        public ICommand ShowPasswordCommand { get; }
        public ICommand RememberPasswordCommand { get; }

        //Constructor
        public LoginViewModel(IUserBusinessLogic userBusinessLogic)
        {
            _userBusinessLogic = userBusinessLogic;// Due to conructor parameter getting error in LoginView.xaml 
        }

        private bool CanExecuteLoginCommand(object obj)
        {
            bool validData;

            if (string.IsNullOrWhiteSpace(Username) || Username.Length < 3 ||
                Password == null || Password.Length < 3)
            {
                validData = false;
            }
            else
            {
                validData = true;
            }

            return validData;
        }

        private async void ExecuteLoginCommand(object obj)
        {
            var isValidUser = await _userBusinessLogic.AuthenticateUser(new NetworkCredential(Username, Password));
            
            if (isValidUser)
            {
                Thread.CurrentPrincipal = new GenericPrincipal(
                    new GenericIdentity(Username), null);
                IsViewVisible = false;
            }
            else
            {
                ErrorMessage = "* Invalid username or password";
            }
        }

        private async void ExecuteRecoverPassCommand(string username, string email)
        {
            throw new NotImplementedException();
        }
    }
}
