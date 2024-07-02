using BusinessLogic.Abstraction.VMA.Models;
using Database.VMA.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VMA.MVVM.ViewModels.Add;

namespace VMA.MVVM.ViewModels.Menus
{
    public class PaymentsViewModel : ViewModelBase
    {
        private readonly MainViewModel _parentViewModel;
        private VendorPaymentModel _selectedVendor;
        private SearchModel _selectComboItem;
        private string _searchValue;

        public SearchModel SelectComboItem
        {
            get { return _selectComboItem; }
            set { _selectComboItem = value; }
        }
        public VendorPaymentModel SelectedVendorService
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
                    PropertyInfo? propertyInfo = typeof(VendorPaymentModel)?.GetProperty(SelectComboItem.NameSearch.Replace(" ", ""));

                    Vendors = new ObservableCollection<VendorPaymentModel>(TempVendors.Where(x => propertyInfo?.GetValue(x, null)?
                                                                                      .ToString()?
                                                                                      .ToLower(System.Globalization.CultureInfo.CurrentCulture)
                                                                                      .Contains(value, StringComparison.CurrentCultureIgnoreCase) ?? false));
                }
                else
                {
                    Vendors = TempVendors;
                }

                OnPropertyChanged(nameof(SearchValue));
            }
        }

        #region Observable collections
        private ObservableCollection<VendorPaymentModel> _vendors;
        private ObservableCollection<VendorPaymentModel> _tempvendors;
        private ObservableCollection<SearchModel> _comboItem;

        public ObservableCollection<VendorPaymentModel> Vendors
        {
            get { return _vendors; }
            set
            {
                _vendors = value;
                OnPropertyChanged(nameof(Vendors));
            }
        }

        public ObservableCollection<VendorPaymentModel> TempVendors
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
        public PaymentsViewModel(MainViewModel parentViewModel)
        {
            _parentViewModel = parentViewModel;
            AddShowVendorFormCommand = new ViewModelAsyncCommand<VendorPaymentModel>(ShowPaymentForm);
            HideVendorFormCommand = new ViewModelAsyncCommand<VendorPaymentModel>(HidePaymentForm);
            EditVendorCommand = new ViewModelAsyncCommand<VendorPaymentModel>(EditPayment);
        }

        private async Task EditPayment(VendorPaymentModel model)
        {
            throw new NotImplementedException();
        }

        public async Task HidePaymentForm(object model)
        {
            _parentViewModel.CurrentChildView = this;
        }

        private async Task ShowPaymentForm(object  model)
        {
            _parentViewModel.CurrentChildView = new AddPaymentsViewModel(this);
        }
    }
}
