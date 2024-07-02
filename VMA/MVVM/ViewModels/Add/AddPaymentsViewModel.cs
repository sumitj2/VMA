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
    public class AddPaymentsViewModel : ViewModelBase
    {
        private readonly PaymentsViewModel _paymentViewModel;
        public ICommand HidePaymentFormCommand { get; }
        public AddPaymentsViewModel(PaymentsViewModel vendorViewModel)
        {
            _paymentViewModel = vendorViewModel;
            HidePaymentFormCommand = new ViewModelAsyncCommand<VendorPaymentModel>(HidePaymentForm);
        }

        public async Task HidePaymentForm(object model)
        {
            await _paymentViewModel.HidePaymentForm(this).ConfigureAwait(true);
        }

    }
}
