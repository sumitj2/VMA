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
using Microsoft.Data.SqlClient;
using System.Windows;
using System.Configuration;
using System.Drawing;
using System.Text.RegularExpressions;
using VMA.Constants;

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
        private string _dbStatus;

        public string DbStatus
        {
            get { return _dbStatus; }
            set
            {
                _dbStatus = value;
                OnPropertyChanged(nameof(DbStatus));
            }
        }

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
            Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Into the constructor", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));            
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
        private async Task CheckDatabaseConnectionAsync()
        {
            string cs = ConfigurationManager.ConnectionStrings["VMA"].ConnectionString;
            try
            {
                using var connection = new SqlConnection(cs);
                await connection.OpenAsync();
                DbStatus = GeneralConstants.Success;
            }
            catch (Exception ex)
            {
                DbStatus = GeneralConstants.Error;
                var csWithotPass= RemovePasswordFromConnectionString(cs);                
                Log.Logger.Error(ex, string.Format("Class: {0}, Method: {1} - CheckDatabaseConnectionAsync", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));
                Log.Logger.Information(csWithotPass);
            }
        }
        public string RemovePasswordFromConnectionString(string connectionString)
        {
            // This regex looks for "Password=" or "Pwd=" followed by any characters until it hits either a semicolon or end of string
            string pattern = @"(?i)(Password|Pwd)=.*?(;|$)";

            // Replace the password section with an empty string
            string sanitizedConnectionString = Regex.Replace(connectionString, pattern, string.Empty);

            return sanitizedConnectionString;
        }
        private async void ExecuteLoginCommand(object obj)
        {
            try
            {
                _ = CheckDatabaseConnectionAsync();
                if (DbStatus == GeneralConstants.Success)
                {
                    Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Into the Login Command", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));

                    var isValidUser = await _userBusinessLogic.AuthenticateUser(new NetworkCredential(Username, Password));

                    if (isValidUser)
                    {
                        Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(Username), null);
                        IsViewVisible = false;

                        Log.Logger.Information(string.Format("Class: {0}, Method: {1} - User Authenticated", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));
                    }
                    else
                    {
                        ErrorMessage = MessagesContants.InvalidUser;
                    }
                }
                else
                {
                    ErrorMessage = MessagesContants.DbNotConnected;
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, string.Format("Class: {0}, Method: {1} - Failed to Authorize user", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));
            }
        }

        private async void ExecuteRecoverPassCommand(string username, string email)
        {
            throw new NotImplementedException();
        }
    }
}
