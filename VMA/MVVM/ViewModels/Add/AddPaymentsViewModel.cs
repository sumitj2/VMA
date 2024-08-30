using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.Abstraction.VMA.Models;
using BusinessLogic.VMA;
using Database.VMA.Entities;
using Database.VMA.Repositories;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Charts;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using VMA.Constants;
using VMA.MVVM.Models;
using VMA.MVVM.ViewModels.Menus;

namespace VMA.MVVM.ViewModels.Add
{
    public class AddPaymentsViewModel : ViewModelBase
    {
        private string _saveButtonName;
        private readonly IVendorDetailsBusinessLogic _vendorDetailsBusinessLogic;
        private readonly IVendorPaymentBusinessLogic _vendorPaymentBusinessLogic;
        private readonly PaymentsViewModel _paymentViewModel;

        private VendorPaymentModel _vendorPaymentModel;
        private readonly IGstcalculationMasterBusinessLogic _gstcalculationMasterBusinessLogic;
        private readonly IVendorBusinessLogic _vendorBusinessLogic;
        private readonly IVenderPaymentNotesBusinessLogic _venderPaymentNotesBusinessLogic;

        #region Radio buttons   

        //Yes
        private bool isGSTDetailsVisible;
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
                    OnPropertyChanged(nameof(IsGSTDetailsVisible));
                }
            }
        }

        //No
        private bool isGSTDetailsNotVisible;
        public bool IsGSTDetailsNotVisible
        {
            get { return isGSTDetailsNotVisible; }
            set
            {
                isGSTDetailsNotVisible = value;
                OnPropertyChanged(nameof(IsGSTDetailsNotVisible));
                SelectedGSTModel = null;
                VendorPaymentTotalAmountPaid = Convert.ToDecimal(0 + Convert.ToDouble(VendorPaymentAmount));
            }
        }

        //Yes
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
                    OnPropertyChanged(nameof(IsTDSTextBoxVisible));
                }
            }
        }

        //No
        private bool isTDSTextBoxNotVisible;
        public bool IsTDSTextBoxNotVisible
        {
            get { return isTDSTextBoxNotVisible; }
            set
            {
                if (isTDSTextBoxNotVisible != value)
                {
                    isTDSTextBoxNotVisible = value;
                    OnPropertyChanged(nameof(IsTDSTextBoxNotVisible));
                    OnPropertyChanged(nameof(IsTDSTextBoxNotVisible));
                }
            }
        }

        //Yes
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
                    OnPropertyChanged(nameof(IsBranchNameVisible));
                }
            }
        }

        //No
        private bool isBranchNameNotVisible;
        public bool IsBranchNameNotVisible
        {
            get { return isBranchNameNotVisible; }
            set
            {
                if (isBranchNameNotVisible != value)
                {
                    isBranchNameNotVisible = value;
                    OnPropertyChanged(nameof(IsBranchNameNotVisible));
                }
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

        private VenderPaymentNoteModel _paymentNoteDetails;
        public VenderPaymentNoteModel PaymentNoteDetails
        {
            get { return _paymentNoteDetails; }
            set
            {
                _paymentNoteDetails = value;
                OnPropertyChanged(nameof(PaymentNoteDetails));
            }
        }

        private ObservableCollection<GstcalculationMasterModel> _GSTDetails;
        public ObservableCollection<GstcalculationMasterModel> GSTDetails
        {
            get { return _GSTDetails; }
            set
            {
                _GSTDetails = value;
                OnPropertyChanged(nameof(GSTDetails));
            }
        }

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

        #region Combo box Value TextBox Properties


        private string _SelctedVendorServiceName;
        public string SelctedVendorServiceName
        {
            get { return _SelctedVendorServiceName; }
            set
            {
                _SelctedVendorServiceName = value;
                OnPropertyChanged(nameof(SelctedVendorServiceName));
            }
        }

        private string _SelectedVendorName;
        public string SelectedVendorName
        {
            get { return _SelectedVendorName; }
            set
            {
                _SelectedVendorName = value;
                OnPropertyChanged(nameof(SelectedVendorName));
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

        private GstcalculationMasterModel _SelectedGSTModel;
        public GstcalculationMasterModel SelectedGSTModel
        {
            get { return _SelectedGSTModel; }
            set
            {
                _SelectedGSTModel = value;
                OnPropertyChanged(nameof(SelectedGSTModel));
                CalculateGST();
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

                    _ = LoadVendorPaymentNotes(Convert.ToInt32(SelectedVendorDetailService.VendorId));
                    SuccessPopupViewModel.Instance.ShowPopup(Enums.NotificationType.Alert, "Please wait...", true);
                    _ = GetAmountToBepaid();
                }
            }
        }

        #endregion

        private string? _vendorPaymentYear;
        public string? VendorPaymentYear
        {
            get { return _vendorPaymentYear; }
            set
            {
                _vendorPaymentYear = value;
                OnPropertyChanged(nameof(VendorPaymentYear));
            }
        }

        private string? _paymentNoteNo;
        public string? PaymentNoteNo
        {
            get { return _paymentNoteNo; }
            set
            {
                _paymentNoteNo = value;
                OnPropertyChanged(nameof(PaymentNoteNo));
            }
        }

        private string? _vendorPaymentNotesDetails;
        public string? VendorPaymentNotesDetails
        {
            get { return _vendorPaymentNotesDetails; }
            set
            {
                _vendorPaymentNotesDetails = value;
                OnPropertyChanged(nameof(VendorPaymentNotesDetails));
            }
        }

        private DateOnly? _vendorPaymentDate;
        public DateOnly? VendorPaymentDate
        {
            get { return _vendorPaymentDate; }
            set
            {
                _vendorPaymentDate = value;
                OnPropertyChanged(nameof(VendorPaymentDate));
            }
        }
        private bool _EnableTotalPaidAmt;

        public bool EnableTotalPaidAmt
        {
            get { return _EnableTotalPaidAmt; }
            set
            {
                _EnableTotalPaidAmt = value;
                OnPropertyChanged(nameof(EnableTotalPaidAmt));
            }
        }


        //Non Taxable
        private decimal? _VendorPaymentAmount;
        public decimal? VendorPaymentAmount
        {
            get { return _VendorPaymentAmount; }
            set
            {
                _VendorPaymentAmount = value;
                OnPropertyChanged(nameof(VendorPaymentAmount));
            }
        }

        //With Tax
        private decimal? _vendorPaymentTotalAmountPaid;
        public decimal? VendorPaymentTotalAmountPaid
        {
            get { return _vendorPaymentTotalAmountPaid; }
            set
            {
                _vendorPaymentTotalAmountPaid = value;
                OnPropertyChanged(nameof(VendorPaymentTotalAmountPaid));
            }
        }

        private string? _bankBranchName;
        public string? BankBranchName
        {
            get { return _bankBranchName; }
            set
            {
                _bankBranchName = value;
                OnPropertyChanged(nameof(BankBranchName));
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

        #region GST Tab

        //CGST
        private decimal? _vendorPaymentCgst;
        public decimal? VendorPaymentCgst
        {
            get { return _vendorPaymentCgst; }
            set
            {
                _vendorPaymentCgst = value;
                OnPropertyChanged(nameof(VendorPaymentCgst));
            }
        }

        //SGST
        private decimal? _vendorPaymentSgst;
        public decimal? VendorPaymentSgst
        {
            get { return _vendorPaymentSgst; }
            set
            {
                _vendorPaymentSgst = value;
                OnPropertyChanged(nameof(VendorPaymentSgst));
            }
        }

        //IGST
        private decimal? _vendorPaymentIgst;
        public decimal? VendorPaymentIgst
        {
            get { return _vendorPaymentIgst; }
            set
            {
                _vendorPaymentIgst = value;
                OnPropertyChanged(nameof(VendorPaymentIgst));
            }
        }

        //GST Total
        private double? _GSTTotal;
        public double? GSTTotal
        {
            get { return _GSTTotal; }
            set
            {
                _GSTTotal = (double?)(VendorPaymentCgst + VendorPaymentSgst + VendorPaymentIgst);
                OnPropertyChanged(nameof(GSTTotal));
            }
        }

        #region GST_Tab % from Combo Box selection

        private int? _Cgstpercentage;

        public int? Cgstpercentage
        {
            get { return _Cgstpercentage; }
            set
            {
                _Cgstpercentage = value;
                OnPropertyChanged(nameof(Cgstpercentage));
            }
        }

        private int? _Sgstpercentage;

        public int? Sgstpercentage
        {
            get { return _Sgstpercentage; }
            set
            {
                _Sgstpercentage = value;
                OnPropertyChanged(nameof(Sgstpercentage));
            }
        }

        private int? _Igstpercentage;

        public int? Igstpercentage
        {
            get { return _Igstpercentage; }
            set
            {
                _Igstpercentage = value;
                OnPropertyChanged(nameof(Igstpercentage));
            }
        }
        #endregion

        #endregion

        #region Invoice Tab

        private string? _InvoiceNumber;
        public string? InvoiceNumber
        {
            get
            {
                return _InvoiceNumber;
            }
            set
            {
                _InvoiceNumber = value;
                OnPropertyChanged(nameof(InvoiceNumber));
            }
        }

        private DateTime? _InvoiceDate;
        public DateTime? InvoiceDate
        {
            get
            {
                return _InvoiceDate;
            }
            set
            {
                _InvoiceDate = value;
                OnPropertyChanged(nameof(InvoiceDate));
            }
        }

        private string? _InvoiceParticulars;
        public string? InvoiceParticulars
        {
            get
            {
                return _InvoiceParticulars;
            }
            set
            {
                _InvoiceParticulars = value;
                OnPropertyChanged(nameof(InvoiceParticulars));
            }
        }

        #endregion

        #region RTGS Details

        private DateOnly? _vendorPaymentRtgsDate;
        public DateOnly? VendorPaymentRtgsDate
        {
            get { return _vendorPaymentRtgsDate; }
            set
            {
                _vendorPaymentRtgsDate = value;
                OnPropertyChanged(nameof(VendorPaymentRtgsDate));
            }
        }

        private decimal? _vendorPaymentRtgsAmount;
        public decimal? VendorPaymentRtgsAmount
        {
            get { return _vendorPaymentRtgsAmount; }
            set
            {
                _vendorPaymentRtgsAmount = value;
                OnPropertyChanged(nameof(VendorPaymentRtgsAmount));
            }
        }

        private string? _vendorPaymentUtrnumber;
        public string? VendorPaymentUtrnumber
        {
            get { return _vendorPaymentUtrnumber; }
            set
            {
                _vendorPaymentUtrnumber = value;
                OnPropertyChanged(nameof(VendorPaymentUtrnumber));
            }
        }

        private string? _vendorPaymentTdsamountNew;

        [RegularExpression("^[0-9.]+$")]
        public string? VendorPaymentTdsamountNew
        {
            get { return _vendorPaymentTdsamountNew; }
            set
            {
                _vendorPaymentTdsamountNew = value;
                OnPropertyChanged(nameof(VendorPaymentTdsamountNew));
            }
        }

        #endregion


        #endregion
        private readonly IConfigurationBusinessLogic _configurationBusinessLogic;
        public AddPaymentsViewModel(PaymentsViewModel vendorViewModel, IVendorDetailsBusinessLogic vendorDetailsBusinessLogic, VendorPaymentModel vendorPaymentModel, IVendorPaymentBusinessLogic vendorPaymentBusinessLogic, IGstcalculationMasterBusinessLogic gstcalculationMasterBusinessLogic, IVendorBusinessLogic vendorBusinessLogic, IVenderPaymentNotesBusinessLogic venderPaymentNotesBusinessLogic, IConfigurationBusinessLogic configurationBusinessLogic)
        {
            Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Into the constructor", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));

            _configurationBusinessLogic = configurationBusinessLogic;
            _vendorPaymentModel = vendorPaymentModel;
            if (_vendorPaymentModel != null)
            {
                IsComboBoxVendorVisible = false;
                IsComboBoxServiceVisible = false;
                IsTextBoxSelectedVendorVisible = true;
                IsTextBoxServiceVisible = true;
                SaveButtonName = GeneralConstants.Update;
            }
            else
            {
                IsComboBoxVendorVisible = true;
                IsComboBoxServiceVisible = true;
                IsTextBoxSelectedVendorVisible = false;
                IsTextBoxServiceVisible = false;
                SaveButtonName = GeneralConstants.Submit;
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
            _configurationBusinessLogic = configurationBusinessLogic;
            _ = CallAync();
        }

        public async Task GetAllConfigurations()
        {
            var allConfigurations = await _configurationBusinessLogic.GetConfigurations().ConfigureAwait(true);

            string? financialYear = allConfigurations.FirstOrDefault(x => x.Cfgkey == GeneralConstants.CFGKeyFinacialYear)?.CfgValue;

            VendorPaymentYear = financialYear;
        }

        private async Task PopulateValues()
        {
            if (_vendorPaymentModel != null)
            {
                VendorPaymentYear = _vendorPaymentModel?.PaymentYear;
                SelectedVendorName = _vendorPaymentModel?.VendorName ?? "";
                SelctedVendorServiceName = _vendorPaymentModel?.VendorServiceName ?? "";
                VendorPaymentDate = _vendorPaymentModel?.VendorPaymentDate;
                VendorPaymentAmount = _vendorPaymentModel?.VendorPaymentAmount;

                IsGSTDetailsVisible = _vendorPaymentModel?.VendorPaymentIsGst != null ? (bool)_vendorPaymentModel.VendorPaymentIsGst : IsGSTDetailsNotVisible;
                IsGSTDetailsNotVisible = !IsGSTDetailsVisible ? true : false;
                VendorPaymentCgst = _vendorPaymentModel?.VendorPaymentCgst;
                VendorPaymentSgst = _vendorPaymentModel?.VendorPaymentSgst;
                VendorPaymentIgst = _vendorPaymentModel?.VendorPaymentIgst;
                GSTTotal = (double?)(VendorPaymentCgst + VendorPaymentSgst + VendorPaymentIgst);
                VendorPaymentTotalAmountPaid = _vendorPaymentModel?.VendorPaymentTotalAmountPaid;

                IsTDSTextBoxVisible = _vendorPaymentModel?.VendorPaymentIsTdsapplicable != null ? (bool)_vendorPaymentModel.VendorPaymentIsTdsapplicable : isTDSTextBoxNotVisible;
                IsTDSTextBoxNotVisible = !IsTDSTextBoxVisible ? true : false;
                IsBranchNameVisible = _vendorPaymentModel?.IsPaymentForBranch != null ? (bool)_vendorPaymentModel.IsPaymentForBranch : isBranchNameNotVisible;
                IsBranchNameNotVisible = !IsBranchNameVisible ? true : false;
                VendorPaymentNotesDetails = _vendorPaymentModel?.Notes;
                BankBranchName = _vendorPaymentModel?.BankBranchName;

                VendorPaymentUtrnumber = _vendorPaymentModel?.VendorPaymentUtrnumber;
                VendorPaymentRtgsAmount = _vendorPaymentModel?.VendorPaymentRtgsAmount;
                VendorPaymentRtgsDate = _vendorPaymentModel?.VendorPaymentRtgsDate;
                if (_vendorPaymentModel?.VendorPaymentTdsamount.ToString() == "0")
                {
                    VendorPaymentTdsamountNew = null;
                }
                else
                {
                    VendorPaymentTdsamountNew = _vendorPaymentModel?.VendorPaymentTdsamount.ToString();
                }
                InvoiceNumber = _vendorPaymentModel?.InvoiceNumber;
                InvoiceParticulars = _vendorPaymentModel?.InvoiceParticulars;
                InvoiceDate = _vendorPaymentModel?.InvoiceDate;

                PaymentNoteNo = _vendorPaymentModel?.PaymentNoteNo;

                var gstSrNo = GSTDetails.ToList().Find(x => x.SrNo == _vendorPaymentModel?.FkGstmasterSrNo);

                if (gstSrNo != null)
                {
                    SelectedGSTModel = GSTDetails[GSTDetails.IndexOf(gstSrNo)];
                }
            }
        }

        private async Task ClearPaymentForm(VendorPaymentModel model)
        {
            BankBranchName = "";
            //IsPaymentForBranch = false;
            //PaymentCode = "";
            VendorPaymentAmount = 0;
            VendorPaymentCgst = 0;
            VendorPaymentDate = DateOnly.MinValue;
            //VendorPaymentIsGst = false;
            VendorPaymentNotesDetails = "";
            // VendorPaymentIsTdsapplicable = false;
            VendorPaymentRtgsDate = DateOnly.MinValue;
            VendorPaymentSgst = 0;
            VendorPaymentTdsamountNew = "";
            VendorPaymentRtgsAmount = 0;
            VendorPaymentUtrnumber = "";
            VendorPaymentTotalAmountPaid = 0;
            IsGSTDetailsVisible = false;
            IsBranchNameVisible = false;
            IsTDSTextBoxVisible = false;
        }
        string errorMsg = "";
        private bool ValidatePAymentDetails()
        {
            bool validData;

            if (SelectedVendorModel == null)
            {
                errorMsg += nameof(SelectedVendorModel);
                validData = false;
            }
            else
            {
                validData = true;
            }

            if (SelectedVendorDetailService == null)
            {
                errorMsg += ", " + nameof(SelectedVendorDetailService);
                validData = false;
            }
            else
            {
                validData = true;
            }

            if (IsGSTDetailsVisible == true)
            {
                if (SelectedGSTModel == null)
                {
                    errorMsg += ", " + nameof(SelectedGSTModel);
                    validData = false;
                }
                else
                {
                    validData = true;
                }
            }

            if (IsBranchNameVisible == true)
            {
                if (BankBranchName == null || BankBranchName == "" || string.IsNullOrEmpty(BankBranchName) || string.IsNullOrWhiteSpace(BankBranchName))
                {
                    errorMsg += " ," + nameof(BankBranchName);
                    validData = false;
                }
                else
                {
                    validData = true;
                }
            }

            if (IsTDSTextBoxVisible)
            {
                if (VendorPaymentTdsamountNew == null || VendorPaymentTdsamountNew == "" || string.IsNullOrEmpty(VendorPaymentTdsamountNew) || string.IsNullOrWhiteSpace(VendorPaymentTdsamountNew))
                {
                    errorMsg += " ," + nameof(VendorPaymentTdsamountNew);
                    validData = false;
                }
                else
                {
                    validData = true;
                }
            }
            if (VendorPaymentDate == null)
            {
                errorMsg += " ," + nameof(VendorPaymentDate);
                validData = false;
            }
            else
            {
                validData = true;
            }
            return validData;
        }

        private async Task SubmitPaymentDetails(VendorPaymentModel model)
        {
            try
            {
                Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Into the SubmitPaymentDetails", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));

                if (SaveButtonName == GeneralConstants.Update)
                {
                    VendorPaymentModel payment = new VendorPaymentModel()
                    {
                        PaymentYear = VendorPaymentYear,
                        FkNoteId = _vendorPaymentModel.NoteId,
                        Notes = VendorPaymentNotesDetails,

                        VendorPaymentDate = VendorPaymentDate != null ? VendorPaymentDate.Value : DateOnly.MinValue,
                        VendorPaymentAmount = VendorPaymentAmount,
                        VendorPaymentTotalAmountPaid = VendorPaymentTotalAmountPaid,

                        VendorPaymentIsGst = IsGSTDetailsVisible,
                        FkGstmasterSrNo = IsGSTDetailsVisible ? SelectedGSTModel.SrNo : 0,
                        VendorPaymentIsTdsapplicable = IsTDSTextBoxVisible,
                        IsPaymentForBranch = IsBranchNameVisible,
                        BankBranchName = IsBranchNameVisible ? BankBranchName : "",

                        VendorPaymentCgst = IsGSTDetailsVisible ? VendorPaymentCgst : 0,
                        VendorPaymentSgst = IsGSTDetailsVisible ? VendorPaymentSgst : 0,
                        VendorPaymentIgst = IsGSTDetailsVisible ? VendorPaymentIgst : 0,

                        InvoiceDate = InvoiceDate,
                        InvoiceNumber = InvoiceNumber,
                        InvoiceParticulars = InvoiceParticulars,

                        PaymentCode = "",

                        VendorPaymentRtgsDate = VendorPaymentRtgsDate,
                        VendorPaymentRtgsAmount = VendorPaymentRtgsAmount,
                        VendorPaymentUtrnumber = VendorPaymentUtrnumber,
                        VendorPaymentTdsamount = IsTDSTextBoxVisible ? Convert.ToDecimal(VendorPaymentTdsamountNew) : 0,

                        LastUpdateBy = UserAccountModel.Username,
                        LastUpdatedDate = DateTime.UtcNow,
                        VendorPaymentId = _vendorPaymentModel.VendorPaymentId,
                        IsActive = true,
                        FkVendorDetailId = _vendorPaymentModel.FkVendorDetailId,
                        InvoiceId = _vendorPaymentModel.InvoiceId,
                    };
                    await _vendorPaymentBusinessLogic.EditUpdateVendorPayment(payment);

                    Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Payment Details Updated Successfully", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));

                    SuccessPopupViewModel.Instance.ShowPopup(Enums.NotificationType.Success, MessagesContants.PaymentNoteDataUpdated, true);
                }
                else
                {
                    VendorPaymentModel payment = new()
                    {
                        PaymentYear = VendorPaymentYear,
                        FkNoteId = PaymentNoteDetails?.NoteId.Value,//
                        FkVendorDetailId = SelectedVendorDetailService.VendorDetailId,
                        Notes = VendorPaymentNotesDetails,

                        VendorPaymentDate = VendorPaymentDate.Value,
                        VendorPaymentAmount = VendorPaymentAmount,
                        VendorPaymentTotalAmountPaid = VendorPaymentTotalAmountPaid,

                        VendorPaymentIsGst = IsGSTDetailsVisible,
                        FkGstmasterSrNo = SelectedGSTModel != null ? SelectedGSTModel.SrNo : 0,
                        VendorPaymentIsTdsapplicable = IsTDSTextBoxVisible,
                        IsPaymentForBranch = IsBranchNameVisible,
                        BankBranchName = IsBranchNameVisible ? BankBranchName : null,

                        VendorPaymentCgst = IsGSTDetailsVisible ? VendorPaymentCgst : 0,
                        VendorPaymentSgst = IsGSTDetailsVisible ? VendorPaymentSgst : 0,
                        VendorPaymentIgst = IsGSTDetailsVisible ? VendorPaymentIgst : 0,

                        InvoiceDate = InvoiceDate,
                        InvoiceNumber = InvoiceNumber,
                        InvoiceParticulars = InvoiceParticulars,

                        PaymentCode = "",

                        VendorPaymentRtgsDate = VendorPaymentRtgsDate,
                        VendorPaymentRtgsAmount = VendorPaymentRtgsAmount,
                        VendorPaymentUtrnumber = VendorPaymentUtrnumber,
                        VendorPaymentTdsamount = IsTDSTextBoxVisible ? Convert.ToDecimal(VendorPaymentTdsamountNew) : 0,

                        CreatedBy = UserAccountModel.Username,
                        CreatedDate = DateTime.UtcNow,
                        IsActive = true,
                    };
                    await _vendorPaymentBusinessLogic.AddVendorPayment(payment);

                    Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Payment Details Added Successfully", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));

                    SuccessPopupViewModel.Instance.ShowPopup(Enums.NotificationType.Success, MessagesContants.PaymentNoteDataAdded, true);
                }

                await HidePaymentForm(this);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, string.Format("Class: {0}, Method: {1} - Failed to submit Payment Details.", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));

                SuccessPopupViewModel.Instance.ShowPopup(Enums.NotificationType.Failure, MessagesContants.PaymentSubmitErroMsg, false, true);
            }
        }

        public async Task HidePaymentForm(object model)
        {
            await _paymentViewModel.HidePaymentForm(this).ConfigureAwait(true);
        }

        private async Task CallAync()
        {
            await MainTask();
        }

        public async Task MainTask()
        {
            await GetAllConfigurations();
            await LoadGSTDetails();
            await LoadVendors();
            await PopulateValues();
        }

        private void CalculateGST()
        {
            if (SelectedGSTModel != null)
            {
                Cgstpercentage = SelectedGSTModel.CgstPercentage;
                Sgstpercentage = SelectedGSTModel.SgstPercentage;
                Igstpercentage = SelectedGSTModel.IgstPercentage;
            }

            VendorPaymentCgst = (VendorPaymentAmount * Cgstpercentage) / 100;
            VendorPaymentIgst = (VendorPaymentAmount * Igstpercentage) / 100;
            VendorPaymentSgst = (VendorPaymentAmount * Sgstpercentage) / 100;

            GSTTotal = Convert.ToDouble(VendorPaymentCgst + VendorPaymentIgst + VendorPaymentSgst);
            VendorPaymentTotalAmountPaid = Convert.ToDecimal(GSTTotal + Convert.ToDouble(VendorPaymentAmount));
        }

        private async Task GetAmountToBepaid()
        {
            try
            {
                Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Getting payment amount", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));

                string? paymentType = SelectedVendorDetailService?.ServicePaymentType;
                decimal? santionedAmt = SelectedVendorDetailService?.ServiceSantionAmount;
                int? vendorDetaillID = SelectedVendorDetailService?.VendorDetailId;
                if (paymentType == GeneralConstants.PaymentTypeNone)
                {
                    EnableTotalPaidAmt = true;
                }
                else
                {
                    EnableTotalPaidAmt = false;

                }
                var res = await _vendorPaymentBusinessLogic.GetAmoutToBePaidDetails(vendorDetaillID, santionedAmt, paymentType).ConfigureAwait(true);
                VendorPaymentAmount = res?.TotalPaymentNotTaxable;

                if (res?.Meassage != null)
                {
                    SuccessPopupViewModel.Instance.ShowPopup(Enums.NotificationType.Alert, res.Meassage, false, true);

                    if (res?.Meassage == MessagesContants.PaymentMsgSantionAmtHigh)
                    {
                        await HidePaymentForm(this);
                    }

                    Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Retrieved payment amount", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, string.Format("Class: {0}, Method: {1} - Failed to get payment amount.", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));
            }
        }
        private async Task LoadGSTDetails()
        {
            var gstDetails = await _gstcalculationMasterBusinessLogic.GetAllGstMaster().ConfigureAwait(true);
            GSTDetails = new ObservableCollection<GstcalculationMasterModel>(gstDetails);
        }


        #region Combobox load vendors and services on combo box selection 

        private async Task LoadVendorPaymentNotes(int vendorId)
        {
            var paymentNotesDetails = await _venderPaymentNotesBusinessLogic.GetPaymentNoteByVendorId(vendorId).ConfigureAwait(true);
            if (paymentNotesDetails != null)
            {
                PaymentNoteDetails = paymentNotesDetails;
                PaymentNoteNo = paymentNotesDetails?.PaymentNoteNo ?? "";
            }
            else
            {
                SuccessPopupViewModel.Instance.ShowPopup(Enums.NotificationType.Warning, MessagesContants.PaymentNoteAlert, true,true);

            }
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
            VendorModels = new ObservableCollection<VendorModel>(vendors.ToList().OrderBy(x => x.VendorName));
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
