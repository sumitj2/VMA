using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.Abstraction.VMA.Models;
using Database.VMA.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VMA.MVVM.Models;

namespace VMA.MVVM.ViewModels.Menus
{
    public class SettingsViewModel : ViewModelBase
    {

        private readonly IGstcalculationMasterBusinessLogic _gstcalculationMasterBusinessLogic;



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
            set { _Sgstpercentage = value;
                OnPropertyChanged(nameof(Sgstpercentage));
            }
        }

        private int _Igstpercentage;

        public int Igstpercentage
        {
            get { return _Igstpercentage; }
            set { _Igstpercentage = value;
                OnPropertyChanged(nameof(Igstpercentage));
            }
        }

        #endregion

        public ICommand SubmitCommand { get; }
        public SettingsViewModel(IGstcalculationMasterBusinessLogic gstcalculationMasterBusinessLogic)
        {
            _gstcalculationMasterBusinessLogic = gstcalculationMasterBusinessLogic;
            SubmitCommand = new ViewModelAsyncCommand<GstcalculationMasterModel>(SaveGst, ValidateGst);
            _ = GetGSTDetails();
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
