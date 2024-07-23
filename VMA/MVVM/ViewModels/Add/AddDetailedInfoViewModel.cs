using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.Abstraction.VMA.Models;
using BusinessLogic.VMA;
using Database.VMA.Entities;
using Database.VMA.Repositories;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.ExtendedProperties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VMA.MVVM.Models;
using VMA.MVVM.ViewModels.Menus;

namespace VMA.MVVM.ViewModels.Add
{
    public class AddDetailedInfoViewModel : ViewModelBase
    {
        private int _selectedTabIndex;
        private int _numbersOfTab = 1;
        private readonly DetailedInfoViewModel _detailedInfoViewModel;
        private readonly IVendorDetailsBusinessLogic _vendorDetailsBusinessLogic;
        private readonly VendorDetailModel _vendorDetailViewModel;
        private readonly IVendorServiceBusinessLogic _vendorServiceBusinessLogic;
        private readonly IVendorBusinessLogic _vendorBusinessLogic;
        private readonly IConfigurationBusinessLogic _configurationBusinessLogic;
        private string _saveButtonName;

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
                var res = _detailsLsit.FirstOrDefault(x => x.VendorServiceName == _selectedVendorDetailService?.VendorServiceName);
                var msg1 = @$"Vendor Details alreday added for {_selectedVendorDetailService?.VendorServiceName}";

                if (res != null)
                {
                    var msg = @$"Vendor Details alreday added for {_selectedVendorDetailService?.VendorServiceName}";
                    MessageBox.Show(msg);

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

        private SearchModel _selectPaymentType;
        public SearchModel? SelectPaymentType
        {
            get { return _selectPaymentType; }
            set { _selectPaymentType = value; }
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


        #endregion

        public string SaveButtonName
        {
            get => _saveButtonName;

            set
            {
                _saveButtonName = value;
                OnPropertyChanged(nameof(SaveButtonName));
            }
        }
        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set
            {
                _selectedTabIndex = value;
                OnPropertyChanged(nameof(SelectedTabIndex));
                OnPropertyChanged(nameof(CanGoBack));
                OnPropertyChanged(nameof(CanGoNext));
            }
        }

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
        public ICommand BackCommand { get; set; }
        public ICommand NextCommand { get; set; }
        public ICommand HideDetailInfoFormCommand { get; }
        public ICommand SubmitCommand { get; }
        public ICommand ClearFormCommand { get; }

        #endregion

        ObservableCollection<VendorDetailModel> _detailsLsit;
        public AddDetailedInfoViewModel(DetailedInfoViewModel detailedInfoViewModel, VendorDetailModel vendorDetailViewModel, IVendorDetailsBusinessLogic vendorDetailsBusinessLogic, IVendorServiceBusinessLogic vendorServiceBusinessLogic, IVendorBusinessLogic vendorBusinessLogic, ObservableCollection<VendorDetailModel> detailsLsit, IConfigurationBusinessLogic configurationBusinessLogic)
        {
            _configurationBusinessLogic= configurationBusinessLogic;
           
            _detailsLsit = detailsLsit;
            ComboxPaymentMethods =
            [
                new(){NameSearch="Monthly",SearchId=1},
                new(){NameSearch="Quarterly",SearchId=2},
                new(){NameSearch="Half Yearly",SearchId=3},
                new(){NameSearch="Yearly",SearchId=4},
                new(){NameSearch="None",SearchId=5},
            ];

            _vendorDetailViewModel = vendorDetailViewModel;
            _vendorServiceBusinessLogic = vendorServiceBusinessLogic;
            _vendorDetailsBusinessLogic = vendorDetailsBusinessLogic;
            _vendorBusinessLogic = vendorBusinessLogic;
            BackCommand = new ViewModelCommand(CanGoBack);
            NextCommand = new ViewModelCommand(CanGoNext);
            SubmitCommand = new ViewModelAsyncCommand<VendorDetailModel>(SaveVendorServiceDetails, ValidateVendorServiceDetails);
            ClearFormCommand = new ViewModelAsyncCommand<VendorDetailModel>(ClearFormFields);
            if (_vendorDetailViewModel != null)
            {
                IsComboBoxVendorVisible = false;
                IsComboBoxServiceVisible = false;
                IsTextBoxSelectedVendorVisible = true;
                IsTextBoxServiceVisible = true;
                SaveButtonName = "Update";
            }
            else
            {
                IsComboBoxVendorVisible = true;
                IsComboBoxServiceVisible = true;
                IsTextBoxSelectedVendorVisible = false;
                IsTextBoxServiceVisible = false;
                SaveButtonName = "Submit";
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

        private bool ValidateVendorServiceDetails()
        {
            return true;
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
        public async Task GetAllConfigurations()
        {
            var allConfigurations = await _configurationBusinessLogic.GetConfigurations().ConfigureAwait(true);
            string? departmentConfigJson = allConfigurations.FirstOrDefault(x => x.Cfgkey == nameof(Menus.Department))?.CfgValue;
            string? expenditureConfigJson = allConfigurations.FirstOrDefault(x => x.Cfgkey == nameof(Expenditure))?.CfgValue;
            string? sanctionConfigJson = allConfigurations.FirstOrDefault(x => x.Cfgkey == nameof(Sanction))?.CfgValue;
            string? financialYear = allConfigurations.FirstOrDefault(x => x.Cfgkey == "FinancialYear")?.CfgValue;
            string? noteID = allConfigurations.FirstOrDefault(x => x.Cfgkey == nameof(NoteId))?.CfgValue;
            
            Random random = new Random();
            int randomNumber = random.Next(100, 1000);

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
        }
        private async Task SaveVendorServiceDetails(VendorDetailModel model)
        {
            if (SaveButtonName == "Update")
            {
                VendorDetailModel vendorModel = new()
                {
                    IsActive = true,
                    QuantityOfUnit = QuantityOfUnit,
                    ServiceSantionAmount = ServiceSantionAmount,
                    ServiceEndDate = ServiceEndDate,
                    RatePerUnit = RatePerUnit,
                    ServiceType = ServiceType,
                    VendorDetailCategory = VendorDetailCategory,
                    ServiceStartDate = ServiceStartDate,
                    ServicePaymentType = SelectPaymentType?.NameSearch,
                    ServiceSantionedBy = ServiceSantionedBy,
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

                SuccessPopupViewModel.Instance.ShowPopup(Enums.NotificationType.Success, "Data Updated Successfully");
            }
            else
            {
                VendorDetailModel vendorModel = new()
                {
                    IsActive = true,
                    CreatedBy = UserAccountModel.Username,
                    CreatedDate = DateTime.UtcNow,
                    QuantityOfUnit = QuantityOfUnit,
                    ServiceSantionAmount = ServiceSantionAmount,
                    ServiceEndDate = ServiceEndDate,
                    RatePerUnit = RatePerUnit,
                    ServiceType = ServiceType,
                    VendorDetailCategory = VendorDetailCategory,
                    ServiceStartDate = ServiceStartDate,
                    ServicePaymentType = SelectPaymentType?.NameSearch,
                    ServiceSantionedBy = ServiceSantionedBy,
                    DetailsYear = ServiceYear,
                    IsAmc = IsAmcYes != true ? false : true,
                    SantionedDate = SantionedDate,
                    VendorServiceName = SelectedVendorDetailService?.VendorServiceName,
                    VendorName = SelectedVendorModel.VendorName,
                    SantionedNoteNo = SantionedNoteNo,
                    VendorServiceId = SelectedVendorDetailService.VendorServiceId,
                    FkVendorId = SelectedVendorModel.VendorId,
                    FkVendorServiceId = SelectedVendorDetailService.VendorServiceId,
                    VendorId = SelectedVendorModel.VendorId,
                    VendorCode = SelectedVendorModel.VendorCode
                };
                await _vendorDetailsBusinessLogic.AddVendorDetails(vendorModel);

                SuccessPopupViewModel.Instance.ShowPopup(Enums.NotificationType.Success, "Data Added Successfully");
            }

            await HideDetailInfoForm(this);
        }

        #region Async Call

        private async void CallAync()
        {
            await MainTask();
        }
        public async Task MainTask()
        {
            await LoadVendors();
            await PopulateValues();
            await GetAllConfigurations();
        }

        #endregion

        private void CanGoBack(object obj)
        {
            if (SelectedTabIndex < 0)
                SelectedTabIndex--;
        }

        private void CanGoNext(object obj)
        {
            if (SelectedTabIndex < _numbersOfTab)
                SelectedTabIndex++;
        }
        private async Task PopulateValues()
        {
            if (_vendorDetailViewModel != null)
            {
                ServiceYear = _vendorDetailViewModel.DetailsYear;
                SantionedDate = _vendorDetailViewModel?.SantionedDate != null ? (DateOnly)_vendorDetailViewModel.SantionedDate : DateOnly.MinValue;
                ServiceSantionedBy = _vendorDetailViewModel?.ServiceSantionedBy ?? "";
                ServiceStartDate = _vendorDetailViewModel?.ServiceStartDate != null ? (DateOnly)_vendorDetailViewModel.ServiceStartDate : DateOnly.MinValue;
                ServiceEndDate = _vendorDetailViewModel?.ServiceEndDate != null ? (DateOnly)_vendorDetailViewModel.ServiceEndDate : DateOnly.MinValue;
                ServiceSantionAmount = _vendorDetailViewModel?.ServiceSantionAmount ?? 0;
                RatePerUnit = _vendorDetailViewModel?.RatePerUnit ?? "";
                QuantityOfUnit = _vendorDetailViewModel?.QuantityOfUnit ?? 0;
                ServiceType = _vendorDetailViewModel?.ServiceType ?? "";
                VendorDetailCategory = _vendorDetailViewModel?.VendorDetailCategory ?? "";
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
            }
        }


        #region Combobox load vendors and services on combo box selection 

        /// <summary>
        /// Combobox load Vendor Service Name on selection of Vendor
        /// </summary>
        /// <returns></returns>
        private async Task LoadVendorServiceDetails(int vendorId)
        {
            var vendorServiceDetails = await _vendorServiceBusinessLogic.GetAllVendorServices().ConfigureAwait(true);

            VendorDetailServices = new ObservableCollection<VendorServiceModel>(vendorServiceDetails.Where(x => x.FkVendorId == vendorId));
        }

        /// <summary>
        /// Combo box load vendors
        /// </summary>
        /// <returns></returns>
        private async Task LoadVendors()
        {
            var vendors = await _vendorBusinessLogic.GetAllVendor().ConfigureAwait(true);
            VendorModels = new ObservableCollection<VendorModel>(vendors);
        }

        #endregion

        private async Task HideDetailInfoForm(object model)
        {
            await _detailedInfoViewModel.HideDetailInfoForm(this).ConfigureAwait(true);
        }

        #region Combo box Vendor Service Visibility setting on Add and edit

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
        #endregion
    }

}
