using BusinessLogic.Abstraction.VMA.Contract;
using Database.VMA.Entities;
using Database.VMA.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VMA.MVVM.ViewModels.Menus
{
    public class VendorViewModel : ViewModelBase
    {
        private readonly IVendorBusinessLogic _vendorBusinessLogic;

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

        private bool _isVendorFormVisible;
        public bool IsVendorFormVisible
        {
            get { return _isVendorFormVisible; }
            set
            {
                _isVendorFormVisible = value;
                OnPropertyChanged(nameof(IsVendorFormVisible));
            }
        }

        private ObservableCollection<Vendor> _vendors;
        public ObservableCollection<Vendor> Vendors
        {
            get { return _vendors; }
            set
            {
                _vendors = value;
                OnPropertyChanged(nameof(Vendors));
            }
        }

        public VendorViewModel(IVendorBusinessLogic vendorBusinessLogic)
        {
            _vendorBusinessLogic = vendorBusinessLogic;
            getVendors();
        }

        private async Task getVendors()
        {
            var vendors = await _vendorBusinessLogic.GetAllVendor();
        }

        private void ShowVendorForm()
        {
            IsVendorFormVisible = true;
        }

        private void HideVendorForm()
        {
            IsVendorFormVisible = true;
        }
    }
}
