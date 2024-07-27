using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.Abstraction.VMA.Models;
using Serilog;
using System.Reflection;
using System.Windows.Input;
using VMA.MVVM.Models;
using VMA.MVVM.ViewModels.Menus;

namespace VMA.MVVM.ViewModels.Add
{
    public class AddUpdateGSTMasterViewModel : ViewModelBase
    {
        private readonly IGstcalculationMasterBusinessLogic _gstcalculationMasterBusinessLogic;
        private readonly GSTViewModel _parentViewMode;
        public AddUpdateGSTMasterViewModel(IGstcalculationMasterBusinessLogic gstcalculationMasterBusinessLogic, GSTViewModel parentViewMode)
        {
            Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Into the constructor", this.GetType().Name, MethodBase.GetCurrentMethod().Name));

            _gstcalculationMasterBusinessLogic = gstcalculationMasterBusinessLogic;
            _parentViewMode = parentViewMode;
            HideGSDetailsTFormCommand = new ViewModelAsyncCommand<VendorModel>(HideGSTForm);
            SubmitCommand = new ViewModelAsyncCommand<VendorModel>(SaveGSTDetails, ValidateVendor);
            ClearFormCommand = new ViewModelAsyncCommand<VendorModel>(ClearValues);
        }

        private async Task ClearValues(VendorModel model)
        {
            GSTCategory = "";
            Igstpercentage = 0;
            Cgstpercentage = 0;
            Sgstpercentage = 0;
        }

        private bool ValidateVendor()
        {
            return true;
        }

        private async Task SaveGSTDetails(VendorModel model)
        {
            try
            {
                Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Saving GST Details", this.GetType().Name, MethodBase.GetCurrentMethod().Name));

                GstcalculationMasterModel gstcalculationMaster = new()
                {
                    GstCategoryName = GSTCategory,
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
                _ = HideGSTForm(this);

                Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Saved GST Details", this.GetType().Name, MethodBase.GetCurrentMethod().Name));
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex,string.Format("Class: {0}, Method: {1} - Failed to save GST Details", this.GetType().Name, MethodBase.GetCurrentMethod().Name));
            }
        }

        #region Properties

        private string _GSTCategory;

        public string GSTCategory
        {
            get { return _GSTCategory; }
            set
            {
                _GSTCategory = value;
                OnPropertyChanged(nameof(GSTCategory));
            }
        }


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

        public ICommand SubmitCommand { get; }
        public ICommand ClearFormCommand { get; }
        public ICommand HideGSDetailsTFormCommand { get; }

        #endregion        

        private async Task HideGSTForm(object obj)
        {
            await _parentViewMode.HideGSTForm(this);
        }
        private bool ValidateGst()
        {
            return true;
        }
       
    }
}
