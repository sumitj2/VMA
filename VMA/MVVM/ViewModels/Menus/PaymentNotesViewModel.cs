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
    public class PaymentNotesViewModel : ViewModelBase
    {
        private readonly IVendorDetailsBusinessLogic _vendorDetailsBusinessLogic;
        private readonly IVenderPaymentNotesBusinessLogic _venderPaymentNotesBusinessLogic;
        private readonly IVendorBusinessLogic _vendorBusinessLogic;
        private readonly MainViewModel _parentViewModel;
        private VenderPaymentNoteModel _selectedVendor;
        private SearchModel _selectComboItem;
        private string _searchValue;

        public SearchModel SelectComboItem
        {
            get { return _selectComboItem; }
            set { _selectComboItem = value; }
        }
        public VenderPaymentNoteModel SelectedVendorService
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
                    PropertyInfo? propertyInfo = typeof(VenderPaymentNoteModel)?.GetProperty(SelectComboItem.NameSearch.Replace(" ", ""));

                    VendorPaymentNotes = new ObservableCollection<VenderPaymentNoteModel>(TempVendorPaymentNotes.Where(x => propertyInfo?.GetValue(x, null)?
                                                                                      .ToString()?
                                                                                      .ToLower(System.Globalization.CultureInfo.CurrentCulture)
                                                                                      .Contains(value, StringComparison.CurrentCultureIgnoreCase) ?? false));
                }
                else
                {
                    VendorPaymentNotes = TempVendorPaymentNotes;
                }

                OnPropertyChanged(nameof(SearchValue));
            }
        }

        #region Observable collections
        private ObservableCollection<VenderPaymentNoteModel> _vendors;
        private ObservableCollection<VenderPaymentNoteModel> _tempvendors;
        private ObservableCollection<SearchModel> _comboItem;

        public ObservableCollection<VenderPaymentNoteModel> VendorPaymentNotes
        {
            get { return _vendors; }
            set
            {
                _vendors = value;
                OnPropertyChanged(nameof(VendorPaymentNotes));
            }
        }

        public ObservableCollection<VenderPaymentNoteModel> TempVendorPaymentNotes
        {
            get { return _tempvendors; }
            set
            {
                _tempvendors = value;
                OnPropertyChanged(nameof(TempVendorPaymentNotes));
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

                    Type type = typeof(VenderPaymentNoteModel);

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

        public ICommand AddShowPaymentNoteFormCommand { get; }

        public ICommand UpdateDetailInfoFormCommand { get; }
        public ICommand HidePaymentNotesFormCommand { get; }

        public ICommand EditPaymentNotesFormCommand { get; }
        #endregion

        private readonly IConfigurationBusinessLogic _configurationBusinessLogic;
        public PaymentNotesViewModel(MainViewModel parentViewModel,IVenderPaymentNotesBusinessLogic venderPaymentNotesBusinessLogic, IVendorDetailsBusinessLogic vendorDetailsBusinessLogic, IVendorBusinessLogic vendorBusinessLogic,IConfigurationBusinessLogic configurationBusinessLogic)
        {
            Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Into the constructor", this.GetType().Name, MethodBase.GetCurrentMethod().Name));

            _configurationBusinessLogic = configurationBusinessLogic;
            _venderPaymentNotesBusinessLogic=venderPaymentNotesBusinessLogic;
            _vendorDetailsBusinessLogic=vendorDetailsBusinessLogic; 
            _vendorBusinessLogic=vendorBusinessLogic;
            _parentViewModel = parentViewModel;
            AddShowPaymentNoteFormCommand = new ViewModelAsyncCommand<VenderPaymentNoteModel>(ShowPaymentNotesForm);
            HidePaymentNotesFormCommand = new ViewModelAsyncCommand<VenderPaymentNoteModel>(HidePaymentNotesForm);
            EditPaymentNotesFormCommand = new ViewModelAsyncCommand<VenderPaymentNoteModel>(EditPaymentNoteForm);
            _=GetPaymentsNote();
        }
        private async Task EditPaymentNoteForm(VenderPaymentNoteModel model)
        {
            SuccessPopupViewModel.Instance.ShowPopup(Enums.NotificationType.Alert, "Please wait...", true);
            _parentViewModel.CurrentChildView = new AddPaymentNotesViewModel(this, _vendorDetailsBusinessLogic,_vendorBusinessLogic,_venderPaymentNotesBusinessLogic,model, _configurationBusinessLogic);
        }
        
        private async Task ShowPaymentNotesForm(VenderPaymentNoteModel model)
        {
            _parentViewModel.CurrentChildView = new AddPaymentNotesViewModel(this,_vendorDetailsBusinessLogic,_vendorBusinessLogic,_venderPaymentNotesBusinessLogic,null, _configurationBusinessLogic);

        }

        public async Task HidePaymentNotesForm(object obj)
        {
            _parentViewModel.CurrentChildView = this;
            await Task.Run(GetPaymentsNote).ConfigureAwait(true);
        }

        private async Task GetPaymentsNote()
        {
            var paymentNotes = await _venderPaymentNotesBusinessLogic.GetAllPaymentNotes().ConfigureAwait(true);
            VendorPaymentNotes = TempVendorPaymentNotes = new ObservableCollection<VenderPaymentNoteModel>(paymentNotes);
        }
    }
}
