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
        private readonly IVendorServiceBusinessLogic _vendorServiceBusinessLogic;
        private readonly IVendorBusinessLogic _vendorBusinessLogic;
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

                    VendorServiceDetails = new ObservableCollection<VendorDetailModel>(TempVendorServiceDetails.Where(x => propertyInfo?.GetValue(x, null)?
                                                                                      .ToString()?
                                                                                      .ToLower(System.Globalization.CultureInfo.CurrentCulture)
                                                                                      .Contains(value, StringComparison.CurrentCultureIgnoreCase) ?? false));
                }
                else
                {
                    VendorServiceDetails = TempVendorServiceDetails;
                }

                OnPropertyChanged(nameof(SearchValue));
            }
        }

        #region Observable collections
        private ObservableCollection<VendorDetailModel> _vendorServiceDetails;
        private ObservableCollection<VendorDetailModel> _tempvendorServiceDetails;
        private ObservableCollection<SearchModel> _comboItem;

        public ObservableCollection<VendorDetailModel> VendorServiceDetails
        {
            get { return _vendorServiceDetails; }
            set
            {
                _vendorServiceDetails = value;
                OnPropertyChanged(nameof(VendorServiceDetails));
            }
        }

        public ObservableCollection<VendorDetailModel> TempVendorServiceDetails
        {
            get { return _tempvendorServiceDetails; }
            set
            {
                _tempvendorServiceDetails = value;
                OnPropertyChanged(nameof(TempVendorServiceDetails));
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

                    Type type = typeof(VendorDetailModel);

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

        public ICommand AddShowDetailInfoFormCommand { get; }

        public ICommand UpdateDetailInfoFormCommand { get; }
        public ICommand HideDetailInfoFormCommand { get; }

        public ICommand EditDetailInfoCommand { get; }
        #endregion
        public DetailedInfoViewModel(MainViewModel parentViewModel, IVendorDetailsBusinessLogic vendorDetailsBusinessLogic, IVendorServiceBusinessLogic vendorServiceBusinessLogic, IVendorBusinessLogic vendorBusinessLogic)
        {
            _vendorDetailsBusinessLogic = vendorDetailsBusinessLogic;
            _vendorServiceBusinessLogic= vendorServiceBusinessLogic;
            _vendorBusinessLogic= vendorBusinessLogic;  
            _parentViewModel = parentViewModel;
            AddShowDetailInfoFormCommand = new ViewModelAsyncCommand<VendorDetailModel>(ShowDetailsInfoForm);
            HideDetailInfoFormCommand = new ViewModelAsyncCommand<VendorDetailModel>(HideDetailInfoForm);
            EditDetailInfoCommand = new ViewModelAsyncCommand<VendorDetailModel>(EditDetailInfoForm);
            _=GetDetailsVendorServices();
        }

        private Task ShowDetailsInfoForm(VendorDetailModel model)
        {
            _parentViewModel.CurrentChildView = new AddDetailedInfoViewModel(this,SelectedVendorService, _vendorDetailsBusinessLogic, _vendorServiceBusinessLogic,_vendorBusinessLogic, VendorServiceDetails);
            return Task.CompletedTask;
        }

        private Task EditDetailInfoForm(VendorDetailModel model)
        {
            _parentViewModel.CurrentChildView = new AddDetailedInfoViewModel(this, model, _vendorDetailsBusinessLogic, _vendorServiceBusinessLogic,_vendorBusinessLogic,null);
            return Task.CompletedTask;
        }

        public async Task HideDetailInfoForm(object obj)
        {
            _parentViewModel.CurrentChildView = this;
            await Task.Run(GetDetailsVendorServices).ConfigureAwait(true);           
        }

        private async Task GetDetailsVendorServices()
        {
            var details = await _vendorDetailsBusinessLogic.GetAllVendorDetails().ConfigureAwait(true);
            VendorServiceDetails = TempVendorServiceDetails = new ObservableCollection<VendorDetailModel>(details);
        }
    }
}
