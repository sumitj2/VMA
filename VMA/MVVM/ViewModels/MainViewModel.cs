using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.VMA;
using Database.VMA.Repositories;
using FontAwesome.Sharp;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.Xml;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using VMA.MVVM.Models;
using VMA.MVVM.ViewModels.Add;
using VMA.MVVM.ViewModels.Login;
using VMA.MVVM.ViewModels.Menus;
using VMA.MVVM.Views;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using Application = System.Windows.Application;
using MessageBox = System.Windows.Forms.MessageBox;

namespace VMA.MVVM.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        //Fields
        private IUserBusinessLogic _userBusinessLogic;
        private IVendorBusinessLogic _vendorBusinessLogic;
        private IVendorServiceBusinessLogic _vendorServiceBusinessLogic;
        private IVendorDetailsBusinessLogic _vendorDetailsBusinessLogic;
        private IVendorPaymentBusinessLogic _vendorPaymentBusinessLogic;
        private IVenderPaymentNotesBusinessLogic _venderPaymentNotesBusinessLogic;
        private IInvoiceDetailsBusinessLogic _invoiceDetailsBusinessLogic;
        private IGstcalculationMasterBusinessLogic _gstcalculationMasterBusinessLogic;
        private IReportExportToExcelPaymentNote _reportExportToExcelPaymentNote;
        private IConfigurationBusinessLogic _configurationBusinessLogic;
        private IPaymentNoteInWord _paymentNoteInWord;
        private IYearlyMonthlyReportPDF _yearlyMonthlyReportPDF;
        private IHomePageBusinessLogic _homePageBusinessLogic;
        //private UserAccountModel _currentUserAccount;

        //public UserAccountModel CurrentUserAccount
        //{
        //    get
        //    {
        //        return _currentUserAccount;
        //    }

        //    set
        //    {
        //        _currentUserAccount = value;
        //        OnPropertyChanged(nameof(CurrentUserAccount));
        //    }
        //}

        private ViewModelBase _currentChildView;

        public ViewModelBase CurrentChildView
        {
            get => _currentChildView;
            set
            {
                _currentChildView = value;
                OnPropertyChanged(nameof(CurrentChildView));
            }
        }

        private string _caption;

        public string Caption
        {
            get => _caption;

            set
            {
                _caption = value;
                OnPropertyChanged(nameof(Caption));
            }
        }

        private IconChar _icon;

        public IconChar Icon
        {
            get => _icon;
            set
            {
                _icon = value;
                OnPropertyChanged(nameof(Icon));
            }
        }

        //->Commond to show HomeView , VendorView,...

        public ICommand ShowHomeViewCommand { get; }
        public ICommand ShowVendorViewCommand { get; }
        public ICommand ShowProductServicesViewCommand { get; }
        public ICommand ShowDetailedInfoViewCommand { get; }
        public ICommand ShowPaymentViewCommand { get; }
        public ICommand ShowPaymentNoteViewCommand { get; }
        public ICommand ShowReportViewCommand { get; }
        public ICommand ShowSettingViewCommand { get; }
        public ICommand ShowGSTViewCommand { get; }
        public ICommand LogOutCommand { get; }
        LoginViewModel _loginViewModel;
        public MainViewModel(IUserBusinessLogic userBusinessLogic, IVendorBusinessLogic vendorBusinessLogic,
                             IVendorServiceBusinessLogic vendorServiceBusinessLogic, IVendorDetailsBusinessLogic vendorDetailsBusinessLogic,
                             IVendorPaymentBusinessLogic vendorPaymentBusinessLogic, IVenderPaymentNotesBusinessLogic venderPaymentNotesBusinessLogic,
                             IGstcalculationMasterBusinessLogic gstcalculationMasterBusinessLogic, IReportExportToExcelPaymentNote reportExportToExcelPaymentNote,
                             IConfigurationBusinessLogic configurationBusinessLogic, IPaymentNoteInWord paymentNoteInWord,
                             IYearlyMonthlyReportPDF yearlyMonthlyReportPDF, IHomePageBusinessLogic homePageBusinessLogic)
        {
            Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Into the constructor", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));
            _userBusinessLogic = userBusinessLogic;
            _vendorBusinessLogic = vendorBusinessLogic;
            _vendorDetailsBusinessLogic = vendorDetailsBusinessLogic;
            _vendorPaymentBusinessLogic = vendorPaymentBusinessLogic;
            _venderPaymentNotesBusinessLogic = venderPaymentNotesBusinessLogic;
            _configurationBusinessLogic = configurationBusinessLogic;
            _paymentNoteInWord = paymentNoteInWord;
            _yearlyMonthlyReportPDF = yearlyMonthlyReportPDF;
            // _currentUserAccount = new UserAccountModel();

            //Initialize command
            ShowHomeViewCommand = new ViewModelCommand(ExecuteShowHomeViewCommand);
            ShowVendorViewCommand = new ViewModelCommand(ExecuteShowVendorViewCommand);
            ShowProductServicesViewCommand = new ViewModelCommand(ExecuteShowProductServicesViewCommand);
            ShowDetailedInfoViewCommand = new ViewModelCommand(ExecuteShowDetailedInfoViewCommand);
            ShowPaymentViewCommand = new ViewModelCommand(ExecuteShowPaymentViewCommand);
            ShowPaymentNoteViewCommand = new ViewModelCommand(ExecutePaymentNoteViewCommand);
            ShowReportViewCommand = new ViewModelCommand(ExecuteShowReportViewCommand);
            ShowSettingViewCommand = new ViewModelCommand(ExecuteShowSettingViewCommand);
            ShowGSTViewCommand = new ViewModelCommand(ExecuteShowGSTViewCommand);
            LogOutCommand = new ViewModelAsyncCommand<Window>(ExecuteLogOut);
            _vendorServiceBusinessLogic = vendorServiceBusinessLogic;
            _gstcalculationMasterBusinessLogic = gstcalculationMasterBusinessLogic;
            _reportExportToExcelPaymentNote = reportExportToExcelPaymentNote;
            _homePageBusinessLogic = homePageBusinessLogic;
            //Default view
            ExecuteShowHomeViewCommand(null);
            _ = LoadCurrentUserData();
            _loginViewModel = new LoginViewModel(userBusinessLogic);
        }

        private async Task ExecuteLogOut(Window window)
        {
            const string message = "Are you sure that you would like to Log Out?";
            const string caption = "Log Out";
            var result = MessageBox.Show(message, caption,
                         MessageBoxButtons.YesNo,
                         MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                //Not working as its shows login screen but not able to login so commenting below code and just closing the application

                RestartApplication();
            }
        }
        private static void RestartApplication()
        {
            // Get the path to the executable file
            var fileName = Process.GetCurrentProcess().MainModule?.FileName;

            // Start a new instance of the application
            Process.Start(fileName?? "");

            // Close the current application
            Application.Current.Shutdown();
        }
        private void ExecuteShowGSTViewCommand(object t)
        {
            try
            {
                CurrentChildView = new GSTViewModel(_gstcalculationMasterBusinessLogic, this);
                Caption = "GTS Master";
                Icon = IconChar.RankingStar;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, string.Format("Class: {0}, Method: {1} - Failed to Load Submenu", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));
            }
        }

        private void ExecuteShowSettingViewCommand(object obj)
        {
            try
            {
                CurrentChildView = new SettingsViewModel(_configurationBusinessLogic);
                Caption = "Settings";
                Icon = IconChar.Gears;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, string.Format("Class: {0}, Method: {1} - Failed to Load Submenu", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));
            }
        }

        private void ExecuteShowReportViewCommand(object obj)
        {
            try
            {
                CurrentChildView = new ReportsViewModel(_reportExportToExcelPaymentNote, _vendorBusinessLogic, _vendorDetailsBusinessLogic, _configurationBusinessLogic, _paymentNoteInWord, _yearlyMonthlyReportPDF);
                Caption = "Reports";
                Icon = IconChar.File;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, string.Format("Class: {0}, Method: {1} - Failed to Load Submenu", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));
            }
        }

        private void ExecutePaymentNoteViewCommand(object obj)
        {
            try
            {
                CurrentChildView = new PaymentNotesViewModel(this, _venderPaymentNotesBusinessLogic, _vendorDetailsBusinessLogic, _vendorBusinessLogic, _configurationBusinessLogic);
                Caption = "Payment Notes";
                Icon = IconChar.NoteSticky;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, string.Format("Class: {0}, Method: {1} - Failed to Load Submenu", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));
            }
        }

        private void ExecuteShowPaymentViewCommand(object obj)
        {
            try
            {
                CurrentChildView = new PaymentsViewModel(this, _vendorPaymentBusinessLogic, _vendorDetailsBusinessLogic, _gstcalculationMasterBusinessLogic, _vendorBusinessLogic, _venderPaymentNotesBusinessLogic, _configurationBusinessLogic);
                Caption = "Payments";
                Icon = IconChar.Paypal;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, string.Format("Class: {0}, Method: {1} - Failed to Load Submenu", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));
            }
        }

        private void ExecuteShowDetailedInfoViewCommand(object obj)
        {
            try
            {
                CurrentChildView = new DetailedInfoViewModel(this, _vendorDetailsBusinessLogic, _vendorServiceBusinessLogic, _vendorBusinessLogic, _configurationBusinessLogic);
                Caption = "Detailed Info";
                Icon = IconChar.InfoCircle;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, string.Format("Class: {0}, Method: {1} - Failed to Load Submenu", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));
            }
        }

        private void ExecuteShowProductServicesViewCommand(object obj)
        {
            try
            {
                CurrentChildView = new ProductServicesViewModel(_vendorServiceBusinessLogic, _vendorBusinessLogic, this);
                Caption = "Products Services";
                Icon = IconChar.ProductHunt;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, string.Format("Class: {0}, Method: {1} - Failed to Load Submenu", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));
            }
        }

        private void ExecuteShowVendorViewCommand(object obj)
        {
            try
            {
                CurrentChildView = new VendorViewModel(_vendorBusinessLogic, this);
                Caption = "Vendors";
                Icon = IconChar.UserGroup;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, string.Format("Class: {0}, Method: {1} - Failed to Load Submenu", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));
            }
        }

        private void ExecuteShowHomeViewCommand(object obj)
        {
            try
            {
                CurrentChildView = new HomeViewModel(_homePageBusinessLogic, _configurationBusinessLogic);
                Caption = "Home";
                Icon = IconChar.Home;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, string.Format("Class: {0}, Method: {1} - Failed to Load Submenu", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));
            }
        }

        private async Task LoadCurrentUserData()
        {
            var user = await _userBusinessLogic.GetByUsername(Thread.CurrentPrincipal?.Identity?.Name ?? "").ConfigureAwait(false);
            if (user != null)
            {

                //CurrentUserAccount.Username = user.Username;
                //CurrentUserAccount.DisplayName = $"Welcome {user.Name} {user.LastName} ;)";
                UserAccountModel.Username = user.Username;
                UserAccountModel.DisplayName = $"Welcome {user.Name} {user.LastName} ;)";

            }
            else
            {
                //CurrentUserAccount.DisplayName = "Invalid user, not logged in";
                UserAccountModel.DisplayName = $"Invalid user, not logged in";
            }
        }
    }
}
