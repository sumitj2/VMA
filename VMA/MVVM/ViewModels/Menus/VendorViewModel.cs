using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.Abstraction.VMA.Models;
using Serilog;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Input;
using VMA.MVVM.ViewModels.Add;

namespace VMA.MVVM.ViewModels.Menus
{
    public class VendorViewModel : ViewModelBase
    {
        private readonly IVendorBusinessLogic _vendorBusinessLogic;
        private readonly MainViewModel _parentViewModel;
        private VendorModel _selectedVendor;
        private SearchModel _selectComboItem;
        private string _searchValue;


        public SearchModel SelectComboItem
        {
            get { return _selectComboItem; }
            set { _selectComboItem = value; }
        }
        public VendorModel SelectedVendor
        {
            get { return _selectedVendor; }
            set { _selectedVendor = value; OnPropertyChanged(nameof(SelectedVendor)); }
        }

        public string SearchValue
        {
            get { return _searchValue; }
            set
            {
                _searchValue = value;

                if (SelectComboItem != null && !string.IsNullOrEmpty(value))
                {
                    PropertyInfo? propertyInfo = typeof(VendorModel)?.GetProperty(SelectComboItem.NameSearch);

                    Vendors = new ObservableCollection<VendorModel>(TempVendors.Where(x => propertyInfo?.GetValue(x, null)?.ToString()?.ToLower(System.Globalization.CultureInfo.CurrentCulture).Contains(value, StringComparison.CurrentCultureIgnoreCase) ?? false));
                }
                else
                {
                    Vendors = TempVendors;
                }

                OnPropertyChanged(nameof(SearchValue));
            }
        }

        #region Observable collections
        private ObservableCollection<VendorModel> _vendors;
        private ObservableCollection<VendorModel> _tempvendors;
        private ObservableCollection<SearchModel> _comboItem;

        public ObservableCollection<VendorModel> Vendors
        {
            get { return _vendors; }
            set
            {
                _vendors = value;
                OnPropertyChanged(nameof(Vendors));
            }
        }

        public ObservableCollection<VendorModel> TempVendors
        {
            get { return _tempvendors; }
            set
            {
                _tempvendors = value;
                OnPropertyChanged(nameof(TempVendors));
            }
        }

        public ObservableCollection<SearchModel> ComboItem
        {
            get
            {
                if (_comboItem == null)
                {
                    List<string> skip = new List<string>() { "CreatedBy", "CreatedDate", "LastUpdateBy", "LastUpdatedDate" };
                    _comboItem = new ObservableCollection<SearchModel>();

                    Type type = typeof(VendorModel);

                    PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                    int id = 1;

                    foreach (PropertyInfo property in properties.Where(x => x.PropertyType == typeof(String)))
                    {
                        if (!skip.Contains(property.Name))
                            _comboItem.Add(new SearchModel() { NameSearch = property.Name, SearchId = id });
                    }
                }

                return _comboItem;
            }
        }
        #endregion

        #region commands

        public ICommand AddShowVendorFormCommand { get; }

        public ICommand UpdateVendorFormCommand { get; }
        public ICommand HideVendorFormCommand { get; }

        public ICommand EditVendorCommand { get; }
        #endregion

        public VendorViewModel(IVendorBusinessLogic vendorBusinessLogic, MainViewModel parentViewModel)
        {
            Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Into the constructor", this.GetType().Name, MethodBase.GetCurrentMethod().Name));

            _vendorBusinessLogic = vendorBusinessLogic;
            _parentViewModel = parentViewModel;
            AddShowVendorFormCommand = new ViewModelCommand(ShowVendorForm);
            HideVendorFormCommand = new ViewModelAsyncCommand<VendorModel>(HideVendorForm);
            EditVendorCommand = new ViewModelAsyncCommand<VendorModel>(EditVendor);
            _ = GetVendors();
        }

        public async Task HideVendorForm(object obj)
        {
            _parentViewModel.CurrentChildView = this;

            await Task.Run(GetVendors).ConfigureAwait(true);
        }

        private async Task EditVendor(object obj)
        {
            SuccessPopupViewModel.Instance.ShowPopup(Enums.NotificationType.Alert, "Please wait...",true);

            _parentViewModel.CurrentChildView = new AddUpdateVendorViewModel(_vendorBusinessLogic, this, (VendorModel)obj);
        }

        private async Task GetVendors()
        {
            try
            {
                Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Getting vendors", this.GetType().Name, MethodBase.GetCurrentMethod().Name));

                var vendors = await _vendorBusinessLogic.GetAllVendor().ConfigureAwait(true);
                Vendors = TempVendors = new ObservableCollection<VendorModel>(vendors);

                Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Retrieved vendors", this.GetType().Name, MethodBase.GetCurrentMethod().Name));
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex,string.Format("Class: {0}, Method: {1} - Failed to get vendors", this.GetType().Name, MethodBase.GetCurrentMethod().Name));
            }
        }

        private void ShowVendorForm(object obj)
        {
            _parentViewModel.CurrentChildView = new AddUpdateVendorViewModel(_vendorBusinessLogic, this, SelectedVendor);
        }

    }
}
