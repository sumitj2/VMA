using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.Abstraction.VMA.Models;
using Database.VMA.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VMA.MVVM.ViewModels.Add;

namespace VMA.MVVM.ViewModels.Menus
{
    public class GSTViewModel : ViewModelBase
    {
        private readonly MainViewModel _parentViewModel;
        private readonly IGstcalculationMasterBusinessLogic _gstcalculationMasterBusinessLogic;
        public GSTViewModel(IGstcalculationMasterBusinessLogic gstcalculationMasterBusinessLogic, MainViewModel parentViewModel)
        {
            _parentViewModel = parentViewModel;
            _gstcalculationMasterBusinessLogic = gstcalculationMasterBusinessLogic;
            AddShowGSTFormCommand = new ViewModelAsyncCommand<GstcalculationMasterModel>(ShowGSTForm);
            HideGSTFormCommand = new ViewModelAsyncCommand<GstcalculationMasterModel>(HideGSTForm);
            EditGSTCommand = new ViewModelAsyncCommand<GstcalculationMasterModel>(EditGST);
        }

        private Task ShowGSTForm(GstcalculationMasterModel model)
        {
            _parentViewModel.CurrentChildView = new AddUpdateGSTMasterViewModel(_gstcalculationMasterBusinessLogic,this);
            return Task.CompletedTask;
        }

        private async Task EditGST(GstcalculationMasterModel model)
        {
            throw new NotImplementedException();
        }

        public Task HideGSTForm(object model)
        {
            _parentViewModel.CurrentChildView = this;
            return Task.CompletedTask;
        }

        

        #region commands

        public ICommand AddShowGSTFormCommand { get; }

        public ICommand UpdateGSTFormCommand { get; }
        public ICommand HideGSTFormCommand { get; }

        public ICommand EditGSTCommand { get; }
        #endregion
    }
}
