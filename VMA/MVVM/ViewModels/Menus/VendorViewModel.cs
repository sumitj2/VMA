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
using VMA.MVVM.ViewModels.Menus.Vendor;
using VMA.MVVM.Views;

namespace VMA.MVVM.ViewModels.Menus
{
    public class VendorViewModel : ViewModelBase
    {
        private readonly IVendorBusinessLogic _vendorBusinessLogic;
        private MainViewModel _parentViewModel;

        //-> Commands
        private ViewModelCommand showVendorFormCommand;
        public ICommand ShowVendorFormCommand
        {
            get
            {
                if (this.showVendorFormCommand == null)
                {
                    this.showVendorFormCommand = new ViewModelCommand(x => ShowVendorForm());

                }
                return this.showVendorFormCommand;
            }
        }

        private ViewModelCommand hideVendorFormCommand;
        public ICommand HideVendorFormCommand
        {
            get
            {
                if (this.hideVendorFormCommand == null)
                {
                    this.hideVendorFormCommand = new ViewModelCommand(x => HideVendorForm());

                }
                return this.hideVendorFormCommand;
            }
        }

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

        public VendorViewModel(IVendorBusinessLogic vendorBusinessLogic, MainViewModel parentViewModel)
        {
            _vendorBusinessLogic = vendorBusinessLogic;
            _parentViewModel = parentViewModel;
            getVendors();
        }

        private async Task getVendors()
        {
            var vendors = await _vendorBusinessLogic.GetAllVendor();
            Vendors = new ObservableCollection<VendorModel>(vendors);
        }

        private void ShowVendorForm()
        {
           _parentViewModel.CurrentChildView = new VendorDetailsViewModel(_vendorBusinessLogic,this);
        }

        public void HideVendorForm()
        {
            _parentViewModel.CurrentChildView = this;
        }
    }
}
