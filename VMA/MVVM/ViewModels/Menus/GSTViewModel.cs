using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.Abstraction.VMA.Models;
using Database.VMA.Repositories;
using System;
using System.Buffers;
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
    public class GSTViewModel : ViewModelBase
    {
        private readonly MainViewModel _parentViewModel;
        private readonly IGstcalculationMasterBusinessLogic _gstcalculationMasterBusinessLogic;

        public GSTViewModel(IGstcalculationMasterBusinessLogic gstcalculationMasterBusinessLogic, MainViewModel parentViewModel)
        {
            _parentViewModel = parentViewModel;
            _gstcalculationMasterBusinessLogic = gstcalculationMasterBusinessLogic;
            AddShowGSTFormCommand = new ViewModelAsyncCommand<GstcalculationMasterModel>(ShowGSTForm);
            HideGSTFormCommand = new ViewModelAsyncCommand<GstcalculationMasterModel>(HideGSTForm);
            EditGSTCommand = new ViewModelAsyncCommand<GstcalculationMasterModel>(EditGST);
            _ = GetGSTMaster();
        }
        private SearchModel _selectComboItem;
        public SearchModel SelectComboItem
        {
            get { return _selectComboItem; }
            set { _selectComboItem = value; }
        }

        private GstcalculationMasterModel _selectedVendor;
        public GstcalculationMasterModel SelectedVendor
        {
            get { return _selectedVendor; }
            set { _selectedVendor = value; OnPropertyChanged(nameof(SelectedVendor)); }
        }

        private string _searchValue;
        public string SearchValue
        {
            get { return _searchValue; }
            set
            {
                _searchValue = value;

                if (SelectComboItem != null && !string.IsNullOrEmpty(value))
                {
                    PropertyInfo? propertyInfo = typeof(GstcalculationMasterModel)?.GetProperty(SelectComboItem.NameSearch.Replace(" ", ""));

                    GSTMaster = new ObservableCollection<GstcalculationMasterModel>(TempGSTMaster.Where(x => propertyInfo?.GetValue(x, null)?.ToString()?.ToLower(System.Globalization.CultureInfo.CurrentCulture).Contains(value, StringComparison.CurrentCultureIgnoreCase) ?? false));
                }
                else
                {
                    GSTMaster = TempGSTMaster;
                }

                OnPropertyChanged(nameof(SearchValue));
            }
        }

        #region Observable collections
        private ObservableCollection<GstcalculationMasterModel> _vendors;
        private ObservableCollection<GstcalculationMasterModel> _tempvendors;
        private ObservableCollection<SearchModel> _comboItem;

        public ObservableCollection<GstcalculationMasterModel> GSTMaster
        {
            get { return _vendors; }
            set
            {
                _vendors = value;
                OnPropertyChanged(nameof(GSTMaster));
            }
        }

        public ObservableCollection<GstcalculationMasterModel> TempGSTMaster
        {
            get { return _tempvendors; }
            set
            {
                _tempvendors = value;
                OnPropertyChanged(nameof(TempGSTMaster));
            }
        }

        public ObservableCollection<SearchModel> ComboItem
        {
            get { return _comboItem; }
            set { _comboItem = value; }
        }
        #endregion

        private Task ShowGSTForm(GstcalculationMasterModel model)
        {
            _parentViewModel.CurrentChildView = new AddUpdateGSTMasterViewModel(_gstcalculationMasterBusinessLogic,this);
            return Task.CompletedTask;
        }

        private async Task EditGST(GstcalculationMasterModel model)
        {
            
        }

        public Task HideGSTForm(object model)
        {
            _parentViewModel.CurrentChildView = this;
            return Task.CompletedTask;
        }
        private async Task GetGSTMaster()
        {
            var vendors = await _gstcalculationMasterBusinessLogic.GetAllGstMaster().ConfigureAwait(true);
            GSTMaster = TempGSTMaster = new ObservableCollection<GstcalculationMasterModel>(vendors);
        }

        #region commands

        public ICommand AddShowGSTFormCommand { get; }

        public ICommand UpdateGSTFormCommand { get; }
        public ICommand HideGSTFormCommand { get; }

        public ICommand EditGSTCommand { get; }
        #endregion
    }
}
