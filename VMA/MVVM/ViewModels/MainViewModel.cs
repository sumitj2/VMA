using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.Abstraction.VMA.Models;
using FontAwesome.Sharp;
using Serilog;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using VMA.Constants;
using VMA.MVVM.Models;
using VMA.MVVM.ViewModels.Login;
using VMA.MVVM.ViewModels.Menus;
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
        private readonly IImportFromExcel _importFromExcel;
        public MainViewModel(IUserBusinessLogic userBusinessLogic, IVendorBusinessLogic vendorBusinessLogic,
                             IVendorServiceBusinessLogic vendorServiceBusinessLogic, IVendorDetailsBusinessLogic vendorDetailsBusinessLogic,
                             IVendorPaymentBusinessLogic vendorPaymentBusinessLogic, IVenderPaymentNotesBusinessLogic venderPaymentNotesBusinessLogic,
                             IGstcalculationMasterBusinessLogic gstcalculationMasterBusinessLogic, IReportExportToExcelPaymentNote reportExportToExcelPaymentNote,
                             IConfigurationBusinessLogic configurationBusinessLogic, IPaymentNoteInWord paymentNoteInWord,
                             IYearlyMonthlyReportPDF yearlyMonthlyReportPDF, IHomePageBusinessLogic homePageBusinessLogic, IImportFromExcel importFromExcel)
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
            _vendorServiceBusinessLogic = vendorServiceBusinessLogic;
            _gstcalculationMasterBusinessLogic = gstcalculationMasterBusinessLogic;
            _reportExportToExcelPaymentNote = reportExportToExcelPaymentNote;
            _homePageBusinessLogic = homePageBusinessLogic;
            _importFromExcel = importFromExcel;
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

            _ = LoadCurrentUserData();
            GetConfiguration();
            _loginViewModel = new LoginViewModel(userBusinessLogic);

            if (settings == null || settings?.Count == 0 || settings?.ToList()?.FirstOrDefault()?.Cfgkey == "")
            {
                SuccessPopupViewModel.Instance.ShowPopup(Enums.NotificationType.Alert, MessagesContants.PleaseAddConfiguratonDetails, false, true);
                ExecuteShowSettingViewCommand(null);
            }
            else
            {
                //Default view
                ExecuteShowHomeViewCommand(null);
            }
        }

        private async Task ExecuteLogOut(Window window)
        {
            const string message = MessagesContants.LogOutMsg;
            const string caption = MessagesContants.CaptionLogOut;
            var result = MessageBox.Show(message, caption,
                         MessageBoxButtons.YesNo,
                         MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                RestartApplication();
            }
        }
        private static void RestartApplication()
        {
            // Get the path to the executable file
            var fileName = Process.GetCurrentProcess().MainModule?.FileName;

            // Start a new instance of the application
            Process.Start(fileName ?? "");

            // Close the current application
            Application.Current.Shutdown();
        }
        private void ExecuteShowGSTViewCommand(object t)
        {
            try
            {
                CurrentChildView = new GSTViewModel(_gstcalculationMasterBusinessLogic, this);
                Caption = MessagesContants.CaptionGSTMaster;
                Icon = IconChar.RankingStar;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, string.Format("Class: {0}, Method: {1} - Failed to Load Submenu", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));
            }
        }

        private void ExecuteShowSettingViewCommand(object? obj)
        {
            try
            {
                CurrentChildView = new SettingsViewModel(_configurationBusinessLogic,_importFromExcel);
                Caption = MessagesContants.CaptionSettings;
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
                Caption = MessagesContants.CaptionReports;
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
                Caption = MessagesContants.CaptionPaymentNotes;
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
                Caption = MessagesContants.CaptionPayment;
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
                Caption = MessagesContants.CaptionDetailedInfo;
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
                Caption = MessagesContants.CaptionProductServices;
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
                Caption = MessagesContants.CaptionVendors;
                Icon = IconChar.UserGroup;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, string.Format("Class: {0}, Method: {1} - Failed to Load Submenu", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));
            }
        }

        private void ExecuteShowHomeViewCommand(object? obj)
        {
            try
            {
                CurrentChildView = new HomeViewModel(_homePageBusinessLogic, _configurationBusinessLogic);
                Caption = MessagesContants.CaptionHome;
                Icon = IconChar.Home;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, string.Format("Class: {0}, Method: {1} - Failed to Load Submenu", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));
            }
        }

        private ObservableCollection<ConfigurationModel> settings;

        public ObservableCollection<ConfigurationModel> Settings
        {
            get { return settings; }
            set { settings = value; }
        }

        private async Task LoadCurrentUserData()
        {
            var user = await _userBusinessLogic.GetByUsername(Thread.CurrentPrincipal?.Identity?.Name ?? "").ConfigureAwait(true);
            if (user != null)
            {
                UserAccountModel.Username = user.Username;
                UserAccountModel.DisplayName = $"Welcome {user.Name} {user.LastName} ;)";

            }
            else
            {
                UserAccountModel.DisplayName = MessagesContants.InvalidUserNotLogIn;
            }
        }

        private void GetConfiguration()
        {
            try
            {
                Task.Run(() =>
                {
                    settings = new ObservableCollection<ConfigurationModel>(_configurationBusinessLogic.GetConfigurations().GetAwaiter().GetResult());
                }).Wait();
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, string.Format("Class: {0}, Method: {1} - Failed to GetConfiguration", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));
            }
            
        }
    }
}
