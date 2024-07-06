using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.Abstraction.VMA.Models;
using Database.VMA.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VMA.MVVM.ViewModels.Add;
using VMA.MVVM.Views.Add;

namespace VMA.MVVM.ViewModels.Menus
{
    public class ProductServicesViewModel : ViewModelBase
    {
        private readonly IVendorBusinessLogic _vendorBusinessLogic;
        private readonly IVendorServiceBusinessLogic _vendorServiceBusinessLogic;
        private readonly MainViewModel _parentViewModel;
        private VendorServiceModel _selectedVendor;
        private SearchModel _selectComboItem;
        private string _searchValue;


        public SearchModel SelectComboItem
        {
            get { return _selectComboItem; }
            set { _selectComboItem = value; }
        }
        public VendorServiceModel SelectedVendorService
        {
            get { return _selectedVendor; }
            set { _selectedVendor = value; OnPropertyChanged(nameof(SelectedVendorService)); }
        }

        public string SearchValue
        {
            get { return _searchValue; }
            set
            {
                _searchValue = value;

                if (SelectComboItem != null && !string.IsNullOrEmpty(value))
                {
                    PropertyInfo? propertyInfo = typeof(VendorServiceModel)?.GetProperty(SelectComboItem.NameSearch.Replace(" ", ""));

                    VendorsServices = new ObservableCollection<VendorServiceModel>(TempVendorServices.Where(x => propertyInfo?.GetValue(x, null)?
                                                                                      .ToString()?
                                                                                      .ToLower(System.Globalization.CultureInfo.CurrentCulture)
                                                                                      .Contains(value, StringComparison.CurrentCultureIgnoreCase) ?? false));
                }
                else
                {
                    VendorsServices = TempVendorServices;
                }

                OnPropertyChanged(nameof(SearchValue));
            }
        }

        #region Observable collections
        private ObservableCollection<VendorServiceModel> _vendors;
        private ObservableCollection<VendorServiceModel> _tempvendors;
        private ObservableCollection<SearchModel> _comboItem;

        public ObservableCollection<VendorServiceModel> VendorsServices
        {
            get { return _vendors; }
            set
            {
                _vendors = value;
                OnPropertyChanged(nameof(VendorsServices));
            }
        }

        public ObservableCollection<VendorServiceModel> TempVendorServices
        {
            get { return _tempvendors; }
            set
            {
                _tempvendors = value;
                OnPropertyChanged(nameof(TempVendorServices));
            }
        }

        public ObservableCollection<SearchModel> ComboItem
        {
            get { return _comboItem; }
            set { _comboItem = value; }
        }
        #endregion

        #region commands

        public ICommand AddShowVendorServiceFormCommand { get; }

        public ICommand UpdateVendorServiceFormCommand { get; }
        public ICommand HideVendorServiceFormCommand { get; }

        public ICommand EditVendorServiceCommand { get; }
        #endregion

        public ProductServicesViewModel(IVendorServiceBusinessLogic vendorServiceBusinessLogic, IVendorBusinessLogic vendorBusinessLogic, MainViewModel parentViewModel)
        {
            ComboItem =
            [
                new(){NameSearch="Vendor Code",SearchId=1},
                new(){NameSearch="Vendor Name",SearchId=2},
                new(){NameSearch="Vendor Services Name",SearchId=3},

            ];
            _vendorBusinessLogic = vendorBusinessLogic;
            _vendorServiceBusinessLogic = vendorServiceBusinessLogic;
            _parentViewModel = parentViewModel;
            AddShowVendorServiceFormCommand = new ViewModelAsyncCommand<VendorServiceModel>(ShowVendorServicesForm);
            HideVendorServiceFormCommand = new ViewModelAsyncCommand<VendorServiceModel>(HideVendorServiceForm);
            EditVendorServiceCommand = new ViewModelAsyncCommand<VendorServiceModel>(EditVendor);
            _ = GetVendorServices();
        }

        private async Task EditVendor(VendorServiceModel model)
        {
            SuccessPopupViewModel.Instance.ShowPopup(Enums.NotificationType.Alert, "Please wait...");

            _parentViewModel.CurrentChildView = new AddProductServicesViewModel(_vendorBusinessLogic, _vendorServiceBusinessLogic, this, model);
        }
       
        public async Task HideVendorServiceForm(object obj)
        {
            _parentViewModel.CurrentChildView = this;

            await Task.Run(GetVendorServices).ConfigureAwait(true);
        }
        private async Task ShowVendorServicesForm(VendorServiceModel model)
        {
            _parentViewModel.CurrentChildView = new AddProductServicesViewModel(_vendorBusinessLogic,_vendorServiceBusinessLogic, this, SelectedVendorService); 
        }
        private async Task GetVendorServices()
         {
            var vendors = await _vendorServiceBusinessLogic.GetAllVendorServices().ConfigureAwait(true);
            VendorsServices = TempVendorServices = new ObservableCollection<VendorServiceModel>(vendors);
        }
    }
}
