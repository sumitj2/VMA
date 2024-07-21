using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.VMA;
using Database.VMA.Repositories;
using FontAwesome.Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VMA.MVVM.Models;
using VMA.MVVM.ViewModels.Add;
using VMA.MVVM.ViewModels.Menus;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

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


        public MainViewModel(IUserBusinessLogic userBusinessLogic, IVendorBusinessLogic vendorBusinessLogic, IVendorServiceBusinessLogic vendorServiceBusinessLogic, IVendorDetailsBusinessLogic vendorDetailsBusinessLogic, IVendorPaymentBusinessLogic vendorPaymentBusinessLogic, IVenderPaymentNotesBusinessLogic venderPaymentNotesBusinessLogic, IGstcalculationMasterBusinessLogic gstcalculationMasterBusinessLogic, IReportExportToExcelPaymentNote reportExportToExcelPaymentNote,IConfigurationBusinessLogic configurationBusinessLogic)
        {
            _userBusinessLogic = userBusinessLogic;
            _vendorBusinessLogic = vendorBusinessLogic;
            _vendorDetailsBusinessLogic = vendorDetailsBusinessLogic;
            _vendorPaymentBusinessLogic = vendorPaymentBusinessLogic;
            _venderPaymentNotesBusinessLogic = venderPaymentNotesBusinessLogic;
            _configurationBusinessLogic = configurationBusinessLogic;
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
            //Default view
            ExecuteShowHomeViewCommand(null);
            _ = LoadCurrentUserData();
            _vendorServiceBusinessLogic = vendorServiceBusinessLogic;
            _gstcalculationMasterBusinessLogic = gstcalculationMasterBusinessLogic;
            _reportExportToExcelPaymentNote = reportExportToExcelPaymentNote;
        }

        private void ExecuteShowGSTViewCommand(object t)
        {
            CurrentChildView = new GSTViewModel(_gstcalculationMasterBusinessLogic,this);
            Caption = "GTS Master";
            Icon = IconChar.RankingStar;
        }

        private void ExecuteShowSettingViewCommand(object obj)
        {
            CurrentChildView = new SettingsViewModel(_configurationBusinessLogic);
            Caption = "Settings";
            Icon = IconChar.Gears;
        }

        private void ExecuteShowReportViewCommand(object obj)
        {
            CurrentChildView = new ReportsViewModel(_reportExportToExcelPaymentNote);
            Caption = "Reports";
            Icon = IconChar.File;
        }

        private void ExecutePaymentNoteViewCommand(object obj)
        {
            CurrentChildView = new PaymentNotesViewModel(this, _venderPaymentNotesBusinessLogic, _vendorDetailsBusinessLogic, _vendorBusinessLogic);
            Caption = "Payment Notes";
            Icon = IconChar.NoteSticky;
        }

        private void ExecuteShowPaymentViewCommand(object obj)
        {
            CurrentChildView = new PaymentsViewModel(this, _vendorPaymentBusinessLogic, _vendorDetailsBusinessLogic, _gstcalculationMasterBusinessLogic,_vendorBusinessLogic,_venderPaymentNotesBusinessLogic);
            Caption = "Payments";
            Icon = IconChar.Paypal;
        }

        private void ExecuteShowDetailedInfoViewCommand(object obj)
        {
            CurrentChildView = new DetailedInfoViewModel(this, _vendorDetailsBusinessLogic, _vendorServiceBusinessLogic,_vendorBusinessLogic);
            Caption = "Detailed Info";
            Icon = IconChar.InfoCircle;
        }

        private void ExecuteShowProductServicesViewCommand(object obj)
        {
            CurrentChildView = new ProductServicesViewModel(_vendorServiceBusinessLogic, _vendorBusinessLogic, this);
            Caption = "Products Services";
            Icon = IconChar.ProductHunt;
        }

        private void ExecuteShowVendorViewCommand(object obj)
        {
            CurrentChildView = new VendorViewModel(_vendorBusinessLogic, this);
            Caption = "Vendors";
            Icon = IconChar.UserGroup;
        }

        private void ExecuteShowHomeViewCommand(object obj)
        {
            CurrentChildView = new HomeViewModel();
            Caption = "Home";
            Icon = IconChar.Home;
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
