
using BusinessLogic.Abstraction.VMA.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VMA.MVVM.ViewModels.Menus;

namespace VMA.MVVM.ViewModels.Add
{
    public class AddUpdateVendorViewModel : ViewModelBase
    {
        private readonly IVendorBusinessLogic _vendorbusinessLogic;
        private readonly VendorViewModel _vendorViewModel;

        //Commands
        public ICommand HideVendorFormCommand { get; }

        public AddUpdateVendorViewModel(IVendorBusinessLogic vendorBusinessLogic, VendorViewModel parentViewModel)
        {
            _vendorbusinessLogic = vendorBusinessLogic;
            _vendorViewModel = parentViewModel;
            HideVendorFormCommand = new ViewModelCommand(HideVendorForm);
        }

        private void HideVendorForm(object obj)
        {
            _vendorViewModel.HideVendorForm(this);
        }
    }
}
