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

        private VendorDetailModel _selectedVendorServiceDetails;
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

        private async void GeneratePaymentCode(VendorDetailModel? vendorDetailModel)
        {
            var paymentCode=await _vendorPaymentBusinessLogic.GeneratePaymentCode(vendorDetailModel);
        }
        #endregion

        public AddPaymentsViewModel(PaymentsViewModel vendorViewModel, IVendorDetailsBusinessLogic vendorDetailsBusinessLogic, VendorPaymentModel vendorPaymentModel,IVendorPaymentBusinessLogic vendorPaymentBusinessLogic)
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
            CallAync();
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
        
    }
}
