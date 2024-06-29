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

        private ObservableCollection<VendorModel> _vendors;
        public ObservableCollection<VendorModel> Vendors
        {
            get { return _vendors; }
            set
            {
                _vendors = value;
                OnPropertyChanged(nameof(Vendors));
            }
        }

        private ObservableCollection<VendorModel> _tempvendors;
        public ObservableCollection<VendorModel> TempVendors
        {
            get { return _tempvendors; }
            set
            {
                _tempvendors = value;
                OnPropertyChanged(nameof(TempVendors));
            }
        }

        private ObservableCollection<SearchModel> _comboItem;

        public ObservableCollection<SearchModel> ComboItem
        {
            get { return _comboItem; }
            set { _comboItem = value; }
        }

        private SearchModel _selectComboItem;

        public SearchModel selectComboItem
        {
            get { return _selectComboItem; }
            set { _selectComboItem = value; }
        }

        private VendorModel _selectedVendor;

        public VendorModel SelectedVendor
        {
            get { return _selectedVendor; }
            set { _selectedVendor = value; OnPropertyChanged(nameof(SelectedVendor)); }
        }

        private string _searchValue;

        public string SearchValue
        {
            get { return _searchValue; }
            set { _searchValue = value;

                if (selectComboItem != null && !string.IsNullOrEmpty(value))
                {
                    PropertyInfo propertyInfo = typeof(VendorModel).GetProperty(selectComboItem.NameSearch.Replace(" ",""));

                    Vendors = new ObservableCollection<VendorModel>(Vendors.Where(x => propertyInfo.GetValue(x, null)?.ToString().ToLower().StartsWith(value.ToLower()) ?? false));
                }
                else
                {
                    Vendors = TempVendors;
                }

                OnPropertyChanged(nameof(SearchValue)); }
        }


        // Commands
        public ICommand AddShowVendorFormCommand { get; }

        public ICommand UpdateVendorFormCommand { get; }
        public ICommand HideVendorFormCommand { get; }

        public ICommand EditVendorCommand {  get; }

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
            HideVendorFormCommand = new ViewModelCommand(HideVendorForm);
            EditVendorCommand = new ViewModelCommand(EditVendor);
            _ = GetVendors();
        }

        private void EditVendor(object obj)
        {
            _parentViewModel.CurrentChildView = new AddUpdateVendorViewModel(_vendorBusinessLogic, this, (VendorModel)obj);
        }

        private async Task GetVendors()
        {
            var vendors = await _vendorBusinessLogic.GetAllVendor();
            Vendors = TempVendors = new ObservableCollection<VendorModel>(vendors);
        }

        private void ShowVendorForm(object obj)
        {
            _parentViewModel.CurrentChildView = new AddUpdateVendorViewModel(_vendorBusinessLogic, this, SelectedVendor);           
        }

        public void HideVendorForm(object obj)
        {
            _parentViewModel.CurrentChildView = this;
            Thread.Sleep(1000);
            GetVendors();
        }
    }
}
