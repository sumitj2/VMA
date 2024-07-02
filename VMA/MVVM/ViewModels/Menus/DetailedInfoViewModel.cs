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
    public class DetailedInfoViewModel : ViewModelBase
    {
        private readonly MainViewModel _parentViewModel;
        private VendorDetailModel _selectedVendor;
        private SearchModel _selectComboItem;
        private string _searchValue;

        public SearchModel SelectComboItem
        {
            get { return _selectComboItem; }
            set { _selectComboItem = value; }
        }
        public VendorDetailModel SelectedVendorService
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
                    PropertyInfo? propertyInfo = typeof(VendorDetailModel)?.GetProperty(SelectComboItem.NameSearch.Replace(" ", ""));

                    Vendors = new ObservableCollection<VendorDetailModel>(TempVendors.Where(x => propertyInfo?.GetValue(x, null)?
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
        private ObservableCollection<VendorDetailModel> _vendors;
        private ObservableCollection<VendorDetailModel> _tempvendors;
        private ObservableCollection<SearchModel> _comboItem;

        public ObservableCollection<VendorDetailModel> Vendors
        {
            get { return _vendors; }
            set
            {
                _vendors = value;
                OnPropertyChanged(nameof(Vendors));
            }
        }

        public ObservableCollection<VendorDetailModel> TempVendors
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

        public ICommand AddShowDetailInfoFormCommand { get; }

        public ICommand UpdateDetailInfoFormCommand { get; }
        public ICommand HideDetailInfoFormCommand { get; }

        public ICommand EditDetailInfoCommand { get; }
        #endregion
        public DetailedInfoViewModel(MainViewModel parentViewModel)
        {
            _parentViewModel = parentViewModel;
            AddShowDetailInfoFormCommand = new ViewModelAsyncCommand<VendorDetailModel>(ShowDetailsInfoForm);
            HideDetailInfoFormCommand = new ViewModelAsyncCommand<VendorDetailModel>(HideDetailInfoForm);
            EditDetailInfoCommand = new ViewModelAsyncCommand<VendorDetailModel>(EditDetailInfoForm);
        }

        private async Task ShowDetailsInfoForm(VendorDetailModel model)
        {
            _parentViewModel.CurrentChildView = new AddDetailedInfoViewModel(this);
        }

        private async Task EditDetailInfoForm(VendorDetailModel model)
        {
            throw new NotImplementedException();
        }

        public async Task HideDetailInfoForm(object obj)
        {
            _parentViewModel.CurrentChildView = this;
        }
    }
}
