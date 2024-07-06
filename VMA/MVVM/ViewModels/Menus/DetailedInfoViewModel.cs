using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.Abstraction.VMA.Models;
using Database.VMA.Entities;
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

namespace VMA.MVVM.ViewModels.Menus
{
    public class DetailedInfoViewModel : ViewModelBase
    {
        private readonly MainViewModel _parentViewModel;
        private VendorDetailModel _selectedVendor;
        private SearchModel _selectComboItem;
        private string _searchValue;
        private readonly IVendorDetailsBusinessLogic _vendorDetailsBusinessLogic;
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

                    VendorsDetails = new ObservableCollection<VendorDetailModel>(TempVendorsDetails.Where(x => propertyInfo?.GetValue(x, null)?
                                                                                      .ToString()?
                                                                                      .ToLower(System.Globalization.CultureInfo.CurrentCulture)
                                                                                      .Contains(value, StringComparison.CurrentCultureIgnoreCase) ?? false));
                }
                else
                {
                    VendorsDetails = TempVendorsDetails;
                }

                OnPropertyChanged(nameof(SearchValue));
            }
        }

        #region Observable collections
        private ObservableCollection<VendorDetailModel> _vendorsDetails;
        private ObservableCollection<VendorDetailModel> _tempvendorsDetails;
        private ObservableCollection<SearchModel> _comboItem;

        public ObservableCollection<VendorDetailModel> VendorsDetails
        {
            get { return _vendorsDetails; }
            set
            {
                _vendorsDetails = value;
                OnPropertyChanged(nameof(VendorsDetails));
            }
        }

        public ObservableCollection<VendorDetailModel> TempVendorsDetails
        {
            get { return _tempvendorsDetails; }
            set
            {
                _tempvendorsDetails = value;
                OnPropertyChanged(nameof(TempVendorsDetails));
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
        public DetailedInfoViewModel(MainViewModel parentViewModel, IVendorDetailsBusinessLogic vendorDetailsBusinessLogic)
        {
            _vendorDetailsBusinessLogic= vendorDetailsBusinessLogic;
            _parentViewModel = parentViewModel;
            AddShowDetailInfoFormCommand = new ViewModelAsyncCommand<VendorDetailModel>(ShowDetailsInfoForm);
            HideDetailInfoFormCommand = new ViewModelAsyncCommand<VendorDetailModel>(HideDetailInfoForm);
            EditDetailInfoCommand = new ViewModelAsyncCommand<VendorDetailModel>(EditDetailInfoForm);
            GetDetailsVendorServices();
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

        private async Task GetDetailsVendorServices()
        {
            var vendors = await _vendorDetailsBusinessLogic.GetAllVendorDetails().ConfigureAwait(true);
            VendorsDetails = TempVendorsDetails = new ObservableCollection<VendorDetailModel>(vendors);
        }
    }
}
