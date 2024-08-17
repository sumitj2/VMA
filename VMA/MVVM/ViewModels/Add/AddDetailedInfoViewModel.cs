using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.Abstraction.VMA.Models;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using VMA.Constants;
using VMA.MVVM.Models;
using VMA.MVVM.ViewModels.Menus;

namespace VMA.MVVM.ViewModels.Add
{
    public class AddDetailedInfoViewModel : ViewModelBase
    {
        private readonly DetailedInfoViewModel _detailedInfoViewModel;
        private readonly IVendorDetailsBusinessLogic _vendorDetailsBusinessLogic;
        private readonly VendorDetailModel _vendorDetailViewModel;
        private readonly IVendorServiceBusinessLogic _vendorServiceBusinessLogic;
        private readonly IVendorBusinessLogic _vendorBusinessLogic;
        private readonly IConfigurationBusinessLogic _configurationBusinessLogic;

        #region Properties

        private string? _serviceYear;
        public string? ServiceYear
        {
            get { return _serviceYear; }
            set
            {
                _serviceYear = value;
                OnPropertyChanged(nameof(ServiceYear));
            }

        }

        private VendorServiceModel? _selectedVendorDetailService;
        public VendorServiceModel? SelectedVendorDetailService
        {
            get { return _selectedVendorDetailService; }
            set
            {
                _selectedVendorDetailService = value;
                OnPropertyChanged(nameof(SelectedVendorDetailService));
                var res = _detailsLsit.FirstOrDefault(x => x.VendorServiceName == _selectedVendorDetailService?.VendorServiceName && x.FkVendorId== _selectedVendorDetailService.FkVendorId);
                var msg1 = @$"{MessagesContants.VendorDetailMsg} {_selectedVendorDetailService?.VendorServiceName}";

                if (res != null)
                {
                    SuccessPopupViewModel.Instance.ShowPopup(Enums.NotificationType.Alert, msg1, false, true);
                    _ = HideDetailInfoForm(this);
                }

            }
        }

        private VendorModel _SelectedVendorModel;
        public VendorModel SelectedVendorModel
        {
            get { return _SelectedVendorModel; }
            set
            {
                _SelectedVendorModel = value;
                OnPropertyChanged(nameof(SelectedVendorModel));
                if (SelectedVendorModel != null)
                    _ = LoadVendorServiceDetails(SelectedVendorModel.VendorId);
            }
        }

        private string _vendorDetailCategory;
        public string VendorDetailCategory
        {
            get
            {
                return _vendorDetailCategory;
            }
            set
            {
                _vendorDetailCategory = value;
                OnPropertyChanged(nameof(VendorDetailCategory));
            }
        }

        private string _serviceType;
        public string ServiceType
        {
            get
            {
                return _serviceType;
            }
            set
            {
                _serviceType = value;
                OnPropertyChanged(nameof(ServiceType));
            }
        }

        private DateOnly _serviceStartDate;
        public DateOnly ServiceStartDate
        {
            get
            {
                return _serviceStartDate;
            }
            set
            {
                _serviceStartDate = value;
                OnPropertyChanged(nameof(ServiceStartDate));
            }
        }

        private DateOnly _serviceEndDate;
        public DateOnly ServiceEndDate
        {
            get
            {
                return _serviceEndDate;
            }
            set
            {
                _serviceEndDate = value;
                OnPropertyChanged(nameof(ServiceEndDate));
            }
        }

        private string _serviceSantionedBy;
        public string ServiceSantionedBy
        {
            get
            {
                return _serviceSantionedBy;
            }
            set
            {
                _serviceSantionedBy = value;
                OnPropertyChanged(nameof(ServiceSantionedBy));
            }
        }

        private decimal? _serviceSantionAmount;

        private bool _enableDisableSantionedAmt;

        public bool EnableDisableSantionedAmt
        {
            get { return _enableDisableSantionedAmt; }
            set
            {
                _enableDisableSantionedAmt = value;
                OnPropertyChanged(nameof(EnableDisableSantionedAmt));
            }
        }

        public decimal? ServiceSantionAmount
        {
            get
            {
                return _serviceSantionAmount;
            }
            set
            {
                _serviceSantionAmount = value;
                OnPropertyChanged(nameof(ServiceSantionAmount));
            }
        }

        private DateOnly _santionedDate;
        public DateOnly SantionedDate
        {
            get { return _santionedDate; }
            set
            {
                _santionedDate = value;
                OnPropertyChanged(nameof(SantionedDate));
            }
        }

        private string _santionedNoteNo;
        public string SantionedNoteNo
        {
            get { return _santionedNoteNo; }
            set
            {
                _santionedNoteNo = value;
                OnPropertyChanged(nameof(SantionedNoteNo));
            }
        }

        private int _quantityOfUnit;
        public int QuantityOfUnit
        {
            get
            {
                return _quantityOfUnit;
            }
            set
            {
                _quantityOfUnit = value;
                OnPropertyChanged(nameof(QuantityOfUnit));
            }
        }

        private string _ratePerUnit;
        public string RatePerUnit
        {
            get
            {
                return _ratePerUnit;
            }
            set
            {
                _ratePerUnit = value;
                OnPropertyChanged(nameof(RatePerUnit));
            }
        }

        private SearchModel? _selectPaymentType;
        [Required(ErrorMessage = MessagesContants.PaymentTypeRequired)]
        public SearchModel? SelectPaymentType
        {
            get { return _selectPaymentType; }
            set
            {
                _selectPaymentType = value;
                if (_selectPaymentType?.NameSearch == GeneralConstants.PaymentTypeNone)
                {
                    EnableDisableSantionedAmt=false;
                    ServiceSantionAmount = null;
                }
                else
                {
                    EnableDisableSantionedAmt = true;
                }
                OnPropertyChanged(nameof(SelectPaymentType));
            }
        }

        private bool _isAmcYes;
        public bool IsAmcYes
        {
            get { return _isAmcYes; }
            set
            {
                _isAmcYes = value;
                OnPropertyChanged(nameof(IsAmcYes));
            }
        }

        private bool _isAmcNo;
        public bool IsAmcNo
        {
            get { return _isAmcNo; }
            set
            {
                _isAmcNo = value;
                OnPropertyChanged(nameof(IsAmcNo));
            }
        }

        private string? _SelectedPaymentTypeText;

        public string? SelectedPaymentTypeText
        {
            get { return _SelectedPaymentTypeText; }
            set
            {
                _SelectedPaymentTypeText = value;
                OnPropertyChanged(nameof(SelectedPaymentTypeText));
            }
        }

        private string _selectedVendorName;

        public string SelectedVendorName
        {
            get { return _selectedVendorName; }
            set
            {
                _selectedVendorName = value;
                OnPropertyChanged(nameof(SelectedVendorName));
            }
        }
        private string _selctedVendorServiceName;

        public string SelctedVendorServiceName
        {
            get { return _selctedVendorServiceName; }
            set
            {
                _selctedVendorServiceName = value;
                OnPropertyChanged(nameof(SelctedVendorServiceName));
            }
        }

        private string _saveButtonName;

        public string SaveButtonName
        {
            get => _saveButtonName;

            set
            {
                _saveButtonName = value;
                OnPropertyChanged(nameof(SaveButtonName));
            }
        }

        private ObservableCollection<Menus.Department> departments;
        public ObservableCollection<Menus.Department> Departments
        {
            get
            { return departments; }
            set
            {
                departments = value;
                OnPropertyChanged(nameof(Departments));
            }
        }

        private Menus.Department selectedDepartment;
        public Menus.Department SelectedDepartment
        {
            get
            { return selectedDepartment; }
            set
            {
                selectedDepartment = value;
                OnPropertyChanged(nameof(SelectedDepartment));
            }
        }

        private ObservableCollection<Expenditure> expenditures;
        public ObservableCollection<Expenditure> Expenditures
        {
            get
            { return expenditures; }
            set
            {
                expenditures = value;
                OnPropertyChanged(nameof(Expenditures));
            }
        }

        private Expenditure selectedExpenditure;
        public Expenditure SelectedExpentidure
        {
            get
            { return selectedExpenditure; }
            set
            {
                selectedExpenditure = value;
                OnPropertyChanged(nameof(SelectedExpentidure));
            }
        }

        private ObservableCollection<Sanction> sanctions;
        public ObservableCollection<Sanction> Sanctions
        {
            get
            { return sanctions; }
            set
            {
                sanctions = value;
                OnPropertyChanged(nameof(Sanctions));
            }
        }

        private Sanction selectedSanction;
        public Sanction SelectedSanction
        {
            get
            { return selectedSanction; }
            set
            {
                selectedSanction = value;
                OnPropertyChanged(nameof(SelectedSanction));
            }
        }


        private string? _noteId;

        public string? NoteId
        {
            get { return _noteId; }
            set
            {
                _noteId = value;
                OnPropertyChanged(nameof(NoteId));
            }
        }

        private bool _IsComboBoxServiceVisible;

        public bool IsComboBoxServiceVisible
        {
            get { return _IsComboBoxServiceVisible; }
            set
            {
                _IsComboBoxServiceVisible = value;
                OnPropertyChanged(nameof(HideServiceSelectComboBox));
            }
        }

        private bool _IsComboPaymentVisible;

        public bool IsComboPaymentVisible
        {
            get { return _IsComboPaymentVisible; }
            set
            {
                _IsComboPaymentVisible = value;
                OnPropertyChanged(nameof(HidePaymentTypeSelectComboBox));
            }
        }

        private bool _IsComboBoxVendorVisible;

        public bool IsComboBoxVendorVisible
        {
            get { return _IsComboBoxVendorVisible; }
            set
            {
                _IsComboBoxVendorVisible = value;
                OnPropertyChanged(nameof(HideVendorSelectComboBox));
            }
        }


        private bool _IsTextBoxServiceVisible;

        public bool IsTextBoxServiceVisible
        {
            get { return _IsTextBoxServiceVisible; }
            set
            {
                _IsTextBoxServiceVisible = value;
                OnPropertyChanged(nameof(HideSelectedService));
            }
        }

        private bool _IsTextBoxSelectedVendorVisible;

        public bool IsTextBoxSelectedVendorVisible
        {
            get { return _IsTextBoxSelectedVendorVisible; }
            set
            {
                _IsTextBoxSelectedVendorVisible = value;
                OnPropertyChanged(nameof(HideSelectedVendor));
            }
        }

        private bool _IsTextBoxPaymentVisible;

        public bool IsTextBoxPaymentVisible
        {
            get { return _IsTextBoxPaymentVisible; }
            set { _IsTextBoxPaymentVisible = value; }
        }

        public Visibility HideSelectedPaymentType
        {
            get { return IsTextBoxPaymentVisible ? Visibility.Visible : Visibility.Collapsed; }
        }
        public Visibility HideSelectedVendor
        {
            get { return IsTextBoxSelectedVendorVisible ? Visibility.Visible : Visibility.Collapsed; }
        }

        public Visibility HideSelectedService
        {
            get { return IsTextBoxServiceVisible ? Visibility.Visible : Visibility.Collapsed; }
        }

        public Visibility HideServiceSelectComboBox
        {
            get { return IsComboBoxServiceVisible ? Visibility.Visible : Visibility.Collapsed; }
        }

        public Visibility HideVendorSelectComboBox
        {
            get { return IsComboBoxServiceVisible ? Visibility.Visible : Visibility.Collapsed; }
        }

        public Visibility HidePaymentTypeSelectComboBox
        {
            get { return IsComboPaymentVisible ? Visibility.Visible : Visibility.Collapsed; }
        }
        #endregion

        #region Observable collections
        private ObservableCollection<VendorServiceModel> _vendorDetailServices;
        private ObservableCollection<SearchModel> _comboxPaymentMethod;

        //Combo box Get detail Service
        public ObservableCollection<VendorServiceModel> VendorDetailServices
        {
            get { return _vendorDetailServices; }
            set
            {
                _vendorDetailServices = value;
                OnPropertyChanged(nameof(VendorDetailServices));
            }
        }

        private ObservableCollection<VendorModel> _vendorModels;

        public ObservableCollection<VendorModel> VendorModels
        {
            get { return _vendorModels; }
            set
            {
                _vendorModels = value;
                OnPropertyChanged(nameof(VendorModels));
            }
        }


        public ObservableCollection<SearchModel> ComboxPaymentMethods
        {
            get { return _comboxPaymentMethod; }
            set { _comboxPaymentMethod = value; }
        }
        #endregion

        #region Command        
        public ICommand HideDetailInfoFormCommand { get; }
        public ICommand SubmitCommand { get; }
        public ICommand ClearFormCommand { get; }

        #endregion

        ObservableCollection<VendorDetailModel> _detailsLsit;
        public AddDetailedInfoViewModel(DetailedInfoViewModel detailedInfoViewModel, VendorDetailModel vendorDetailViewModel, IVendorDetailsBusinessLogic vendorDetailsBusinessLogic, IVendorServiceBusinessLogic vendorServiceBusinessLogic, IVendorBusinessLogic vendorBusinessLogic, ObservableCollection<VendorDetailModel> detailsLsit, IConfigurationBusinessLogic configurationBusinessLogic)
        {
            EnableDisableSantionedAmt = true;
            Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Into the constructor", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));

            _configurationBusinessLogic = configurationBusinessLogic;

            _detailsLsit = detailsLsit;
            ComboxPaymentMethods =
            [
                new(){NameSearch=GeneralConstants.PaymentTypeMonthly,SearchId=1},
                new(){NameSearch=GeneralConstants.PaymentTypeQuarterly,SearchId=2},
                new(){NameSearch=GeneralConstants.PaymentTypeHalfYearly,SearchId=3},
                new(){NameSearch=GeneralConstants.PaymentTypeYearly,SearchId=4},
                new(){NameSearch=GeneralConstants.PaymentTypeNone,SearchId=5},
            ];

            _vendorDetailViewModel = vendorDetailViewModel;
            _vendorServiceBusinessLogic = vendorServiceBusinessLogic;
            _vendorDetailsBusinessLogic = vendorDetailsBusinessLogic;
            _vendorBusinessLogic = vendorBusinessLogic;
            SubmitCommand = new ViewModelAsyncCommand<VendorDetailModel>(SaveVendorServiceDetails, ValidateVendorServiceDetails);
            ClearFormCommand = new ViewModelAsyncCommand<VendorDetailModel>(ClearFormFields);
            if (_vendorDetailViewModel != null)
            {
                IsComboPaymentVisible = false;
                IsComboBoxVendorVisible = false;
                IsComboBoxServiceVisible = false;
                IsTextBoxSelectedVendorVisible = true;
                IsTextBoxServiceVisible = true;
                IsTextBoxPaymentVisible = true;
                SaveButtonName = GeneralConstants.Update;
            }
            else
            {
                IsComboPaymentVisible = true;
                IsComboBoxVendorVisible = true;
                IsComboBoxServiceVisible = true;
                IsTextBoxSelectedVendorVisible = false;
                IsTextBoxServiceVisible = false;
                IsTextBoxPaymentVisible = false;
                SaveButtonName = GeneralConstants.Submit;
            }
            _detailedInfoViewModel = detailedInfoViewModel;
            HideDetailInfoFormCommand = new ViewModelAsyncCommand<VendorDetailModel>(HideDetailInfoForm);
            _vendorDetailViewModel = vendorDetailViewModel;
            CallAync();
        }

        private async Task ClearFormFields(VendorDetailModel model)
        {
            await Task.Run(() =>
            {
                ServiceSantionedBy = "";
                ServiceStartDate = DateOnly.MinValue;
                ServiceEndDate = DateOnly.MinValue;
                ServiceSantionAmount = 0;
                RatePerUnit = "";
                QuantityOfUnit = 0;
                ServiceType = "";
                VendorDetailCategory = "";
                SelectedVendorDetailService = null;
                SelectPaymentType = null;

            });
        }
        string errorMsg = "";
        private bool ValidateVendorServiceDetails()
        {
            return true;
            //bool validData;


            //if (SelectedVendorDetailService == null)
            //{
            //    validData = false;
            //    errorMsg += nameof(SelectedVendorDetailService);
            //}
            //else
            //{
            //    validData = true;
            //}
            //if (SelectedDepartment == null)
            //{
            //    validData = false;
            //    errorMsg += nameof(SelectedDepartment);
            //}
            //else
            //{
            //    validData = true;
            //}
            //if (selectedExpenditure == null)
            //{
            //    validData = false;
            //    errorMsg += nameof(selectedExpenditure);
            //}
            //else
            //{
            //    validData = true;
            //}
            //if (SelectPaymentType == null)
            //{
            //    validData = false;
            //    errorMsg += nameof(SelectPaymentType);
            //}
            //else
            //{
            //    validData = true;
            //}

            //if (SantionedDate == DateOnly.MinValue)
            //{
            //    validData = false;
            //    errorMsg += nameof(SantionedDate);
            //}
            //else
            //{
            //    validData = true;
            //}

            //if (ServiceSantionAmount == null || ServiceSantionAmount == 0 || ServiceSantionAmount.Value == 0)
            //{
            //    validData = false;
            //    errorMsg += nameof(ServiceSantionAmount);
            //}
            //else
            //{
            //    validData = true;
            //}

            //if (SelectedSanction == null)
            //{
            //    validData = false;
            //    errorMsg += nameof(SelectedSanction);
            //}
            //else
            //{
            //    validData = true;
            //}

            //return validData;
        }
        public async Task GetAllConfigurations()
        {
            try
            {
                Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Getting all the configuration", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));

                var allConfigurations = await _configurationBusinessLogic.GetConfigurations().ConfigureAwait(true);
                string? departmentConfigJson = allConfigurations.FirstOrDefault(x => x.Cfgkey == nameof(Menus.Department))?.CfgValue;
                string? expenditureConfigJson = allConfigurations.FirstOrDefault(x => x.Cfgkey == nameof(Expenditure))?.CfgValue;
                string? sanctionConfigJson = allConfigurations.FirstOrDefault(x => x.Cfgkey == nameof(Sanction))?.CfgValue;
                string? financialYear = allConfigurations.FirstOrDefault(x => x.Cfgkey == GeneralConstants.CFGKeyFinacialYear)?.CfgValue;
                string? noteID = allConfigurations.FirstOrDefault(x => x.Cfgkey == nameof(NoteId))?.CfgValue;

                NoteId = noteID;
                ServiceYear = financialYear;

                if (!string.IsNullOrEmpty(departmentConfigJson))
                {
                    Departments = JsonSerializer.Deserialize<ObservableCollection<Menus.Department>>(departmentConfigJson);
                }
                else
                {
                    Departments = new ObservableCollection<Menus.Department>();
                }

                if (!string.IsNullOrEmpty(expenditureConfigJson))
                {
                    Expenditures = JsonSerializer.Deserialize<ObservableCollection<Expenditure>>(expenditureConfigJson);
                }
                else
                {
                    Expenditures = new ObservableCollection<Expenditure>();
                }

                if (!string.IsNullOrEmpty(sanctionConfigJson))
                {
                    Sanctions = JsonSerializer.Deserialize<ObservableCollection<Sanction>>(sanctionConfigJson);
                }
                else
                {
                    Sanctions = new ObservableCollection<Sanction>();
                }

                Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Retrieved all the configuration Successfully", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, string.Format("Class: {0}, Method: {1} - Failed to get all the configuration", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));

                SuccessPopupViewModel.Instance.ShowPopup(Enums.NotificationType.Failure, MessagesContants.ErrorMsgConfiguration, false, true);
            }
        }
        private async Task SaveVendorServiceDetails(VendorDetailModel model)
        {
            try
            {
                Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Into save Vendor service details", this.GetType().Name, MethodBase.GetCurrentMethod().Name));

                if (SaveButtonName == GeneralConstants.Update)
                {
                    VendorDetailModel vendorModel = new()
                    {
                        ServiceType = selectedExpenditure.ExpenditureName,
                        ServiceSantionedBy = SelectedSanction.SanctionName,
                        VendorDetailCategory = SelectedDepartment.DepartmentName,

                        IsActive = true,
                        QuantityOfUnit = QuantityOfUnit,
                        ServiceSantionAmount = ServiceSantionAmount,
                        ServiceEndDate = ServiceEndDate,
                        RatePerUnit = RatePerUnit,

                        ServiceStartDate = ServiceStartDate,
                        ServicePaymentType = SelectPaymentType?.NameSearch,

                        DetailsYear = ServiceYear,
                        IsAmc = IsAmcYes != true ? false : true,
                        SantionedDate = SantionedDate,
                        VendorServiceName = _vendorDetailViewModel.VendorServiceName,
                        VendorName = _vendorDetailViewModel.VendorName,
                        SantionedNoteNo = SantionedNoteNo,
                        VendorServiceId = _vendorDetailViewModel.VendorServiceId,
                        FkVendorId = _vendorDetailViewModel.VendorId,
                        FkVendorServiceId = _vendorDetailViewModel.VendorServiceId,
                        VendorId = _vendorDetailViewModel.VendorId,
                        VendorCode = _vendorDetailViewModel.VendorCode,
                        LastUpdateBy = UserAccountModel.Username,
                        VendorDetailId = _vendorDetailViewModel.VendorDetailId,
                        LastUpdatedDate = DateTime.UtcNow,

                    };
                    await _vendorDetailsBusinessLogic.EditUpdateVendorDetails(vendorModel);

                    SuccessPopupViewModel.Instance.ShowPopup(Enums.NotificationType.Success, MessagesContants.SuccessVendorDetailsUpdated, true);

                    Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Updated Vendor service details Successfully", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));
                }
                else
                {
                    VendorDetailModel vendorModel = new()
                    {
                        ServiceType = selectedExpenditure.ExpenditureName,
                        ServiceSantionedBy = SelectedSanction.SanctionName,
                        VendorDetailCategory = SelectedDepartment.DepartmentName,

                        IsActive = true,
                        CreatedBy = UserAccountModel.Username,
                        CreatedDate = DateTime.UtcNow,
                        QuantityOfUnit = QuantityOfUnit,
                        ServiceSantionAmount = ServiceSantionAmount,
                        ServiceEndDate = ServiceEndDate,
                        RatePerUnit = RatePerUnit,


                        ServiceStartDate = ServiceStartDate,
                        ServicePaymentType = SelectPaymentType?.NameSearch,
                        DetailsYear = ServiceYear,
                        IsAmc = IsAmcYes != true ? false : true,
                        SantionedDate = SantionedDate,
                        VendorServiceName = SelectedVendorDetailService?.VendorServiceName,
                        VendorName = SelectedVendorModel.VendorName,
                        SantionedNoteNo = SantionedNoteNo,
                        VendorServiceId = SelectedVendorDetailService?.VendorServiceId,
                        FkVendorId = SelectedVendorModel.VendorId,
                        FkVendorServiceId = SelectedVendorDetailService?.VendorServiceId,
                        VendorId = SelectedVendorModel.VendorId,
                        VendorCode = SelectedVendorModel.VendorCode
                    };
                    await _vendorDetailsBusinessLogic.AddVendorDetails(vendorModel);

                    Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Saved Vendor service details Successfully", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));

                    SuccessPopupViewModel.Instance.ShowPopup(Enums.NotificationType.Success, MessagesContants.SuccessVendorDetailsAdded, true);
                }

                await HideDetailInfoForm(this);
            }
            catch (Exception ex)
            {
                SuccessPopupViewModel.Instance.ShowPopup(Enums.NotificationType.Failure, MessagesContants.ErrorMessageVendorDetailsSave, false, true);

                Log.Logger.Error(ex, string.Format("Class: {0}, Method: {1} - Failed to Save vendor service details", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));
            }
        }

        #region Async Call

        private async void CallAync()
        {
            await MainTask();
        }
        public async Task MainTask()
        {
            await GetAllConfigurations();
            await LoadVendors();
            await PopulateValues();
        }

        #endregion
        private async Task PopulateValues()
        {
            if (_vendorDetailViewModel != null)
            {
                ServiceYear = _vendorDetailViewModel.DetailsYear;
                SantionedDate = _vendorDetailViewModel?.SantionedDate != null ? (DateOnly)_vendorDetailViewModel.SantionedDate : DateOnly.MinValue;
                ServiceStartDate = _vendorDetailViewModel?.ServiceStartDate != null ? (DateOnly)_vendorDetailViewModel.ServiceStartDate : DateOnly.MinValue;
                ServiceEndDate = _vendorDetailViewModel?.ServiceEndDate != null ? (DateOnly)_vendorDetailViewModel.ServiceEndDate : DateOnly.MinValue;
                ServiceSantionAmount = _vendorDetailViewModel?.ServiceSantionAmount ?? 0;
                RatePerUnit = _vendorDetailViewModel?.RatePerUnit ?? "";
                QuantityOfUnit = _vendorDetailViewModel?.QuantityOfUnit ?? 0;
                SantionedNoteNo = _vendorDetailViewModel?.SantionedNoteNo ?? "";
                bool amc = _vendorDetailViewModel?.IsAmc ?? false;
                if (amc)
                {
                    IsAmcYes = true;
                }
                else
                {
                    IsAmcNo = true;
                }
                SelctedVendorServiceName = _vendorDetailViewModel?.VendorServiceName ?? "";
                SelectedVendorName = _vendorDetailViewModel?.VendorName ?? "";

                var expenditureServiceType = expenditures.ToList().Find(x => x.ExpenditureName == _vendorDetailViewModel?.ServiceType);
                if (expenditureServiceType != null)
                {
                    SelectedExpentidure = expenditures[expenditures.IndexOf(expenditureServiceType)];

                }
                var santionedBY = sanctions.ToList().Find(x => x.SanctionName == _vendorDetailViewModel?.ServiceSantionedBy);
                if (santionedBY != null)
                {
                    SelectedSanction = sanctions[sanctions.IndexOf(santionedBY)];

                }
                var depName = departments.ToList().Find(x => x.DepartmentName == _vendorDetailViewModel?.VendorDetailCategory);
                if (depName != null)
                {
                    SelectedDepartment = departments[departments.IndexOf(depName)];

                }

                var paymentType = ComboxPaymentMethods.ToList().Find(x => x.NameSearch == _vendorDetailViewModel?.ServicePaymentType);
                if (paymentType != null)
                {
                    SelectPaymentType = ComboxPaymentMethods[ComboxPaymentMethods.IndexOf(paymentType)];
                }
                SelectedPaymentTypeText = _vendorDetailViewModel?.ServicePaymentType;
            }
        }

        #region Combobox load vendors and services on combo box selection 

        /// <summary>
        /// Combobox load Vendor Service Name on selection of Vendor
        /// </summary>
        /// <returns></returns>
        private async Task LoadVendorServiceDetails(int vendorId)
        {
            try
            {
                Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Loading vendor service details", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));

                var vendorServiceDetails = await _vendorServiceBusinessLogic.GetAllVendorServices().ConfigureAwait(true);

                VendorDetailServices = new ObservableCollection<VendorServiceModel>(vendorServiceDetails.Where(x => x.FkVendorId == vendorId));

                Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Loaded vendor service details", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));

            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, string.Format("Class: {0}, Method: {1} - Failed to load vendor service details", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));

            }
        }

        /// <summary>
        /// Combo box load vendors
        /// </summary>
        /// <returns></returns>
        private async Task LoadVendors()
        {
            try
            {
                Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Loading vendors", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));

                var vendors = await _vendorBusinessLogic.GetAllVendor().ConfigureAwait(true);
                VendorModels = new ObservableCollection<VendorModel>(vendors);

                Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Loaded vendors", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, string.Format("Class: {0}, Method: {1} - Failed to load vendors", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));
            }
        }

        #endregion

        private async Task HideDetailInfoForm(object model)
        {
            await _detailedInfoViewModel.HideDetailInfoForm(this).ConfigureAwait(true);
        }
    }

}
