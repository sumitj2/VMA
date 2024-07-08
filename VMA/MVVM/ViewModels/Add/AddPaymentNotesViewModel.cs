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

        #endregion

        private void AddPaymentItemInCombo(VendorDetailModel selectedVendorServiceDetails)
        {
            _=LoadVendorServicePayment(selectedVendorServiceDetails.VendorDetailId);
        }

        public AddPaymentNotesViewModel(PaymentNotesViewModel paymentNotesViewModel, IVendorDetailsBusinessLogic vendorDetailsBusinessLogic, IVendorPaymentBusinessLogic vendorPaymentBusinessLogic,IVenderPaymentNotesBusinessLogic venderPaymentNotesBusinessLogic)
        {
            _paymentNotesViewModel = paymentNotesViewModel;
            
            if (_paymentNotesViewModel != null)
            {
                SaveButtonName = "Update";
            }
            else
            {
                SaveButtonName = "Submit";
            }
            _vendorDetailsBusinessLogic = vendorDetailsBusinessLogic;
            _vendorPaymentBusinessLogic= vendorPaymentBusinessLogic;
            _venderPaymentNotesBusinessLogic= venderPaymentNotesBusinessLogic;
            HidePaymentNotesFormCommand = new ViewModelAsyncCommand<VenderPaymentNoteModel>(HidePaymentNoteForm);            
            SubmitCommand =new  ViewModelAsyncCommand<VenderPaymentNoteModel>(SubmitPaymentNote,ValidatePaymentNote);
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
                VenderPaymentNoteModel payment = new()
                {
                   
                    //LastUpdateBy = UserAccountModel.Username,
                    //VendorPaymentId = _vendorPaymentModel.VendorPaymentId,
                    //IsActive = true,
                    //NoteId = 
                };
                await _venderPaymentNotesBusinessLogic.EditUpdatePaymentNotes(payment);

                SuccessPopupViewModel.Instance.ShowPopup(Enums.NotificationType.Success, "Data Updated Successfully");
            }
            else
            {
                VenderPaymentNoteModel paymentNote = new()
                {
                    PaymentNoteNo = PaymentNoteNo,
                    PaymentNoteDate=PaymentNoteDate,
                    InvoiceNumber = InvoiceNumber,
                    InvoiceDate = InvoiceDate,
                    InvoiceParticulars = InvoiceParticulars,
                    PaymentCode = SelectedVendorPaymentCode?.PaymentCode,                   
                    VendorServiceName=SelectedVendorServiceDetails?.VendorServiceName,
                    FkVendorPaymentId= SelectedVendorPaymentCode?.VendorPaymentId,                    
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
            ///await LoadVendorServicePayment();
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
        private async Task LoadVendorServicePayment(int id)
        {
            var vendors = await _vendorPaymentBusinessLogic.GetAllVendorPayment().ConfigureAwait(true);
            VendorsPayment = TempVendorsPayment = new ObservableCollection<VendorPaymentModel>(vendors.Where(x=>x.FkVendorDetailId==id));
        }
    }
}
