using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.Abstraction.VMA.Models;
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
        private int _selectedTabIndex;
        private int _numbersOfTab = 1;
        private string _saveButtonName;
        private readonly IVendorDetailsBusinessLogic _vendorDetailsBusinessLogic;
        private readonly IVendorPaymentBusinessLogic _vendorPaymentBusinessLogic;
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

        #region Observable collections for Combo box
        private ObservableCollection<VendorDetailModel> _vendorDetails;
        private ObservableCollection<SearchModel> _comboxPaymentMethod;
        private ObservableCollection<VendorPaymentModel> _vendorsPayment;
        private ObservableCollection<VendorPaymentModel> _tempvendorsPayment;

        public ObservableCollection<VendorDetailModel> VendorServiceDetails
        {
            get { return _vendorDetails; }
            set
            {
                _vendorDetails = value;
                OnPropertyChanged(nameof(VendorServiceDetails));
            }
        }


        public ObservableCollection<VendorPaymentModel> VendorsPayment
        {
            get { return _vendorsPayment; }
            set
            {
                _vendorsPayment = value;
                OnPropertyChanged(nameof(VendorsPayment));
            }
        }

        public ObservableCollection<VendorPaymentModel> TempVendorsPayment
        {
            get { return _tempvendorsPayment; }
            set
            {
                _tempvendorsPayment = value;
                OnPropertyChanged(nameof(TempVendorsPayment));
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
        public ICommand HidePaymentNotesFormCommand { get; }
        public ICommand SubmitCommand { get; }
        public ICommand ClearFormCommand { get; }

        #endregion

        #region Properties

        private string? _PaymentNoteNo;
        private DateTime? _PaymentNoteDate;
        private string? _InvoiceNumber;
        private DateTime? _InvoiceDate;
        private string? _InvoiceParticulars;

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

        //private bool _IsComboBoxServiceVisible;

        //public bool IsComboBoxServiceVisible
        //{
        //    get { return _IsComboBoxServiceVisible; }
        //    set
        //    {
        //        _IsComboBoxServiceVisible = value;
        //        OnPropertyChanged(nameof(HideServiceSelectComboBox));
        //    }
        //}

        //private bool _IsComboBoxPaymentCodeVisible;

        //public bool IsComboBoxPaymentCodeVisible
        //{
        //    get { return _IsComboBoxPaymentCodeVisible; }
        //    set
        //    {
        //        _IsComboBoxPaymentCodeVisible = value;
        //        OnPropertyChanged(nameof(HidePaymentCodeComboBox));
        //    }
        //}

        //private bool _IsTextBoxServiceVisible;

        //public bool IsTextBoxServiceVisible
        //{
        //    get { return _IsTextBoxServiceVisible; }
        //    set
        //    {
        //        _IsTextBoxServiceVisible = value;
        //        OnPropertyChanged(nameof(HideSelectedService));
        //    }
        //}

        //private bool _IsTextBoxSelectedVendorVisible;

        //public bool IsTextBoxSelectedVendorVisible
        //{
        //    get { return _IsTextBoxSelectedVendorVisible; }
        //    set
        //    {
        //        _IsTextBoxSelectedVendorVisible = value;
        //        OnPropertyChanged(nameof(HideSelectedVendor));
        //    }
        //}


        #endregion


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

        private VendorDetailModel? _selectedVendorServiceDetails;
        public VendorDetailModel? SelectedVendorServiceDetails
        {
            get { return _selectedVendorServiceDetails; }
            set
            {

                _selectedVendorServiceDetails = value;
                OnPropertyChanged(nameof(SelectedVendorServiceDetails));
                AddPaymentItemInCombo(SelectedVendorServiceDetails);

            }
        }

        private VendorPaymentModel? _SelectedVendorPaymentCode;
        public VendorPaymentModel? SelectedVendorPaymentCode
        {
            get { return _SelectedVendorPaymentCode; }
            set
            {

                _SelectedVendorPaymentCode = value;
                OnPropertyChanged(nameof(SelectedVendorPaymentCode));


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

        private void AddPaymentItemInCombo(VendorDetailModel? selectedVendorServiceDetails)
        {
            _ = LoadVendorServicePayment(selectedVendorServiceDetails?.VendorDetailId);
        }
        //public Visibility HidePaymentCodeComboBox
        //{
        //    get { return IsComboBoxPaymentCodeVisible ? Visibility.Visible : Visibility.Collapsed; }
        //}
        //public Visibility HideServiceSelectComboBox
        //{
        //    get { return IsComboBoxServiceVisible ? Visibility.Visible : Visibility.Collapsed; }
        //}

        //public Visibility HideSelectedVendor
        //{
        //    get { return IsTextBoxSelectedVendorVisible ? Visibility.Visible : Visibility.Collapsed; }
        //}

        //public Visibility HideSelectedService
        //{
        //    get { return IsTextBoxServiceVisible ? Visibility.Visible : Visibility.Collapsed; }
        //}
        public AddPaymentNotesViewModel(PaymentNotesViewModel paymentNotesViewModel, IVendorDetailsBusinessLogic vendorDetailsBusinessLogic, IVendorPaymentBusinessLogic vendorPaymentBusinessLogic, IVenderPaymentNotesBusinessLogic venderPaymentNotesBusinessLogic, VenderPaymentNoteModel? editPaymentNote)
        {
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
            _vendorPaymentBusinessLogic = vendorPaymentBusinessLogic;
            _venderPaymentNotesBusinessLogic = venderPaymentNotesBusinessLogic;
            HidePaymentNotesFormCommand = new ViewModelAsyncCommand<VenderPaymentNoteModel>(HidePaymentNoteForm);
            SubmitCommand = new ViewModelAsyncCommand<VenderPaymentNoteModel>(SubmitPaymentNote, ValidatePaymentNote);
            CallAync();
        }

        private bool ValidatePaymentNote()
        {
            return true;
        }

        private async Task SubmitPaymentNote(VenderPaymentNoteModel model)
        {
            if (SaveButtonName == "Update")
            {
                VenderPaymentNoteModel payment = new VenderPaymentNoteModel()
                {
                    LastUpdateBy = UserAccountModel.Username,
                    IsActive = true,
                    PaymentNoteNo = PaymentNoteNo,
                    PaymentNoteDate = Convert.ToDateTime(PaymentNoteDate),
                    NoteId = _editPaymentNote.NoteId
                };
                await _venderPaymentNotesBusinessLogic.EditUpdatePaymentNotes(payment);

                SuccessPopupViewModel.Instance.ShowPopup(Enums.NotificationType.Success, "Data Updated Successfully");
            }
            else
            {
                VenderPaymentNoteModel paymentNote = new VenderPaymentNoteModel()
                {
                    PaymentNoteNo = PaymentNoteNo,
                    PaymentNoteDate = Convert.ToDateTime(PaymentNoteDate),                   
                    CreatedBy = UserAccountModel.Username,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true
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
            await LoadVendorServiceDetails();
            await LoadVendorServicePayment(null);
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
        private async Task HidePaymentNoteForm(object model)
        {
            await _paymentNotesViewModel.HidePaymentNotesForm(this);
        }

        private async Task PopulateValues()
        {
            if (_editPaymentNote != null)
            {
                PaymentNoteNo = _editPaymentNote.PaymentNoteNo;
                PaymentNoteDate = _editPaymentNote.PaymentNoteDate;
                //InvoiceDate = _editPaymentNote.InvoiceDate;
                //InvoiceNumber = _editPaymentNote.InvoiceNumber;
                //InvoiceParticulars = _editPaymentNote.InvoiceParticulars;
                //TextBoxPaymentCodeName = _editPaymentNote.PaymentCode;
                //TextBoxServiceName = _editPaymentNote.VendorServiceName;




            }
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

        /// <summary>
        /// Load all payment to get show conditionly on combo box on service selected
        /// </summary>
        /// <returns></returns>
        private async Task LoadVendorServicePayment(int? id)
        {
            var vendors = await _vendorPaymentBusinessLogic.GetAllVendorPayment().ConfigureAwait(true);
            if (id == null)
            {
                VendorsPayment = TempVendorsPayment = new ObservableCollection<VendorPaymentModel>(vendors);
            }
            else
            {
                VendorsPayment = TempVendorsPayment = new ObservableCollection<VendorPaymentModel>(vendors.Where(x => x.FkVendorDetailId == id));
            }
        }
        #region Combobox load vendors and services on combo box selection 

        /// <summary>
        /// Combobox load Vendor Service Name on selection of Vendor
        /// </summary>
        /// <returns></returns>
        private async Task LoadVendorServiceDetails(int vendorId)
        {
            //var vendorServiceDetails = await _vendorDetailsBusinessLogic.GetAllVendorDetails().ConfigureAwait(true);

            //VendorDetailServices = new ObservableCollection<VendorServiceModel>(vendorServiceDetails.Where(x => x.FkVendorId == vendorId));
        }

        /// <summary>
        /// Combo box load vendors
        /// </summary>
        /// <returns></returns>
        private async Task LoadVendors()
        {
            //var vendors = await _vendorBusinessLogic.GetAllVendor().ConfigureAwait(true);
            //VendorModels = new ObservableCollection<VendorModel>(vendors);
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
