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
        public bool IsGSTDetailsVisible
        {
            get { return isGSTDetailsVisible; }
            set
            {
                if (isGSTDetailsVisible != value)
                {
                    isGSTDetailsVisible = value;
                    OnPropertyChanged(nameof(GSTTabVisible));
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

        #region Command
        public ICommand BackCommand { get; set; }
        public ICommand NextCommand { get; set; }
        public ICommand HidePaymentFormCommand { get; }
        public ICommand SubmitCommand { get; }
        public ICommand ClearFormCommand { get; }

        #endregion

        #region Observable collections
        private ObservableCollection<VendorDetailModel> _vendorDetails;
        private ObservableCollection<SearchModel> _comboxPaymentMethod;

        public ObservableCollection<VendorDetailModel> VendorServiceDetails
        {
            get { return _vendorDetails; }
            set
            {
                _vendorDetails = value;
                OnPropertyChanged(nameof(VendorServiceDetails));
            }
        }

        public ObservableCollection<SearchModel> ComboxPaymentMethods
        {
            get { return _comboxPaymentMethod; }
            set { _comboxPaymentMethod = value; }
        }

        #endregion

        #region Properties
        private VendorDetailModel _selectedVendorServiceDetails;
        private string _paymentCode;

        private string? _vendorPaymentYear;
        private DateTime? _vendorPaymentDate;
        private string? _VendorPaymentAmount;
        private bool? _vendorPaymentIsGst;
        private decimal? _vendorPaymentCgst;
        private decimal? _vendorPaymentSgst;
        private int? _vendorPaymentTotalAmountPaid;
        private int? _vendorPaymentUtrnumber;
        private decimal? _vendorPaymentRtgsAmount;
        private DateOnly? _vendorPaymentRtgsDate;
        private bool? _vendorPaymentIsTdsapplicable;
        private bool? _isPaymentForBranch;
        private decimal? _vendorPaymentTdsamount;
        private string? _vendorPaymentNotesDetails;
        private string? _bankBranchName;

        public VendorDetailModel? SelectedVendorServiceDetails
        {
            get { return _selectedVendorServiceDetails; }
            set
            {

                _selectedVendorServiceDetails = value;
                OnPropertyChanged(nameof(SelectedVendorServiceDetails));
                GeneratePaymentCode(_selectedVendorServiceDetails);

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
        public int? VendorPaymentUtrnumber
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

        public AddPaymentsViewModel(PaymentsViewModel vendorViewModel, IVendorDetailsBusinessLogic vendorDetailsBusinessLogic, VendorPaymentModel vendorPaymentModel, IVendorPaymentBusinessLogic vendorPaymentBusinessLogic)
        {
            _vendorPaymentModel = vendorPaymentModel;
            if (_vendorPaymentModel != null)
            {
                SaveButtonName = "Update";
            }
            else
            {
                SaveButtonName = "Submit";
            }
            _vendorDetailsBusinessLogic = vendorDetailsBusinessLogic;
            _vendorPaymentBusinessLogic = vendorPaymentBusinessLogic;
            _paymentViewModel = vendorViewModel;
            HidePaymentFormCommand = new ViewModelAsyncCommand<VendorPaymentModel>(HidePaymentForm);
            SubmitCommand = new ViewModelAsyncCommand<VendorPaymentModel>(SubmitPaymentDetails, ValidatePAymentDetails);
            ClearFormCommand = new ViewModelAsyncCommand<VendorPaymentModel>(ClearPaymentForm);
            CallAync();
        }
        private async Task PopulateValues()
        {
            if (_vendorPaymentModel != null)
            {
                ///need to code to set no if resceive false
                ///need to add binding for no radio button
                PaymentCode = _vendorPaymentModel?.PaymentCode??"";
                VendorPaymentYear = _vendorPaymentModel?.VendorPaymentYear;
                VendorPaymentDate = _vendorPaymentModel?.VendorPaymentDate;
                VendorPaymentAmount = _vendorPaymentModel?.VendorPaymentAmount;
                IsGSTDetailsVisible = _vendorPaymentModel?.VendorPaymentIsGst!=null ? (bool)_vendorPaymentModel.VendorPaymentIsGst:false;
                VendorPaymentCgst = _vendorPaymentModel?.VendorPaymentCgst;
                VendorPaymentSgst = _vendorPaymentModel?.VendorPaymentSgst;
                VendorPaymentTotalAmountPaid = _vendorPaymentModel?.VendorPaymentTotalAmountPaid;
                VendorPaymentUtrnumber = _vendorPaymentModel?.VendorPaymentUtrnumber;
                VendorPaymentRtgsAmount = _vendorPaymentModel?.VendorPaymentRtgsAmount;
                VendorPaymentRtgsDate = _vendorPaymentModel?.VendorPaymentRtgsDate;
                IsTDSTextBoxVisible = _vendorPaymentModel?.VendorPaymentIsTdsapplicable != null ? (bool)_vendorPaymentModel.VendorPaymentIsTdsapplicable : false;
                IsBranchNameVisible = _vendorPaymentModel?.IsPaymentForBranch != null ? (bool)_vendorPaymentModel.IsPaymentForBranch : false;
                VendorPaymentTdsamount = _vendorPaymentModel?.VendorPaymentTdsamount;
                VendorPaymentNotesDetails = _vendorPaymentModel?.VendorPaymentNotesDetails;
                BankBranchName = _vendorPaymentModel?.BankBranchName;

                var vendorID = VendorServiceDetails.ToList().Find(x => x.VendorDetailId == _vendorPaymentModel?.FkVendorDetailId);

                if (vendorID != null)
                {
                    SelectedVendorServiceDetails = VendorServiceDetails[VendorServiceDetails.IndexOf(vendorID)];
                }
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
            VendorPaymentUtrnumber = 0;
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
                VendorPaymentModel payment = new()
                {
                    BankBranchName = BankBranchName,
                    IsPaymentForBranch = IsBranchNameVisible,
                    PaymentCode = PaymentCode,
                    VendorPaymentAmount = VendorPaymentAmount,
                    VendorPaymentCgst = VendorPaymentCgst,
                    VendorPaymentDate = VendorPaymentDate,
                    VendorPaymentIsGst = IsGSTDetailsVisible,
                    VendorPaymentNotesDetails = VendorPaymentNotesDetails,
                    VendorPaymentIsTdsapplicable = IsTDSTextBoxVisible,
                    VendorPaymentRtgsDate = VendorPaymentRtgsDate,
                    VendorPaymentSgst = VendorPaymentSgst,
                    VendorPaymentTdsamount = VendorPaymentTdsamount,
                    VendorServiceName = SelectedVendorServiceDetails?.VendorServiceName,
                    ServicePaymentType = SelectedVendorServiceDetails?.ServicePaymentType,
                    VendorPaymentRtgsAmount = VendorPaymentRtgsAmount,
                    VendorPaymentUtrnumber = VendorPaymentUtrnumber,
                    VendorServiceId = SelectedVendorServiceDetails.VendorServiceId,
                    VendorPaymentTotalAmountPaid = VendorPaymentTotalAmountPaid,
                    VendorPaymentYear = VendorPaymentYear,
                    ServiceSantionAmount = SelectedVendorServiceDetails?.ServiceSantionAmount,
                    FkVendorDetailId = SelectedVendorServiceDetails?.VendorDetailId,
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
                    VendorPaymentDate = VendorPaymentDate,
                    VendorPaymentIsGst = IsGSTDetailsVisible,
                    VendorPaymentNotesDetails = VendorPaymentNotesDetails,
                    VendorPaymentIsTdsapplicable = IsTDSTextBoxVisible,
                    VendorPaymentRtgsDate = VendorPaymentRtgsDate,
                    VendorPaymentSgst = VendorPaymentSgst,
                    VendorPaymentTdsamount = VendorPaymentTdsamount,
                    VendorServiceName = SelectedVendorServiceDetails?.VendorServiceName,
                    ServicePaymentType = SelectedVendorServiceDetails?.ServicePaymentType,
                    VendorPaymentRtgsAmount = VendorPaymentRtgsAmount,
                    VendorPaymentUtrnumber = VendorPaymentUtrnumber,
                    VendorServiceId = SelectedVendorServiceDetails.VendorServiceId,
                    VendorPaymentTotalAmountPaid = VendorPaymentTotalAmountPaid,
                    VendorPaymentYear = VendorPaymentYear,
                    ServiceSantionAmount = SelectedVendorServiceDetails?.ServiceSantionAmount,
                    FkVendorDetailId = SelectedVendorServiceDetails?.VendorDetailId,
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
            await LoadVendorServiceDetails();
            await PopulateValues();
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


        /// <summary>
        /// Combobox load item with Vendor Details 
        /// </summary>
        /// <returns></returns>
        private async Task LoadVendorServiceDetails()
        {
            var vendorServiceDetails = await _vendorDetailsBusinessLogic.GetAllVendorDetails().ConfigureAwait(true);
            VendorServiceDetails = new ObservableCollection<VendorDetailModel>(vendorServiceDetails);
        }

    }
}
