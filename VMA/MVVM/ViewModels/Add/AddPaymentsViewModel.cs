using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.Abstraction.VMA.Models;
using Database.VMA.Entities;
using Database.VMA.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VMA.MVVM.Models;
using VMA.MVVM.ViewModels.Menus;

namespace VMA.MVVM.ViewModels.Add
{
    public class AddPaymentsViewModel : ViewModelBase
    {
        private int _selectedTabIndex;
        private int _numbersOfTab = 1;
        private string _saveButtonName;
        private readonly IVendorDetailsBusinessLogic _vendorDetailsBusinessLogic;
        private readonly IVendorPaymentBusinessLogic _vendorPaymentBusinessLogic;
        private readonly PaymentsViewModel _paymentViewModel;
        private bool isGSTDetailsVisible;
        private VendorPaymentModel _vendorPaymentModel;
        private readonly IGstcalculationMasterBusinessLogic _gstcalculationMasterBusinessLogic;
        private readonly IVendorBusinessLogic _vendorBusinessLogic;
        private readonly IVenderPaymentNotesBusinessLogic _venderPaymentNotesBusinessLogic;
        public bool IsGSTDetailsVisible
        {
            get { return isGSTDetailsVisible; }
            set
            {
                if (isGSTDetailsVisible != value)
                {
                    isGSTDetailsVisible = value;
                    OnPropertyChanged(nameof(GSTTabVisible));
                    OnPropertyChanged(nameof(TextBoxGSTCategoryVisibility));
                    OnPropertyChanged(nameof(ComboBoxGSTCategoryVisibility));

                    VendorPaymentIsGst = true;
                }
            }
        }

        private bool isTDSTextBoxVisible;

        public bool IsTDSTextBoxVisible
        {
            get { return isTDSTextBoxVisible; }
            set
            {
                if (isTDSTextBoxVisible != value)
                {
                    isTDSTextBoxVisible = value;
                    OnPropertyChanged(nameof(TextBoxVisibility));
                    OnPropertyChanged(nameof(TextBlockVisibility));

                }
            }
        }

        private bool isBranchNameVisible;

        public bool IsBranchNameVisible
        {
            get { return isBranchNameVisible; }
            set
            {

                if (isBranchNameVisible != value)
                {
                    isBranchNameVisible = value;
                    OnPropertyChanged(nameof(TextBoxBranchNameVisibility));
                    OnPropertyChanged(nameof(TextBlockBranchNameVisibility));

                }
            }
        }

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
        public Visibility GSTTabVisible
        {
            get { return IsGSTDetailsVisible ? Visibility.Visible : Visibility.Collapsed; }
        }
        public Visibility TextBoxVisibility
        {
            get { return IsTDSTextBoxVisible ? Visibility.Visible : Visibility.Collapsed; }
        }
        public Visibility TextBlockVisibility
        {
            get { return IsTDSTextBoxVisible ? Visibility.Visible : Visibility.Collapsed; }
        }

        public Visibility TextBoxBranchNameVisibility
        {
            get { return IsBranchNameVisible ? Visibility.Visible : Visibility.Collapsed; }
        }
        public Visibility TextBlockBranchNameVisibility
        {
            get { return IsBranchNameVisible ? Visibility.Visible : Visibility.Collapsed; }
        }

        public Visibility TextBoxGSTCategoryVisibility
        {
            get { return IsGSTDetailsVisible ? Visibility.Visible : Visibility.Collapsed; }
        }

        public Visibility ComboBoxGSTCategoryVisibility
        {
            get { return IsGSTDetailsVisible ? Visibility.Visible : Visibility.Collapsed; }
        }

        #region Command
        public ICommand BackCommand { get; set; }
        public ICommand NextCommand { get; set; }
        public ICommand HidePaymentFormCommand { get; }
        public ICommand SubmitCommand { get; }
        public ICommand ClearFormCommand { get; }

        #endregion

        #region Observable collections

        private ObservableCollection<SearchModel> _comboxPaymentMethod;



        public ObservableCollection<SearchModel> ComboxPaymentMethods
        {
            get { return _comboxPaymentMethod; }
            set { _comboxPaymentMethod = value; }
        }

        #endregion

        #region Observable collections for Combo box

        private ObservableCollection<VendorDetailModel> _VendorServiceDetails;

        public ObservableCollection<VendorDetailModel> VendorServiceDetails
        {
            get { return _VendorServiceDetails; }
            set
            {
                _VendorServiceDetails = value;
                OnPropertyChanged(nameof(VendorServiceDetails));
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
        #endregion

        #region Properties

        #region TextBox Properties


        private string _TextBoxServiceName;
        public string TextBoxServiceName
        {
            get { return _TextBoxServiceName; }
            set
            {
                _TextBoxServiceName = value;
                OnPropertyChanged(nameof(TextBoxServiceName));
            }
        }

        private string _TextBoxPaymentCodeName;
        public string TextBoxPaymentCodeName
        {
            get { return _TextBoxPaymentCodeName; }
            set
            {
                _TextBoxPaymentCodeName = value;
                OnPropertyChanged(nameof(TextBoxPaymentCodeName));
            }
        }

        private VendorModel _SelectedVendorModel;
        public VendorModel SelectedVendorModel
        {
            get { return _SelectedVendorModel; }
            set
            {
                _SelectedVendorModel = value;

                if (SelectedVendorModel != null)
                {
                    OnPropertyChanged(nameof(SelectedVendorModel));
                    _ = LoadVendorServiceDetails(SelectedVendorModel.VendorId);
                    PaymentNoteNo = "";
                }
            }
        }

        private VendorDetailModel _SelectedVendorDetailService;

        public VendorDetailModel SelectedVendorDetailService
        {
            get { return _SelectedVendorDetailService; }
            set
            {
                _SelectedVendorDetailService = value;
                if (SelectedVendorDetailService != null)
                {
                    OnPropertyChanged(nameof(SelectedVendorDetailService));

                    LoadVendorPaymentNotes(SelectedVendorDetailService.VendorId);
                    // PaymentNoteNo= SelectedVendorDetailService.notr
                }
            }
        }

        #endregion

        //private VendorDetailModel _selectedVendorServiceDetails;
        private string _paymentCode;

        private string? _vendorPaymentYear;
        private DateTime? _vendorPaymentDate;
        private string? _VendorPaymentAmount;
        private bool? _vendorPaymentIsGst;
        private decimal? _vendorPaymentCgst;
        private decimal? _vendorPaymentSgst;
        private int? _vendorPaymentTotalAmountPaid;
        private string? _vendorPaymentUtrnumber;
        private decimal? _vendorPaymentRtgsAmount;
        private DateOnly? _vendorPaymentRtgsDate;
        private bool? _vendorPaymentIsTdsapplicable;
        private bool? _isPaymentForBranch;
        private decimal? _vendorPaymentTdsamount;
        private string? _vendorPaymentNotesDetails;
        private string? _bankBranchName;

        private string _paymentNoteNo;

        public string PaymentNoteNo
        {
            get { return _paymentNoteNo; }
            set
            {
                _paymentNoteNo = value;
                OnPropertyChanged(nameof(PaymentNoteNo));
            }
        }


        //public VendorDetailModel? SelectedVendorServiceDetails
        //{
        //    get { return _selectedVendorServiceDetails; }
        //    set
        //    {

        //        _selectedVendorServiceDetails = value;
        //        OnPropertyChanged(nameof(SelectedVendorServiceDetails));
        //        GeneratePaymentCode(_selectedVendorServiceDetails);

        //    }
        //}
        private int _Cgstpercentage;

        public int Cgstpercentage
        {
            get { return _Cgstpercentage; }
            set
            {
                _Cgstpercentage = value;
                OnPropertyChanged(nameof(Cgstpercentage));
            }
        }

        private int _Sgstpercentage;

        public int Sgstpercentage
        {
            get { return _Sgstpercentage; }
            set
            {
                _Sgstpercentage = value;
                OnPropertyChanged(nameof(Sgstpercentage));
            }
        }

        private int _Igstpercentage;

        public int Igstpercentage
        {
            get { return _Igstpercentage; }
            set
            {
                _Igstpercentage = value;
                OnPropertyChanged(nameof(Igstpercentage));
            }
        }

        public string PaymentCode
        {
            get { return _paymentCode; }
            set
            {
                _paymentCode = value;
                OnPropertyChanged(nameof(PaymentCode));
            }
        }
        public string? VendorPaymentYear
        {
            get { return _vendorPaymentYear; }
            set
            {
                _vendorPaymentYear = value;
                OnPropertyChanged(nameof(VendorPaymentYear));
            }
        }
        public DateTime? VendorPaymentDate
        {
            get { return _vendorPaymentDate; }
            set
            {
                _vendorPaymentDate = value;
                OnPropertyChanged(nameof(VendorPaymentDate));
            }
        }
        public string? VendorPaymentAmount
        {
            get { return _VendorPaymentAmount; }
            set
            {
                _VendorPaymentAmount = value;
                OnPropertyChanged(nameof(VendorPaymentAmount));
            }
        }
        public bool? VendorPaymentIsGst
        {
            get { return _vendorPaymentIsGst; }
            set
            {
                _vendorPaymentIsGst = value;
                OnPropertyChanged(nameof(VendorPaymentIsGst));
            }
        }
        public decimal? VendorPaymentCgst
        {
            get { return _vendorPaymentCgst; }
            set
            {
                _vendorPaymentCgst = value;
                OnPropertyChanged(nameof(VendorPaymentCgst));
            }
        }
        public decimal? VendorPaymentSgst
        {
            get { return _vendorPaymentSgst; }
            set
            {
                _vendorPaymentSgst = value;
                OnPropertyChanged(nameof(VendorPaymentSgst));
            }
        }
        public int? VendorPaymentTotalAmountPaid
        {
            get { return _vendorPaymentTotalAmountPaid; }
            set
            {
                _vendorPaymentTotalAmountPaid = value;
                OnPropertyChanged(nameof(VendorPaymentTotalAmountPaid));
            }
        }
        public string? VendorPaymentUtrnumber
        {
            get { return _vendorPaymentUtrnumber; }
            set
            {
                _vendorPaymentUtrnumber = value;
                OnPropertyChanged(nameof(VendorPaymentUtrnumber));
            }
        }
        public decimal? VendorPaymentRtgsAmount
        {
            get { return _vendorPaymentRtgsAmount; }
            set
            {
                _vendorPaymentRtgsAmount = value;
                OnPropertyChanged(nameof(VendorPaymentRtgsAmount));
            }
        }
        public DateOnly? VendorPaymentRtgsDate
        {
            get { return _vendorPaymentRtgsDate; }
            set
            {
                _vendorPaymentRtgsDate = value;
                OnPropertyChanged(nameof(VendorPaymentRtgsDate));
            }
        }
        public bool? VendorPaymentIsTdsapplicable
        {
            get { return _vendorPaymentIsTdsapplicable; }
            set
            {
                _vendorPaymentIsTdsapplicable = value;
                OnPropertyChanged(nameof(VendorPaymentIsTdsapplicable));
            }
        }
        public bool? IsPaymentForBranch
        {
            get { return _isPaymentForBranch; }
            set
            {
                _isPaymentForBranch = value;
                OnPropertyChanged(nameof(IsPaymentForBranch));
            }
        }
        public decimal? VendorPaymentTdsamount
        {
            get { return _vendorPaymentTdsamount; }
            set
            {
                _vendorPaymentTdsamount = value;
                OnPropertyChanged(nameof(VendorPaymentTdsamount));
            }
        }
        public string? VendorPaymentNotesDetails
        {
            get { return _vendorPaymentNotesDetails; }
            set
            {
                _vendorPaymentNotesDetails = value;
                OnPropertyChanged(nameof(VendorPaymentNotesDetails));
            }
        }
        public string? BankBranchName
        {
            get { return _bankBranchName; }
            set
            {
                _bankBranchName = value;
                OnPropertyChanged(nameof(BankBranchName));
            }
        }


        #endregion

        public AddPaymentsViewModel(PaymentsViewModel vendorViewModel, IVendorDetailsBusinessLogic vendorDetailsBusinessLogic, VendorPaymentModel vendorPaymentModel, IVendorPaymentBusinessLogic vendorPaymentBusinessLogic, IGstcalculationMasterBusinessLogic gstcalculationMasterBusinessLogic, IVendorBusinessLogic vendorBusinessLogic, IVenderPaymentNotesBusinessLogic venderPaymentNotesBusinessLogic)
        {
            _vendorPaymentModel = vendorPaymentModel;
            if (_vendorPaymentModel != null)
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
            _vendorDetailsBusinessLogic = vendorDetailsBusinessLogic;
            _vendorPaymentBusinessLogic = vendorPaymentBusinessLogic;
            _vendorBusinessLogic = vendorBusinessLogic;
            _venderPaymentNotesBusinessLogic = venderPaymentNotesBusinessLogic;
            _paymentViewModel = vendorViewModel;
            HidePaymentFormCommand = new ViewModelAsyncCommand<VendorPaymentModel>(HidePaymentForm);
            SubmitCommand = new ViewModelAsyncCommand<VendorPaymentModel>(SubmitPaymentDetails, ValidatePAymentDetails);
            ClearFormCommand = new ViewModelAsyncCommand<VendorPaymentModel>(ClearPaymentForm);
            _gstcalculationMasterBusinessLogic = gstcalculationMasterBusinessLogic;
            CallAync();
        }
        private async Task PopulateValues()
        {
            if (_vendorPaymentModel != null)
            {
                ///need to code to set no if resceive false
                ///need to add binding for no radio button
                PaymentCode = _vendorPaymentModel?.PaymentCode ?? "";
                VendorPaymentYear = _vendorPaymentModel?.PaymentYear;
                VendorPaymentDate = _vendorPaymentModel?.VendorPaymentDate;
                VendorPaymentAmount = _vendorPaymentModel?.VendorPaymentAmount;
                IsGSTDetailsVisible = _vendorPaymentModel?.VendorPaymentIsGst != null ? (bool)_vendorPaymentModel.VendorPaymentIsGst : false;
                VendorPaymentCgst = _vendorPaymentModel?.VendorPaymentCgst;
                VendorPaymentSgst = _vendorPaymentModel?.VendorPaymentSgst;
                VendorPaymentTotalAmountPaid = _vendorPaymentModel?.VendorPaymentTotalAmountPaid;
                VendorPaymentUtrnumber = _vendorPaymentModel?.VendorPaymentUtrnumber;
                VendorPaymentRtgsAmount = _vendorPaymentModel?.VendorPaymentRtgsAmount;
                VendorPaymentRtgsDate = _vendorPaymentModel?.VendorPaymentRtgsDate;
                IsTDSTextBoxVisible = _vendorPaymentModel?.VendorPaymentIsTdsapplicable != null ? (bool)_vendorPaymentModel.VendorPaymentIsTdsapplicable : false;
                IsBranchNameVisible = _vendorPaymentModel?.IsPaymentForBranch != null ? (bool)_vendorPaymentModel.IsPaymentForBranch : false;
                VendorPaymentTdsamount = _vendorPaymentModel?.VendorPaymentTdsamount;
                VendorPaymentNotesDetails = _vendorPaymentModel?.Notes;
                BankBranchName = _vendorPaymentModel?.BankBranchName;

                //var vendorID = VendorServiceDetails.ToList().Find(x => x.VendorDetailId == _vendorPaymentModel?.FkVendorDetailId);

                //if (vendorID != null)
                //{
                //    SelectedVendorServiceDetails = VendorServiceDetails[VendorServiceDetails.IndexOf(vendorID)];
                //}
            }

        }
        private async void GeneratePaymentCode(VendorDetailModel? vendorDetailModel)
        {
            var paymentCode = await _vendorPaymentBusinessLogic.GeneratePaymentCode(vendorDetailModel);
            PaymentCode = paymentCode;
        }

        private async Task ClearPaymentForm(VendorPaymentModel model)
        {
            BankBranchName = "";
            IsPaymentForBranch = false;
            PaymentCode = "";
            VendorPaymentAmount = "";
            VendorPaymentCgst = 0;
            VendorPaymentDate = DateTime.MaxValue;
            VendorPaymentIsGst = false;
            VendorPaymentNotesDetails = "";
            VendorPaymentIsTdsapplicable = false;
            VendorPaymentRtgsDate = DateOnly.MinValue;
            VendorPaymentSgst = 0;
            VendorPaymentTdsamount = 0;
            VendorPaymentRtgsAmount = 0;
            VendorPaymentUtrnumber = "";
            VendorPaymentTotalAmountPaid = 0;
            VendorPaymentYear = "";
            IsGSTDetailsVisible = false;
            IsBranchNameVisible = false;
            IsTDSTextBoxVisible = false;
        }

        private bool ValidatePAymentDetails()
        {
            return true;
        }

        private async Task SubmitPaymentDetails(VendorPaymentModel model)
        {
            if (SaveButtonName == "Update")
            {
                VendorPaymentModel payment = new VendorPaymentModel()
                {
                    BankBranchName = BankBranchName,
                    IsPaymentForBranch = IsBranchNameVisible,
                    PaymentCode = PaymentCode,
                    VendorPaymentAmount = VendorPaymentAmount ?? "",
                    VendorPaymentCgst = VendorPaymentCgst,
                    VendorPaymentDate = Convert.ToDateTime(VendorPaymentDate),
                    VendorPaymentIsGst = IsGSTDetailsVisible,
                    Notes = VendorPaymentNotesDetails,
                    VendorPaymentIsTdsapplicable = IsTDSTextBoxVisible,
                    VendorPaymentRtgsDate = VendorPaymentRtgsDate,
                    VendorPaymentSgst = VendorPaymentSgst,
                    VendorPaymentTdsamount = VendorPaymentTdsamount,

                    VendorPaymentRtgsAmount = VendorPaymentRtgsAmount,
                    VendorPaymentUtrnumber = VendorPaymentUtrnumber,

                    VendorPaymentTotalAmountPaid = VendorPaymentTotalAmountPaid,
                    PaymentYear = VendorPaymentYear,

                    // FkVendorDetailId = SelectedVendorServiceDetails.VendorDetailId,
                    LastUpdateBy = UserAccountModel.Username,
                    VendorPaymentId = _vendorPaymentModel.VendorPaymentId,
                    IsActive = true
                };
                await _vendorPaymentBusinessLogic.EditUpdateVendorPayment(payment);

                SuccessPopupViewModel.Instance.ShowPopup(Enums.NotificationType.Success, "Data Updated Successfully");
            }
            else
            {
                VendorPaymentModel payment = new()
                {
                    BankBranchName = BankBranchName,
                    IsPaymentForBranch = IsBranchNameVisible,
                    PaymentCode = PaymentCode,
                    VendorPaymentAmount = VendorPaymentAmount,
                    VendorPaymentCgst = VendorPaymentCgst,
                    VendorPaymentDate = Convert.ToDateTime(VendorPaymentDate),
                    VendorPaymentIsGst = IsGSTDetailsVisible,
                    Notes = VendorPaymentNotesDetails,
                    VendorPaymentIsTdsapplicable = IsTDSTextBoxVisible,
                    VendorPaymentRtgsDate = VendorPaymentRtgsDate,
                    VendorPaymentSgst = VendorPaymentSgst,
                    VendorPaymentTdsamount = VendorPaymentTdsamount,

                    VendorPaymentRtgsAmount = VendorPaymentRtgsAmount,
                    VendorPaymentUtrnumber = VendorPaymentUtrnumber,

                    VendorPaymentTotalAmountPaid = VendorPaymentTotalAmountPaid,
                    PaymentYear = VendorPaymentYear,

                    CreatedBy = UserAccountModel.Username,
                    IsActive = true
                };
                await _vendorPaymentBusinessLogic.AddVendorPayment(payment);


                SuccessPopupViewModel.Instance.ShowPopup(Enums.NotificationType.Success, "Data Added Successfully");
            }

            await HidePaymentForm(this);
        }

        public async Task HidePaymentForm(object model)
        {
            await _paymentViewModel.HidePaymentForm(this).ConfigureAwait(true);
        }

        private async void CallAync()
        {
            await MainTask();
        }
        public async Task MainTask()
        {
            // await LoadVendorServiceDetails();
            await LoadVendors();
            await PopulateValues();
            await LoadGSTDetails();
        }
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


        ///// <summary>
        ///// Combobox load item with Vendor Details 
        ///// </summary>
        ///// <returns></returns>
        //private async Task LoadVendorServiceDetails()
        //{
        //    var vendorServiceDetails = await _vendorDetailsBusinessLogic.GetAllVendorDetails().ConfigureAwait(true);
        //    VendorServiceDetails = new ObservableCollection<VendorDetailModel>(vendorServiceDetails);
        //}

        private async Task LoadGSTDetails()
        {
            var gstDetails = await _gstcalculationMasterBusinessLogic.GetAllGstMaster().ConfigureAwait(true);
            Cgstpercentage = Convert.ToInt32(gstDetails?.FirstOrDefault()?.CgstPercentage);
            Sgstpercentage = Convert.ToInt32(gstDetails?.FirstOrDefault()?.SgstPercentage);
            Igstpercentage = Convert.ToInt32(gstDetails?.FirstOrDefault()?.IgstPercentage);

        }


        #region Combobox load vendors and services on combo box selection 

        private async Task LoadVendorPaymentNotes(int vendorId)
        {
            var vendorServiceDetails = await _venderPaymentNotesBusinessLogic.GetPaymentNoteByVendorId(vendorId).ConfigureAwait(true);
            // VendorServiceDetails = new ObservableCollection<VendorDetailModel>(vendorServiceDetails.Where(x => x.VendorId == vendorId));
            PaymentNoteNo = vendorServiceDetails.PaymentNoteNo;
        }

        /// <summary>
        /// Combobox load Vendor Service Name on selection of Vendor
        /// </summary>
        /// <returns></returns>
        private async Task LoadVendorServiceDetails(int vendorId)
        {
            var vendorServiceDetails = await _vendorDetailsBusinessLogic.GetAllVendorDetails().ConfigureAwait(true);
            VendorServiceDetails = new ObservableCollection<VendorDetailModel>(vendorServiceDetails.Where(x => x.VendorId == vendorId));
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

        #region Vendor and Service Combo box

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
