using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.Abstraction.VMA.Models;
using Database.VMA.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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
        public ICommand SwitchToTab1Command { get; }
        public ICommand SwitchToTab2Command { get; }

        #endregion
       

        public AddDetailedInfoViewModel(DetailedInfoViewModel detailedInfoViewModel, VendorDetailModel vendorDetailViewModel,IVendorDetailsBusinessLogic vendorDetailsBusinessLogic,IVendorServiceBusinessLogic vendorServiceBusinessLogic)
        {
            ComboxPaymentMethods =
            [
                new(){NameSearch="Monthly",SearchId=1},
                new(){NameSearch="Quarterly",SearchId=2},
                new(){NameSearch="Half Yearly",SearchId=3},
                new(){NameSearch="Yearly",SearchId=4},

            ];
            _vendorDetailViewModel = vendorDetailViewModel;
            _vendorServiceBusinessLogic= vendorServiceBusinessLogic;
            _vendorDetailsBusinessLogic = vendorDetailsBusinessLogic;
            BackCommand = new ViewModelCommand(CanGoBack);
            NextCommand = new ViewModelCommand(CanGoNext);
            if (_vendorDetailViewModel != null)
            {
                SaveButtonName = "Update";
            }
            else
            {
                SaveButtonName = "Submit";
            }
            _detailedInfoViewModel = detailedInfoViewModel;
            HideDetailInfoFormCommand = new ViewModelAsyncCommand<VendorViewModel>(HideDetailInfoForm);
            _vendorDetailViewModel = vendorDetailViewModel;
            CallAync();            
        }
        private async void CallAync()
        {
            await ManinTasss();
        }
        public async Task ManinTasss()
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

        private async Task LoadVendorServiceDetails()
        {
            var vendorServiceDetails = await _vendorServiceBusinessLogic.GetAllVendorServices().ConfigureAwait(true);

            VendorDetailServices = new ObservableCollection<VendorServiceModel>(vendorServiceDetails);
        }

        private async Task HideDetailInfoForm(VendorViewModel model)
        {
            await _detailedInfoViewModel.HideDetailInfoForm(this).ConfigureAwait(true);
        }

    }
}
