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
using Serilog;
using System.Reflection;

namespace VMA.MVVM.ViewModels.Login
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IUserBusinessLogic _userBusinessLogic;

        //Fields
        private string _username;
        private SecureString _password;
        private string _errorMessage;
        private bool _isViewVisible = true;

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
            Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Into the constructor", this.GetType().Name, MethodBase.GetCurrentMethod().Name));

            _userBusinessLogic = userBusinessLogic;
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
            try
            {
                Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Into the Login Command", this.GetType().Name, MethodBase.GetCurrentMethod().Name));

                var isValidUser = await _userBusinessLogic.AuthenticateUser(new NetworkCredential(Username, Password));

                if (isValidUser)
                {
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(Username), null);
                    IsViewVisible = false;

                    Log.Logger.Information(string.Format("Class: {0}, Method: {1} - User Authenticated", this.GetType().Name, MethodBase.GetCurrentMethod().Name));
                }
                else
                {
                    ErrorMessage = "* Invalid username or password";
                }

            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, string.Format("Class: {0}, Method: {1} - Failed to Authorize user", this.GetType().Name, MethodBase.GetCurrentMethod().Name));
            }
        }

        private async void ExecuteRecoverPassCommand(string username, string email)
        {
            throw new NotImplementedException();
        }
    }
}
