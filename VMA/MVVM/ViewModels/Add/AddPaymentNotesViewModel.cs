using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.Abstraction.VMA.Models;
using BusinessLogic.VMA;
using Database.Abstraction.VMA.Contract;
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

                var res = VendorModels.FirstOrDefault(x => x.VendorName == _SelectedVendorModel?.VendorName);
                //_ = LoadVendorServiceDetails(SelectedVendorModel.VendorId);
                if (res != null)
                {
                    var msg1 = @$"Payment Note alreday genrated for {_SelectedVendorModel?.VendorName}";
                    MessageBox.Show(msg1);
                    _=HidePaymentNoteForm(this);

                }
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
            PaymentNoteNo = Convert.ToString(paymentNotesViewModel.VendorPaymentNotes.Count + 1);
            _paymentNotesViewModel = paymentNotesViewModel;
            _editPaymentNote = editPaymentNote;
            if (_editPaymentNote != null)
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

            string? financialYear = allConfigurations.FirstOrDefault(x => x.Cfgkey == "FinancialYear")?.CfgValue;
            string? noteId = allConfigurations.FirstOrDefault(x => x.Cfgkey == "NoteId")?.CfgValue;
            PaymentNoteYear = financialYear;
            PaymentNoteId = noteId;
        }

        private bool ValidatePaymentNote()
        {
            return true;
        }

        private async Task SubmitPaymentNote(VenderPaymentNoteModel model)
        {
            if (SaveButtonName == "Update")
            {
                VenderPaymentNoteModel payment = new()
                {
                    LastUpdateBy = UserAccountModel.Username,
                    IsActive = true,
                    PaymentNoteNo = PaymentNoteId + PaymentNoteNo ?? "",
                    PaymentNoteDate = Convert.ToDateTime(PaymentNoteDate),
                    NoteId = _editPaymentNote?.NoteId,
                    FkVendorId = _editPaymentNote?.VendorId,
                    PaymentNoteYear = PaymentNoteYear
                };
                await _venderPaymentNotesBusinessLogic.EditUpdatePaymentNotes(payment);

                SuccessPopupViewModel.Instance.ShowPopup(Enums.NotificationType.Success, "Data Updated Successfully");
            }
            else
            {
                VenderPaymentNoteModel paymentNote = new()
                {
                    PaymentNoteNo = PaymentNoteId + PaymentNoteNo ?? "",
                    PaymentNoteDate = Convert.ToDateTime(PaymentNoteDate),
                    CreatedBy = UserAccountModel.Username,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true,
                    FkVendorId = SelectedVendorModel.VendorId,
                    PaymentNoteYear = PaymentNoteYear
                };
                await _venderPaymentNotesBusinessLogic.AddPaymentNotes(paymentNote);


                SuccessPopupViewModel.Instance.ShowPopup(Enums.NotificationType.Success, "Data Added Successfully");
            }

            await HidePaymentNoteForm(this);
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
            if (_editPaymentNote != null)
            {
                //PaymentNoteId = "";
                PaymentNoteNo = _editPaymentNote.PaymentNoteNo.Replace(PaymentNoteId,"");
                PaymentNoteDate = _editPaymentNote.PaymentNoteDate;
                SelectedVendorName = VendorModels?.FirstOrDefault(x => x.VendorId == _editPaymentNote.FkVendorId)?.VendorName ?? "";
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
