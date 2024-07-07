using BusinessLogic.Abstraction.VMA.Contract;
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
        private IVendorPaymentBusinessLogic _vendorPaymentBusinessLogic;
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

                    VendorsPayment = new ObservableCollection<VendorPaymentModel>(TempVendorsPayment.Where(x => propertyInfo?.GetValue(x, null)?
                                                                                      .ToString()?
                                                                                      .ToLower(System.Globalization.CultureInfo.CurrentCulture)
                                                                                      .Contains(value, StringComparison.CurrentCultureIgnoreCase) ?? false));
                }
                else
                {
                    VendorsPayment = TempVendorsPayment;
                }

                OnPropertyChanged(nameof(SearchValue));
            }
        }

        #region Observable collections
        private ObservableCollection<VendorPaymentModel> _vendorsPayment;
        private ObservableCollection<VendorPaymentModel> _tempvendorsPayment;
        private ObservableCollection<SearchModel> _comboItem;

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
        public PaymentsViewModel(MainViewModel parentViewModel, IVendorPaymentBusinessLogic vendorPaymentBusinessLogic)
        {
            ComboItem =
            [
               new(){NameSearch="Vendor Code need to chnage",SearchId=1},
                new(){NameSearch="Vendor Name",SearchId=2},
                new(){NameSearch="Vendor Services Name",SearchId=3}
            ];
            _vendorPaymentBusinessLogic = vendorPaymentBusinessLogic;
            _parentViewModel = parentViewModel;
            AddShowVendorFormCommand = new ViewModelAsyncCommand<VendorPaymentModel>(ShowPaymentForm);
            HideVendorFormCommand = new ViewModelAsyncCommand<VendorPaymentModel>(HidePaymentForm);
            EditVendorCommand = new ViewModelAsyncCommand<VendorPaymentModel>(EditPayment);
            _=GetVendorPayments();
        }

        private async Task EditPayment(VendorPaymentModel model)
        {
            
        }

        public async Task HidePaymentForm(object model)
        {
            _parentViewModel.CurrentChildView = this;
        }

        private async Task ShowPaymentForm(object model)
        {
            _parentViewModel.CurrentChildView = new AddPaymentsViewModel(this);
        }

        private async Task GetVendorPayments()
        {
            var vendors = await _vendorPaymentBusinessLogic.GetAllVendorPayment().ConfigureAwait(true);
            VendorsPayment = TempVendorsPayment = new ObservableCollection<VendorPaymentModel>(vendors);
        }
    }
}
