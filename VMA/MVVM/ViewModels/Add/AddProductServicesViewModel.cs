using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.Abstraction.VMA.Models;
using Database.VMA.Entities;
using Database.VMA.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VMA.MVVM.Models;
using VMA.MVVM.ViewModels.Menus;

namespace VMA.MVVM.ViewModels.Add
{
    public class AddProductServicesViewModel : ViewModelBase
    {
        private readonly IVendorBusinessLogic _vendorBusinessLogic;
        private readonly IVendorServiceBusinessLogic _vendorServiceBusinessLogic;
        private readonly ProductServicesViewModel productServicesViewModel;

        #region Command

        public ICommand HideVendorProductServiceFormCommand { get; }
        public ICommand SubmitCommand { get; }
        public ICommand ClearFormCommand { get; }

        #endregion

        #region Properties
        private string _vendorCode;
        private string _vendorName;
        private string _vendorServiceName;
        private string _saveButtonName;

        public VendorServiceModel SelectedProductVendorService { get; set; }
        public string SaveButtonName
        {
            get => _saveButtonName;

            set
            {
                _saveButtonName = value;
                OnPropertyChanged(nameof(SaveButtonName));
            }
        }
        public string VendorCode
        {
            get
            {
                return _vendorCode;
            }
            set
            {
                _vendorCode = value;
                OnPropertyChanged(nameof(VendorCode));
            }
        }

        public string VendorName
        {
            get
            {
                return _vendorName;
            }
            set
            {
                _vendorName = value;
                OnPropertyChanged(nameof(VendorName));
            }
        }

        public string VendorServiceName
        {
            get
            {
                return _vendorServiceName;
            }
            set
            {
                _vendorServiceName = value;
                OnPropertyChanged(nameof(VendorServiceName));
            }
        }

        private VendorModel _selectedVendor;
        public VendorModel SelectedVendor
        {
            get { return _selectedVendor; }
            set
            {

                _selectedVendor = value;
                OnPropertyChanged("SelectedVendor");

            }
        }



        #endregion


        #region Observable collections
        private ObservableCollection<VendorModel> _vendors;
        private ObservableCollection<SearchModel> _comboItem;

        public ObservableCollection<VendorModel> Vendors
        {
            get { return _vendors; }
            set
            {
                _vendors = value;
                OnPropertyChanged(nameof(Vendors));
            }
        }



        public ObservableCollection<SearchModel> ComboItem
        {
            get { return _comboItem; }
            set { _comboItem = value; }
        }
        #endregion

        public AddProductServicesViewModel(IVendorBusinessLogic vendorBusinessLogic, IVendorServiceBusinessLogic vendorServiceBusinessLogic, ProductServicesViewModel parentViewModel, VendorServiceModel SelectedVendorService)
        {
            this.SelectedProductVendorService = SelectedVendorService;
            if (SelectedVendorService != null)
            {
                SaveButtonName = "Update";
            }
            else
            {
                SaveButtonName = "Submit";
            }
           
            productServicesViewModel = parentViewModel;
            _vendorServiceBusinessLogic = vendorServiceBusinessLogic;
            _vendorBusinessLogic = vendorBusinessLogic;
          
            HideVendorProductServiceFormCommand = new ViewModelAsyncCommand<VendorServiceModel>(HideVendorServiceForm);
            SubmitCommand = new ViewModelAsyncCommand<VendorServiceModel>(SaveVendorService);
            ClearFormCommand = new ViewModelAsyncCommand<VendorServiceModel>(ClearValues);

            CallAync();
        }

        private async void CallAync()
        {
            await ManinTasss();
        }

        private async Task ClearValues(VendorServiceModel model)
        {
            throw new NotImplementedException();
        }

        private async Task SaveVendorService(object obj)
        {


            if (SaveButtonName == "Update")
            {
                VendorServiceModel model = new VendorServiceModel()
                {
                    VendorId = SelectedVendor.VendorId,
                    VendorServiceName = VendorServiceName,
                    CreatedBy = UserAccountModel.Username,
                };
                await _vendorServiceBusinessLogic.EditUpdateVendorService(model);

                SuccessPopupViewModel.Instance.ShowPopup(Enums.NotificationType.Success, "Data Updated Successfully");
            }
            else
            {
                VendorServiceModel model = new VendorServiceModel()
                {
                    VendorId = SelectedVendor.VendorId,
                    VendorServiceName = VendorServiceName,
                    CreatedBy = UserAccountModel.Username,
                    IsActive = true,
                    FkVendorId = SelectedVendor.VendorId,
                };
                await _vendorServiceBusinessLogic.AddVendorService(model);


                SuccessPopupViewModel.Instance.ShowPopup(Enums.NotificationType.Success, "Data Added Successfully");
            }

            await HideVendorServiceForm(this);
        }

        private async Task HideVendorServiceForm(object model)
        {
            await productServicesViewModel.HideVendorServiceForm(this);
        }

        private async Task PopulateValues()
        {
            if (SelectedProductVendorService != null)
            {

                VendorCode = SelectedProductVendorService.VendorCode ?? "";
                VendorName = SelectedProductVendorService.VendorName ?? "";
                VendorServiceName = SelectedProductVendorService.VendorServiceName ?? "";
                var vendorID = Vendors.ToList().Find(x => x.VendorId == SelectedProductVendorService.VendorId);
                
                if (vendorID != null)
                {
                    SelectedVendor = Vendors[Vendors.IndexOf(vendorID)];

                }
            }
        }

        private async Task LoadVendors()
        {
            var vendors = await _vendorBusinessLogic.GetAllVendor().ConfigureAwait(true);
           
            Vendors = new ObservableCollection<VendorModel>(vendors);
        }

        public async Task ManinTasss()
        {

            await LoadVendors();
            await PopulateValues();
        }
    }
}
