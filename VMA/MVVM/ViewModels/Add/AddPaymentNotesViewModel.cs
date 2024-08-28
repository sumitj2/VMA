using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.Abstraction.VMA.Models;
using Serilog;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using VMA.Constants;
using VMA.MVVM.Models;
using VMA.MVVM.ViewModels.Menus;

namespace VMA.MVVM.ViewModels.Add
{
    public class AddPaymentNotesViewModel : ViewModelBase
    {
        private string _saveButtonName;
        private readonly IVendorDetailsBusinessLogic _vendorDetailsBusinessLogic;
        private readonly IVendorBusinessLogic _vendorBusinessLogic;
        private readonly IVenderPaymentNotesBusinessLogic _venderPaymentNotesBusinessLogic;
        private readonly PaymentNotesViewModel _paymentNotesViewModel;
        private readonly VenderPaymentNoteModel? _editPaymentNote;

        public string SaveButtonName
        {
            get => _saveButtonName;

            set
            {
                _saveButtonName = value;
                OnPropertyChanged(nameof(SaveButtonName));
            }
        }

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

        private ObservableCollection<SearchModel> _comboxPaymentMethod;

        public ObservableCollection<SearchModel> ComboxPaymentMethods
        {
            get { return _comboxPaymentMethod; }
            set { _comboxPaymentMethod = value; }
        }

        #endregion

        #region Command        
        public ICommand HidePaymentNotesFormCommand { get; }
        public ICommand SubmitCommand { get; }
        public ICommand ClearFormCommand { get; }

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
                OnPropertyChanged(nameof(SelectedVendorModel));

                var res = _paymentNotesViewModel.TempVendorPaymentNotes.FirstOrDefault(x => x.VendorName == _SelectedVendorModel?.VendorName);
                //_ = LoadVendorServiceDetails(SelectedVendorModel.VendorId);
                if (res != null)
                {
                    var msg1 = @$"{MessagesContants.PaymentNoteAlreadyGeneratedMsg} {_SelectedVendorModel?.VendorName}";

                    SuccessPopupViewModel.Instance.ShowPopup(Enums.NotificationType.Alert, msg1, false, true);

                    _ = HidePaymentNoteForm(this);

                }
                Task.Run(async () =>
                {
                    int countOfnotes = await _venderPaymentNotesBusinessLogic.GetAllVendorsPaymentNotesCount().ConfigureAwait(true);
                    countOfnotes++;
                    PaymentNoteNo = PaymentNoteId + countOfnotes;//_SelectedVendorModel?.VendorId;
                });


            }
        }
        #endregion

        private string? _PaymentNoteNo;
        public string? PaymentNoteNo
        {
            get
            {
                return _PaymentNoteNo;
            }
            set
            {
                _PaymentNoteNo = value;
                OnPropertyChanged(nameof(PaymentNoteNo));
            }
        }

        private string? _PaymentNoteYear;
        public string? PaymentNoteYear
        {
            get
            {
                return _PaymentNoteYear;
            }
            set
            {
                _PaymentNoteYear = value;
                OnPropertyChanged(nameof(PaymentNoteYear));
            }
        }

        private string? _PaymentNoteId;
        public string? PaymentNoteId
        {
            get
            {
                return _PaymentNoteId;
            }
            set
            {
                _PaymentNoteId = value;
                OnPropertyChanged(nameof(PaymentNoteId));
            }
        }

        private DateTime? _PaymentNoteDate;
        public DateTime? PaymentNoteDate
        {
            get
            {
                return _PaymentNoteDate;
            }
            set
            {
                _PaymentNoteDate = value;
                OnPropertyChanged(nameof(PaymentNoteDate));
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

        private VendorDetailModel? _selectedVendorDetailService;
        public VendorDetailModel? SelectedVendorDetailService
        {
            get { return _selectedVendorDetailService; }
            set
            {
                _selectedVendorDetailService = value;
                OnPropertyChanged(nameof(SelectedVendorDetailService));
            }
        }

        #endregion

        private readonly IConfigurationBusinessLogic _configurationBusinessLogic;

        public AddPaymentNotesViewModel(PaymentNotesViewModel paymentNotesViewModel, IVendorDetailsBusinessLogic vendorDetailsBusinessLogic, IVendorBusinessLogic vendorBusinessLogic, IVenderPaymentNotesBusinessLogic venderPaymentNotesBusinessLogic, VenderPaymentNoteModel? editPaymentNote, IConfigurationBusinessLogic configurationBusinessLogic)
        {
            Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Into the constructor", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));

            _paymentNotesViewModel = paymentNotesViewModel;
            _editPaymentNote = editPaymentNote;
            if (_editPaymentNote != null)
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
            _vendorBusinessLogic = vendorBusinessLogic;
            _venderPaymentNotesBusinessLogic = venderPaymentNotesBusinessLogic;
            HidePaymentNotesFormCommand = new ViewModelAsyncCommand<VenderPaymentNoteModel>(HidePaymentNoteForm);
            SubmitCommand = new ViewModelAsyncCommand<VenderPaymentNoteModel>(SubmitPaymentNote, ValidatePaymentNote);
            _configurationBusinessLogic = configurationBusinessLogic;
            CallAync();
        }

        public async Task GetAllConfigurations()
        {
            var allConfigurations = await _configurationBusinessLogic.GetConfigurations().ConfigureAwait(true);

            string? financialYear = allConfigurations.FirstOrDefault(x => x.Cfgkey == GeneralConstants.CFGKeyFinacialYear)?.CfgValue;
            string? noteId = allConfigurations.FirstOrDefault(x => x.Cfgkey == GeneralConstants.CFGKeyNoteID)?.CfgValue;
            PaymentNoteYear = financialYear;
            PaymentNoteNo = PaymentNoteId = noteId + PaymentNoteNo;
        }

        string errorMsg = "";
        private bool ValidatePaymentNote()
        {
            bool validData;

            if (SelectedVendorModel == null)
            {
                validData = false;
                errorMsg += nameof(SelectedVendorModel);
            }
            else
            {
                validData = true;
            }
            if (PaymentNoteDate == null)
            {
                errorMsg += ", " + nameof(PaymentNoteDate);

                validData = false;
            }
            else
            {
                validData = true;
            }
            return validData;
        }

        private async Task SubmitPaymentNote(VenderPaymentNoteModel model)
        {
            try
            {
                Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Into the SubmitPaymentNote", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));

                if (SaveButtonName == GeneralConstants.Update)
                {
                    VenderPaymentNoteModel payment = new()
                    {
                        LastUpdateBy = UserAccountModel.Username,
                        IsActive = true,
                        PaymentNoteNo = PaymentNoteNo ?? "",
                        PaymentNoteDate = PaymentNoteDate.ToString(),
                        NoteId = _editPaymentNote?.NoteId.Value,
                        FkVendorId = _editPaymentNote?.VendorId,
                        PaymentNoteYear = PaymentNoteYear
                    };
                    await _venderPaymentNotesBusinessLogic.EditUpdatePaymentNotes(payment);

                    Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Vendor payment updated Successfully", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));

                    SuccessPopupViewModel.Instance.ShowPopup(Enums.NotificationType.Success, MessagesContants.PaymentNoteDataUpdated, true);
                }
                else
                {
                    VenderPaymentNoteModel paymentNote = new()
                    {
                        PaymentNoteNo = PaymentNoteNo ?? "",
                        PaymentNoteDate = PaymentNoteDate.ToString(),
                        CreatedBy = UserAccountModel.Username,
                        CreatedDate = DateTime.UtcNow,
                        IsActive = true,
                        FkVendorId = SelectedVendorModel.VendorId,
                        PaymentNoteYear = PaymentNoteYear
                    };
                    await _venderPaymentNotesBusinessLogic.AddPaymentNotes(paymentNote);

                    Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Vendor payment saved Successfully", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));


                    SuccessPopupViewModel.Instance.ShowPopup(Enums.NotificationType.Success, MessagesContants.PaymentNoteDataAdded, true);
                }

                await HidePaymentNoteForm(this);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, string.Format("Class: {0}, Method: {1} - Failed to save vendor payments", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));

                SuccessPopupViewModel.Instance.ShowPopup(Enums.NotificationType.Failure, "Failed to save vendor payments, Please contact to Administrator", false, true);
            }
        }

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
        private async Task HidePaymentNoteForm(object model)
        {
            await _paymentNotesViewModel.HidePaymentNotesForm(this);
        }

        private async Task PopulateValues()
        {
            var countOfnotes = await _venderPaymentNotesBusinessLogic.GetAllPaymentNotes().ConfigureAwait(true);
            var lastPaymentNote= countOfnotes?.OrderByDescending(x => x.PaymentNoteNo)?.ToList()?.FirstOrDefault()?.PaymentNoteNo;
            if (_editPaymentNote != null)
            {
                //PaymentNoteId = "";
                PaymentNoteYear = _editPaymentNote.PaymentNoteYear;
                PaymentNoteNo = lastPaymentNote != null ? IncrementNoteId(lastPaymentNote) : IncrementNoteId(_editPaymentNote.PaymentNoteNo);
                PaymentNoteDate = Convert.ToDateTime(_editPaymentNote.PaymentNoteDate);
                SelectedVendorName = VendorModels?.FirstOrDefault(x => x.VendorId == _editPaymentNote.FkVendorId)?.VendorName ?? "";
            }
        }
        static string IncrementNoteId(string noteId)
        {
            // Regular expression to extract the numeric part
            string pattern = @"(\d+)$";
            var match = Regex.Match(noteId, pattern);

            if (match.Success)
            {
                // Convert the numeric part to an integer and increment it
                int number = int.Parse(match.Value) + 1;

                // Reassemble the string with the incremented number
                return Regex.Replace(noteId, pattern, number.ToString());
            }
            else
            {
                throw new ArgumentException("The Note ID does not contain a valid numeric part.");
            }
        }

        #region Combobox load vendors and services on combo box selection 

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
