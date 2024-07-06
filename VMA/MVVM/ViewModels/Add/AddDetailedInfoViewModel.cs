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
        private string _saveButtonName;

        #region Properties
        private string _vendorDetailCategory;
        private string _ratePerUnit;

        private int _quantityOfUnit;

        private string _serviceSantionAmount;

        private DateOnly _serviceStartDate;

        private DateOnly _serviceEndDate;

        private string _serviceSantionedBy;

        private string _serviceType;

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


        public string ServiceSantionAmount
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

        private SearchModel _selectPaymentType;
        public SearchModel SelectPaymentType
        {
            get { return _selectPaymentType; }
            set { _selectPaymentType = value; }
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

        private VendorServiceModel _selectedVendorDetailService;
        public VendorServiceModel SelectedVendorDetailService
        {
            get { return _selectedVendorDetailService; }
            set
            {

                _selectedVendorDetailService = value;
                OnPropertyChanged(nameof(SelectedVendorDetailService));

            }
        }

        #region Observable collections
        private ObservableCollection<VendorServiceModel> _vendorDetailServices;
        private ObservableCollection<SearchModel> _comboxPaymentMethod;

        public ObservableCollection<VendorServiceModel> VendorDetailServices
        {
            get { return _vendorDetailServices; }
            set
            {
                _vendorDetailServices = value;
                OnPropertyChanged(nameof(VendorDetailServices));
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


        public AddDetailedInfoViewModel(DetailedInfoViewModel detailedInfoViewModel, VendorDetailModel vendorDetailViewModel, IVendorDetailsBusinessLogic vendorDetailsBusinessLogic, IVendorServiceBusinessLogic vendorServiceBusinessLogic)
        {
            ComboxPaymentMethods =
            [
                new(){NameSearch="Monthly",SearchId=1},
                new(){NameSearch="Quarterly",SearchId=2},
                new(){NameSearch="Half Yearly",SearchId=3},
                new(){NameSearch="Yearly",SearchId=4},

            ];

            _vendorDetailViewModel = vendorDetailViewModel;
            _vendorServiceBusinessLogic = vendorServiceBusinessLogic;
            _vendorDetailsBusinessLogic = vendorDetailsBusinessLogic;
            BackCommand = new ViewModelCommand(CanGoBack);
            NextCommand = new ViewModelCommand(CanGoNext);
            SubmitCommand = new ViewModelAsyncCommand<VendorDetailModel>(SaveVendorServiceDetails, ValidateVendorServiceDetails);
            ClearFormCommand = new ViewModelAsyncCommand<VendorDetailModel>(ClearFormFields);
            if (_vendorDetailViewModel != null)
            {
                SaveButtonName = "Update";
            }
            else
            {
                SaveButtonName = "Submit";
            }
            _detailedInfoViewModel = detailedInfoViewModel;
            HideDetailInfoFormCommand = new ViewModelAsyncCommand<VendorDetailModel>(HideDetailInfoForm);
            _vendorDetailViewModel = vendorDetailViewModel;
            CallAync();
        }

        private async Task ClearFormFields(VendorDetailModel model)
        {

        }

        private bool ValidateVendorServiceDetails()
        {
            return true;
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
                    ServicePaymentType = SelectPaymentType.NameSearch,
                    VendorServiceName = SelectedVendorDetailService.VendorServiceName,
                    FkVendorServiceId = SelectedVendorDetailService.FkVendorId,
                    VendorServiceId = SelectedVendorDetailService.VendorServiceId,
                    ServiceSantionedBy = ServiceSantionedBy,
                    LastUpdateBy = UserAccountModel.Username
                   
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
                    QuantityOfUnit = QuantityOfUnit,
                    ServiceSantionAmount = ServiceSantionAmount,
                    ServiceEndDate = ServiceEndDate,
                    RatePerUnit = RatePerUnit,
                    ServiceType = ServiceType,
                    VendorDetailCategory = VendorDetailCategory,
                    ServiceStartDate = ServiceStartDate,
                    ServicePaymentType = SelectPaymentType.NameSearch,
                    VendorServiceName = SelectedVendorDetailService.VendorServiceName,
                    FkVendorServiceId = SelectedVendorDetailService.FkVendorId,
                    VendorServiceId = SelectedVendorDetailService.VendorServiceId,
                    ServiceSantionedBy = ServiceSantionedBy

                };
                await _vendorDetailsBusinessLogic.AddVendorDetails(vendorModel);

                SuccessPopupViewModel.Instance.ShowPopup(Enums.NotificationType.Success, "Data Added Successfully");
            }

            await HideDetailInfoForm(this);
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
        private async Task PopulateValues()
        {
            if (_vendorDetailViewModel != null)
            {
                ServiceSantionedBy = _vendorDetailViewModel.ServiceSantionedBy ?? "";
                ServiceStartDate = (DateOnly)_vendorDetailViewModel.ServiceStartDate;
                ServiceEndDate = (DateOnly)_vendorDetailViewModel.ServiceEndDate;
                ServiceSantionAmount = _vendorDetailViewModel.ServiceSantionAmount ?? "";
                RatePerUnit = _vendorDetailViewModel.RatePerUnit ?? "";
                QuantityOfUnit = _vendorDetailViewModel.QuantityOfUnit ?? 0;
                ServiceType = _vendorDetailViewModel.ServiceType ?? "";
                VendorDetailCategory = _vendorDetailViewModel.VendorDetailCategory ?? "";
                var paymentMethod = ComboxPaymentMethods.ToList().Find(x => x.NameSearch == _vendorDetailViewModel.ServicePaymentType);
               
                //to-do Edit button payment method is not updated need to check
                if (paymentMethod != null)
                    SelectPaymentType = ComboxPaymentMethods[1];//ComboxPaymentMethods[ComboxPaymentMethods.IndexOf(paymentMethod)];


                var vendorID = VendorDetailServices.ToList().Find(x => x.VendorServiceId == _vendorDetailViewModel.VendorServiceId);

                if (vendorID != null)
                {
                    SelectedVendorDetailService = VendorDetailServices[VendorDetailServices.IndexOf(vendorID)];

                }
            }
        }

        /// <summary>
        /// Combobox load item with Vendor Service Name
        /// </summary>
        /// <returns></returns>
        private async Task LoadVendorServiceDetails()
        {
            var vendorServiceDetails = await _vendorServiceBusinessLogic.GetAllVendorServices().ConfigureAwait(true);

            VendorDetailServices = new ObservableCollection<VendorServiceModel>(vendorServiceDetails);
        }

        private async Task HideDetailInfoForm(object model)
        {
            await _detailedInfoViewModel.HideDetailInfoForm(this).ConfigureAwait(true);
        }

    }
}
