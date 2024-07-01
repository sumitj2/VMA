using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.Abstraction.VMA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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
        private string _vendorService;
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

        public string VendorService
        {
            get
            {
                return _vendorService;
            }
            set
            {
                _vendorService = value;
                OnPropertyChanged(nameof(VendorService));
            }
        }
        #endregion
        public AddProductServicesViewModel(IVendorBusinessLogic vendorBusinessLogic,IVendorServiceBusinessLogic vendorServiceBusinessLogic, ProductServicesViewModel parentViewModel, VendorServiceModel SelectedVendorService)
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
           // PopulateValues();
            productServicesViewModel = parentViewModel;
            _vendorServiceBusinessLogic = vendorServiceBusinessLogic;
            _vendorBusinessLogic = vendorBusinessLogic;
            HideVendorProductServiceFormCommand = new ViewModelAsyncCommand<VendorServiceModel>(HideVendorServiceForm);
            SubmitCommand = new ViewModelAsyncCommand<VendorServiceModel>(SaveVendorService);
            ClearFormCommand = new ViewModelAsyncCommand<VendorServiceModel>(ClearValues);
        }

        private async Task ClearValues(VendorServiceModel model)
        {
            throw new NotImplementedException();
        }

        private async Task SaveVendorService(VendorServiceModel model)
        {
            throw new NotImplementedException();
        }

        private async Task HideVendorServiceForm(VendorServiceModel model)
        {
          await  productServicesViewModel.HideVendorServiceForm(this);
        }

        private void PopulateValues()
        {
            throw new NotImplementedException();
        }
    }
}
