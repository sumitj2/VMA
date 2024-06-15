using BusinessLogic.Abstraction.VMA.Contract;
using Database.VMA.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMA.MVVM.Models;

namespace VMA.MVVM.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        //Fields
        private UserAccountModel _currentUserAccount;
        private IUserBusinessLogic _userBusinessLogic;

        public UserAccountModel CurrentUserAccount
        {
            get
            {
                return _currentUserAccount;
            }

            set
            {
                _currentUserAccount = value;
                OnPropertyChanged(nameof(CurrentUserAccount));
            }
        }

        public MainViewModel(IUserBusinessLogic userBusinessLogic)
        {
            _userBusinessLogic = userBusinessLogic;
            CurrentUserAccount = new UserAccountModel();
            LoadCurrentUserData();
        }

        private async void LoadCurrentUserData()
        {
            var user =await _userBusinessLogic.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            if (user != null)
            {
                CurrentUserAccount.Username = user.Username;
                CurrentUserAccount.DisplayName = $"Welcome {user.Name} {user.LastName} ;)";
                CurrentUserAccount.ProfilePicture = null;
            }
            else
            {
                CurrentUserAccount.DisplayName = "Invalid user, not logged in";
                //Hide child views.
            }
        }
    }
}
