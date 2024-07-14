using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.Abstraction.VMA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VMA.MVVM.Models;
using VMA.MVVM.ViewModels.Menus;

namespace VMA.MVVM.ViewModels.Add
{
    public class AddUpdateGSTMasterViewModel : ViewModelBase
    {
        private readonly IGstcalculationMasterBusinessLogic _gstcalculationMasterBusinessLogic;
        private readonly GSTViewModel _parentViewMode;
        public AddUpdateGSTMasterViewModel(IGstcalculationMasterBusinessLogic gstcalculationMasterBusinessLogic,GSTViewModel parentViewMode)
        {
            _gstcalculationMasterBusinessLogic= gstcalculationMasterBusinessLogic;
            _parentViewMode = parentViewMode;
            HideVendorFormCommand = new ViewModelAsyncCommand<VendorModel>(HideVendorForm);
            SubmitCommand = new ViewModelAsyncCommand<VendorModel>(SaveVendor, ValidateVendor);
            ClearFormCommand = new ViewModelAsyncCommand<VendorModel>(ClearValues);
        }

        private async Task ClearValues(VendorModel model)
        {
           
        }

        private bool ValidateVendor()
        {
            return true;
        }

        private async Task SaveVendor(VendorModel model)
        {
           
        }

        #region Properties

        private int _Cgstpercentage;

        public int Cgstpercentage
        {
            get { return _Cgstpercentage; }
            set
            {
                _Cgstpercentage = value;
                OnPropertyChanged(nameof(Cgstpercentage));
            }
        }

        private int _Sgstpercentage;

        public int Sgstpercentage
        {
            get { return _Sgstpercentage; }
            set
            {
                _Sgstpercentage = value;
                OnPropertyChanged(nameof(Sgstpercentage));
            }
        }

        private int _Igstpercentage;

        public int Igstpercentage
        {
            get { return _Igstpercentage; }
            set
            {
                _Igstpercentage = value;
                OnPropertyChanged(nameof(Igstpercentage));
            }
        }

        #endregion

        #region Command
       
        public ICommand HideGSTFormCommand { get; }
        public ICommand SubmitCommand { get; }
        public ICommand ClearFormCommand { get; }
        public ICommand HideVendorFormCommand { get; }

        #endregion        

        private async Task HideVendorForm(object obj)
        {
            await _parentViewMode.HideGSTForm(this);
        }
        private bool ValidateGst()
        {
            return true;
        }
        private async Task SaveGst(object model)
        {
            GstcalculationMasterModel gstcalculationMaster = new()
            {
                CgstPercentage = Cgstpercentage,
                IgstPercentage = Igstpercentage,
                SgstPercentage = Sgstpercentage,
                CreatedBy = UserAccountModel.Username,
                LastUpdateBy = UserAccountModel.Username,
                LastUpdatedDate = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow,
                IsActive = true,
            };
            await _gstcalculationMasterBusinessLogic.AddGstMaster(gstcalculationMaster);
        }

        public async Task GetGSTDetails()
        {
            var latestGST = await _gstcalculationMasterBusinessLogic.GetAllGstMaster();
            Cgstpercentage = Convert.ToInt32(latestGST?.ToList()?.FirstOrDefault()?.CgstPercentage);
            Sgstpercentage = Convert.ToInt32(latestGST?.ToList()?.FirstOrDefault()?.SgstPercentage);
            Igstpercentage = Convert.ToInt32(latestGST?.ToList()?.FirstOrDefault()?.IgstPercentage);

        }
    }
}
