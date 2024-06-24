using BusinessLogic.Abstraction.VMA.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VMA.MVVM.ViewModels.Menus.Vendor
{
    public class VendorDetailsViewModel : ViewModelBase
    {
        private IVendorBusinessLogic _vendorbusinessLogic;
        private VendorViewModel _vendorViewModel;

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

        public VendorDetailsViewModel(IVendorBusinessLogic vendorBusinessLogic, VendorViewModel parentViewModel) 
        {
            _vendorbusinessLogic = vendorBusinessLogic;
            _vendorViewModel = parentViewModel;
        }

        private void HideVendorForm() { _vendorViewModel.HideVendorForm();
        }

    }
}
