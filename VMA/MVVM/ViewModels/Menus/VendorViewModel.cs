using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.Abstraction.VMA.Models;
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

        // Commands
        public ICommand AddShowVendorFormCommand { get; }
        public ICommand HideVendorFormCommand { get; }    
        public ICommand SelectAllCommand { get; }

        public VendorViewModel(IVendorBusinessLogic vendorBusinessLogic, MainViewModel parentViewModel)
        {
            _vendorBusinessLogic = vendorBusinessLogic;
            _parentViewModel = parentViewModel;
            AddShowVendorFormCommand = new ViewModelCommand(ShowVendorForm);
            HideVendorFormCommand = new ViewModelCommand(HideVendorForm);
            SelectAllCommand = new ViewModelCommand(CheckBoxChecked);
            _ = GetVendors();
        }

        private void CheckBoxChecked(object obj)
        {
            
        }

        private async Task GetVendors()
        {
            var vendors = await _vendorBusinessLogic.GetAllVendor();
            Vendors = new ObservableCollection<VendorModel>(vendors);
        }

        private void ShowVendorForm(object obj)
        {
            _parentViewModel.CurrentChildView = new AddUpdateVendorViewModel(_vendorBusinessLogic, this);
        }

        public void HideVendorForm(object obj)
        {
            _parentViewModel.CurrentChildView = this;
        }
    }
}
