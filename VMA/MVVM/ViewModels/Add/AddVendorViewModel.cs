
using BusinessLogic.Abstraction.VMA.Contract;
using Database.VMA.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VMA.MVVM.ViewModels.Menus;

namespace VMA.MVVM.ViewModels.Add
{
    public class AddVendorViewModel : ViewModelBase
    {
        private IVendorBusinessLogic _vendorbusinessLogic;
        private VendorViewModel _vendorViewModel;

        private ViewModelCommand hideVendorFormCommand;
        public ICommand HideVendorFormCommand;
        //{
        //    get
        //    {
        //        if (this.hideVendorFormCommand == null)
        //        {
        //            this.hideVendorFormCommand = new ViewModelCommand(x => HideVendorForm());

        //        }
        //        return this.hideVendorFormCommand;
        //    }
        //}

        public AddVendorViewModel(IVendorBusinessLogic vendorBusinessLogic, VendorViewModel parentViewModel)
        {
            _vendorbusinessLogic = vendorBusinessLogic;
            _vendorViewModel = parentViewModel;
            HideVendorFormCommand = new ViewModelCommand(HideVendorForm);
        }
        private void HideVendorForm(object obj)
        {
            //_vendorViewModel.HideVendorForm(this);
        }

    }
}
