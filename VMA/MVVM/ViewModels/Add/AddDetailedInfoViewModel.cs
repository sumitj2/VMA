using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VMA.MVVM.ViewModels.Menus;

namespace VMA.MVVM.ViewModels.Add
{
    public class AddDetailedInfoViewModel : ViewModelBase
    {
        private readonly DetailedInfoViewModel _detailedInfoViewModel;
        public ICommand HideDetailInfoFormCommand { get; }

        public AddDetailedInfoViewModel(DetailedInfoViewModel detailedInfoViewModel)
        {
            _detailedInfoViewModel = detailedInfoViewModel;
            HideDetailInfoFormCommand = new ViewModelAsyncCommand<VendorViewModel>(HideDetailInfoForm);
        }

        private async Task HideDetailInfoForm(VendorViewModel model)
        {
            await _detailedInfoViewModel.HideDetailInfoForm(this).ConfigureAwait(true);
        }

    }
}
