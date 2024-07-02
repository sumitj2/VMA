using BusinessLogic.Abstraction.VMA.Models;
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

                    Vendors = new ObservableCollection<VenderPaymentNoteModel>(TempVendors.Where(x => propertyInfo?.GetValue(x, null)?
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
        private ObservableCollection<VenderPaymentNoteModel> _vendors;
        private ObservableCollection<VenderPaymentNoteModel> _tempvendors;
        private ObservableCollection<SearchModel> _comboItem;

        public ObservableCollection<VenderPaymentNoteModel> Vendors
        {
            get { return _vendors; }
            set
            {
                _vendors = value;
                OnPropertyChanged(nameof(Vendors));
            }
        }

        public ObservableCollection<VenderPaymentNoteModel> TempVendors
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

        public ICommand AddShowPaymentNoteFormCommand { get; }

        public ICommand UpdateDetailInfoFormCommand { get; }
        public ICommand HidePaymentNotesFormCommand { get; }

        public ICommand EditPaymentNotesFormCommand { get; }
        #endregion
        public PaymentNotesViewModel(MainViewModel parentViewModel)
        {
            _parentViewModel = parentViewModel;
            AddShowPaymentNoteFormCommand = new ViewModelAsyncCommand<VenderPaymentNoteModel>(ShowPaymentNotesForm);
            HidePaymentNotesFormCommand = new ViewModelAsyncCommand<VenderPaymentNoteModel>(HidePaymentNotesForm);
            EditPaymentNotesFormCommand = new ViewModelAsyncCommand<VenderPaymentNoteModel>(EditPaymentNoteForm);
        }

        private async Task EditPaymentNoteForm(VenderPaymentNoteModel model)
        {
            throw new NotImplementedException();
        }

        private async Task ShowPaymentNotesForm(VenderPaymentNoteModel model)
        {
            _parentViewModel.CurrentChildView = new AddPaymentNotesViewModel(this);

        }

        public async Task HidePaymentNotesForm(object obj)
        {
            _parentViewModel.CurrentChildView = this;
        }
    }
}
