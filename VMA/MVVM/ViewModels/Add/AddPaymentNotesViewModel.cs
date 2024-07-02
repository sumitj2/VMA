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
    public class AddPaymentNotesViewModel : ViewModelBase
    {
        private readonly PaymentNotesViewModel _paymentNotesViewModel;
        public ICommand HidePaymentNotesFormCommand { get; }

        public AddPaymentNotesViewModel(PaymentNotesViewModel paymentNotesViewModel)
        {
            _paymentNotesViewModel = paymentNotesViewModel;
            HidePaymentNotesFormCommand = new ViewModelAsyncCommand<VenderPaymentNoteModel>(HidePaymentNoteForm);
        }

        private async Task HidePaymentNoteForm(VenderPaymentNoteModel model)
        {
            await _paymentNotesViewModel.HidePaymentNotesForm(this);
        }

    }
}
