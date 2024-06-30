using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.Abstraction.VMA.Models;
using Database.VMA.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Immutable;
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
                    PropertyInfo? propertyInfo = typeof(VendorModel)?.GetProperty(SelectComboItem.NameSearch.Replace(" ", ""));

                    Vendors = new ObservableCollection<VendorModel>(Vendors.Where(x => propertyInfo?.GetValue(x, null)?.ToString()?.ToLower(System.Globalization.CultureInfo.CurrentCulture).Contains(value, StringComparison.CurrentCultureIgnoreCase) ?? false));
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
            get { return _comboItem; }
            set { _comboItem = value; }
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
            ComboItem =
            [
                new(){NameSearch="Vendor Code",SearchId=1},
                new(){NameSearch="Vendor Name",SearchId=2},

            ];
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
            SuccessPopupViewModel.Instance.ShowPopup(Enums.NotificationType.Alert, "Please wait...");

            _parentViewModel.CurrentChildView = new AddUpdateVendorViewModel(_vendorBusinessLogic, this, (VendorModel)obj);
        }

        private async Task GetVendors()
        {
            var vendors = await _vendorBusinessLogic.GetAllVendor().ConfigureAwait(true);
            Vendors = TempVendors = new ObservableCollection<VendorModel>(vendors);
        }

        private void ShowVendorForm(object obj)
        {
            SuccessPopupViewModel.Instance.ShowPopup(Enums.NotificationType.Warning, "Please wait...");

            _parentViewModel.CurrentChildView = new AddUpdateVendorViewModel(_vendorBusinessLogic, this, SelectedVendor);
        }

    }
}
