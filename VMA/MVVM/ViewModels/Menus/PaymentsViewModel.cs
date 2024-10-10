using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.Abstraction.VMA.Models;
using Database.VMA.Entities;
using Database.VMA.Repositories;
using Serilog;
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
        private IVendorDetailsBusinessLogic _vendorDetailsBusinessLogic;
        private IGstcalculationMasterBusinessLogic _gstcalculationMasterBusinessLogic;
        private readonly IVendorBusinessLogic _vendorBusinessLogic;
        private readonly IVenderPaymentNotesBusinessLogic _venderPaymentNotesBusinessLogic;
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
            get
            {
                if (_comboItem == null)
                {
                    List<string> skip = new List<string>() { "CreatedBy", "CreatedDate", "LastUpdateBy", "LastUpdatedDate","PaymentCode" };
                    _comboItem = new ObservableCollection<SearchModel>();

                    Type type = typeof(VendorPaymentModel);

                    PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                    int id = 1;

                    foreach (PropertyInfo property in properties.Where(x => x.PropertyType == typeof(String)))
                    {
                        if (!skip.Contains(property.Name))
                        {
                            _comboItem.Add(new SearchModel() { NameSearch = property.Name, SearchId = id });
                        }
                    }
                }

                return _comboItem;
            }
        }
        #endregion

        #region commands

        public ICommand AddShowVendorFormCommand { get; }

        public ICommand UpdateVendorFormCommand { get; }
        public ICommand HidePaymentFormCommand { get; }

        public ICommand EditPaymentCommand { get; }
        #endregion
        private readonly IConfigurationBusinessLogic _configurationBusinessLogic;
        public PaymentsViewModel(MainViewModel parentViewModel, IVendorPaymentBusinessLogic vendorPaymentBusinessLogic, IVendorDetailsBusinessLogic vendorDetailsBusinessLogic, IGstcalculationMasterBusinessLogic gstcalculationMasterBusinessLogic, IVendorBusinessLogic vendorBusinessLogic,IVenderPaymentNotesBusinessLogic venderPaymentNotesBusinessLogic, IConfigurationBusinessLogic configurationBusinessLogic)
        {
            Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Into the constructor", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));

            _configurationBusinessLogic = configurationBusinessLogic;
            _vendorPaymentBusinessLogic = vendorPaymentBusinessLogic;
            _vendorDetailsBusinessLogic = vendorDetailsBusinessLogic;
            _gstcalculationMasterBusinessLogic = gstcalculationMasterBusinessLogic;
            _vendorBusinessLogic = vendorBusinessLogic;
            _venderPaymentNotesBusinessLogic = venderPaymentNotesBusinessLogic;
            _parentViewModel = parentViewModel;
            AddShowVendorFormCommand = new ViewModelAsyncCommand<VendorPaymentModel>(ShowPaymentForm);
            HidePaymentFormCommand = new ViewModelAsyncCommand<VendorPaymentModel>(HidePaymentForm);
            EditPaymentCommand = new ViewModelAsyncCommand<VendorPaymentModel>(EditPayment);
            _ = GetVendorPayments();
        }

        private async Task EditPayment(VendorPaymentModel model)
        {
            SuccessPopupViewModel.Instance.ShowPopup(Enums.NotificationType.Alert, "Please wait...", true);
            _parentViewModel.CurrentChildView = new AddPaymentsViewModel(this,_vendorDetailsBusinessLogic, model, _vendorPaymentBusinessLogic, _gstcalculationMasterBusinessLogic,_vendorBusinessLogic, _venderPaymentNotesBusinessLogic, _configurationBusinessLogic);
        }

        public async Task HidePaymentForm(object model)
        {
            _parentViewModel.CurrentChildView = this;
            await Task.Run(GetVendorPayments).ConfigureAwait(true);
        }

        private async Task ShowPaymentForm(object model)
        {
            _parentViewModel.CurrentChildView = new AddPaymentsViewModel(this, _vendorDetailsBusinessLogic, SelectedVendorService,_vendorPaymentBusinessLogic, _gstcalculationMasterBusinessLogic,_vendorBusinessLogic, _venderPaymentNotesBusinessLogic, _configurationBusinessLogic);
        }

        private async Task GetVendorPayments()
        {
            try
            {
                Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Getting vendor payments", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));

                var vendorsPayment = await _vendorPaymentBusinessLogic.GetAllVendorPayment().ConfigureAwait(true);
                VendorsPayment = TempVendorsPayment = new ObservableCollection<VendorPaymentModel>(vendorsPayment.OrderByDescending(x=>x.CreatedDate).ThenByDescending(x=>x.LastUpdatedDate).ThenByDescending(x=>x.PaymentNoteNo));

                Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Retrieved vendor payments", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, string.Format("Class: {0}, Method: {1} - Failed to get vendor payments", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));
            }
        }
    }
}
