using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.Abstraction.VMA.Models;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using VMA.MVVM.ViewModels.Add;

namespace VMA.MVVM.ViewModels.Menus
{
    public class VendorViewModel : ViewModelBase
    {
        private readonly IVendorBusinessLogic _vendorBusinessLogic;

        private readonly MainViewModel _parentViewModel;

        private ObservableCollection<VendorModel> _vendors;
        public ObservableCollection<VendorModel> Vendors
        {
            get { return _vendors; }
            set
            {
                _vendors = value;
                OnPropertyChanged(nameof(Vendors));
            }
        }

        private ObservableCollection<SearchModel> _comboItem;

        public ObservableCollection<SearchModel> ComboItem
        {
            get { return _comboItem; }
            set { _comboItem = value; }
        }
        private SearchModel _selectComboItem;

        public SearchModel selectComboItem
        {
            get { return _selectComboItem; }
            set { _selectComboItem = value; }
        }

        private VendorModel _selectedVendor;

        public VendorModel SelectedVendor
        {
            get { return _selectedVendor; }
            set { _selectedVendor = value; OnPropertyChanged(nameof(SelectedVendor)); }
        }


        // Commands
        public ICommand AddShowVendorFormCommand { get; }

        public ICommand UpdateVendorFormCommand { get; }
        public ICommand HideVendorFormCommand { get; }

        public ICommand EditVendorCommand {  get; }

        public VendorViewModel(IVendorBusinessLogic vendorBusinessLogic, MainViewModel parentViewModel)
        {
            ComboItem =
            [
                new(){NameSearch="Vedor Code",SearchId=1},
                new(){NameSearch="Vedor Name",SearchId=2},

            ];
            _vendorBusinessLogic = vendorBusinessLogic;
            _parentViewModel = parentViewModel;
            AddShowVendorFormCommand = new ViewModelCommand(ShowVendorForm);            
            HideVendorFormCommand = new ViewModelCommand(HideVendorForm);
            EditVendorCommand = new ViewModelCommand(EditVendor);
            _ = GetVendors();
        }

        private void EditVendor(object obj)
        {
            _parentViewModel.CurrentChildView = new AddUpdateVendorViewModel(_vendorBusinessLogic, this, (VendorModel)obj);
        }

        private async Task GetVendors()
        {
            var vendors = await _vendorBusinessLogic.GetAllVendor();
            Vendors = new ObservableCollection<VendorModel>(vendors);
        }
        private void ShowVendorForm(object obj)
        {
            _parentViewModel.CurrentChildView = new AddUpdateVendorViewModel(_vendorBusinessLogic, this, SelectedVendor);           
        }

        public void HideVendorForm(object obj)
        {
            _parentViewModel.CurrentChildView = this;
        }
    }
}
