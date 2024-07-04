using BusinessLogic.Abstraction.VMA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VMA.MVVM.ViewModels.Menus;

namespace VMA.MVVM.ViewModels.Add
{
    public class AddPaymentsViewModel : ViewModelBase
    {
        private readonly PaymentsViewModel _paymentViewModel;
        private bool isGSTDetailsVisible;

        public bool IsGSTDetailsVisible
        {
            get { return isGSTDetailsVisible; }
            set 
            {
                if (isGSTDetailsVisible != value)
                {
                    isGSTDetailsVisible = value;                    
                    OnPropertyChanged(nameof(GSTTabVisible));
                }
            }
        }

        private bool isTDSTextBoxVisible;

        public bool IsTDSTextBoxVisible
        {
            get { return isTDSTextBoxVisible; }
            set 
            {
                if(isTDSTextBoxVisible!=value)
                {
                    isTDSTextBoxVisible = value;
                    OnPropertyChanged(nameof(TextBoxVisibility));
                    OnPropertyChanged(nameof(TextBlockVisibility));

                }
            }
        }

        private bool isBranchNameVisible;

        public bool IsBranchNameVisible
        {
            get { return isBranchNameVisible; }
            set 
            { 
                
                if (isBranchNameVisible != value)
                {
                    isBranchNameVisible = value;
                    OnPropertyChanged(nameof(TextBoxBranchNameVisibility));
                    OnPropertyChanged(nameof(TextBlockBranchNameVisibility));

                }
            }
        }


        public Visibility GSTTabVisible
        {
            get { return IsGSTDetailsVisible ? Visibility.Visible : Visibility.Collapsed; }
        }
        public Visibility TextBoxVisibility
        {
            get { return IsTDSTextBoxVisible ? Visibility.Visible : Visibility.Collapsed; }
        }
        public Visibility TextBlockVisibility
        {
            get { return IsTDSTextBoxVisible ? Visibility.Visible : Visibility.Collapsed; }
        }

        public Visibility TextBoxBranchNameVisibility
        {
            get { return IsBranchNameVisible ? Visibility.Visible : Visibility.Collapsed; }
        }
        public Visibility TextBlockBranchNameVisibility
        {
            get { return IsBranchNameVisible ? Visibility.Visible : Visibility.Collapsed; }
        }

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
