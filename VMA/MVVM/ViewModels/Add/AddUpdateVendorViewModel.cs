
using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.Abstraction.VMA.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Windows.Input;
using VMA.Constants;
using VMA.MVVM.Models;
using VMA.MVVM.ViewModels.Menus;

namespace VMA.MVVM.ViewModels.Add
{
    public class AddUpdateVendorViewModel : ViewModelBase
    {
        private readonly IVendorBusinessLogic _vendorbusinessLogic;
        private readonly VendorViewModel _vendorViewModel;       

        #region Command
        public ICommand BackCommand { get; set; }
        public ICommand NextCommand { get; set; }
        public ICommand HideVendorFormCommand { get; }
        public ICommand SubmitCommand { get; }
        public ICommand ClearFormCommand { get; }        

        #endregion

        #region Properties
        private string _vendorPAN;
        private string _vendorCode;
        private string _vendorName;
        private string _vendorAddress;
        private string _vendorPinCode;
        private string _vendorPhoneNo;
        private string _vendorEmailId;
        private string _vendorBankName;
        private string _vendorAccountNumber;
        private string _vendorIfsccode;
        private string _vendorGstnumber;
        private string _saveButtonName;

        public VendorModel SelectedVendor { get; set; }
        public string SaveButtonName
        {
            get => _saveButtonName;

            set
            {
                _saveButtonName = value;
                OnPropertyChanged(nameof(SaveButtonName));
            }
        }

        public string VendorPAN
        {
            get
            {
                return _vendorPAN;
            }
            set
            {
                _vendorPAN = value;
                OnPropertyChanged(nameof(VendorPAN));
            }
        }

        public string VendorCode
        {
            get
            {
                return _vendorCode;
            }
            set
            {
                _vendorCode = value;
                OnPropertyChanged(nameof(VendorCode));
            }
        }

        [Required(ErrorMessage = MessagesContants.RequireVendorName)]
        public string VendorName
        {
            get
            {
                return _vendorName;
            }
            set
            {
                _vendorName = value;
                OnPropertyChanged(nameof(VendorName));
            }
        }

        public string VendorAddress
        {
            get
            {
                return _vendorAddress;
            }
            set
            {
                _vendorAddress = value;
                OnPropertyChanged(nameof(VendorAddress));
            }
        }

        public string VendorPinCode
        {
            get
            {
                return _vendorPinCode;
            }
            set
            {
                if (value.Count() <= 6)
                {
                    _vendorPinCode = value;
                    OnPropertyChanged(nameof(VendorPinCode));
                }
            }
        }

        public string VendorPhoneNo
        {
            get
            {
                return _vendorPhoneNo;
            }
            set
            {
                if (value.Count() <= 10)
                {
                    _vendorPhoneNo = value;
                    OnPropertyChanged(nameof(VendorPhoneNo));
                }
            }
        }

        [EmailAddress(ErrorMessage = MessagesContants.InvalidEmail)]
        public string VendorEmailId
        {
            get
            {
                return _vendorEmailId;
            }
            set
            {
                _vendorEmailId = value;
                OnPropertyChanged(nameof(VendorEmailId));
            }
        }

        public string VendorBankName
        {
            get
            {
                return _vendorBankName;
            }
            set
            {
                _vendorBankName = value;
                OnPropertyChanged(nameof(VendorBankName));
            }
        }

        public string VendorAccountNumber
        {
            get
            {
                return _vendorAccountNumber;
            }
            set
            {
                _vendorAccountNumber = value;
                OnPropertyChanged(nameof(VendorAccountNumber));
            }
        }

        public string VendorIfsccode
        {
            get
            {
                return _vendorIfsccode;
            }
            set
            {
                _vendorIfsccode = value;
                OnPropertyChanged(nameof(VendorIfsccode));
            }
        }

        public string VendorGstnumber
        {
            get
            {
                return _vendorGstnumber;
            }
            set
            {
                _vendorGstnumber = value;
                OnPropertyChanged(nameof(VendorGstnumber));
            }
        }        

        #endregion

        public AddUpdateVendorViewModel(IVendorBusinessLogic vendorBusinessLogic, VendorViewModel parentViewModel, VendorModel SelectedVendor)
        {
            Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Into the constructor", this.GetType().Name, MethodBase.GetCurrentMethod().Name));

            this.SelectedVendor = SelectedVendor;
            if (SelectedVendor != null)
            {
                SaveButtonName = GeneralConstants.Update;
            }
            else
            {
                SaveButtonName = GeneralConstants.Submit;
            }            
            VendorCode = Convert.ToString(parentViewModel?.Vendors?.Count + 1);

            PopulateValues();
            _vendorbusinessLogic = vendorBusinessLogic;
            _vendorViewModel = parentViewModel;
            HideVendorFormCommand = new ViewModelAsyncCommand<VendorModel>(HideVendorForm);
            SubmitCommand = new ViewModelAsyncCommand<VendorModel>(SaveVendor, ValidateVendor);
            ClearFormCommand = new ViewModelAsyncCommand<VendorModel>(ClearValues);
        }

        private bool ValidateVendor()
        {
            bool validData;

            if (string.IsNullOrWhiteSpace(VendorName) || string.IsNullOrWhiteSpace(VendorEmailId))
            {
                validData = false;
            }
            else
            {
                validData = true;
            }

            return validData;
        }
        private void PopulateValues()
        {
            if (SelectedVendor != null)
            {
                VendorAccountNumber = SelectedVendor.VendorAccountNumber ?? "";
                VendorCode = SelectedVendor.VendorCode ?? "";
                VendorAddress = SelectedVendor.VendorAddress ?? "";
                VendorBankName = SelectedVendor.VendorBankName ?? "";
                VendorEmailId = SelectedVendor.VendorEmailId ?? "";
                VendorName = SelectedVendor.VendorName ?? "";
                VendorIfsccode = SelectedVendor.VendorIfsccode ?? "";
                VendorPhoneNo = SelectedVendor.VendorPhoneNo ?? "";
                VendorPinCode = SelectedVendor.VendorPinCode ?? "";
                VendorGstnumber = SelectedVendor.VendorGstnumber ?? "";
                VendorPAN = SelectedVendor.VendorPan ?? "";
            }
        }

        private async Task ClearValues(object obj)
        {
            await Task.Run(() =>
            {
                VendorAccountNumber = "";
                VendorAddress = "";
                VendorBankName = "";
                VendorEmailId = "";
                VendorName = "";
                VendorIfsccode = "";
                VendorPhoneNo = "";
                VendorPinCode = "";
                VendorGstnumber = "";
            });
        }

        private async Task SaveVendor(object obj)
        {
            try
            {
                Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Saving Vendor Details", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));

                if (SaveButtonName == GeneralConstants.Update)
                {
                    VendorModel vendorModel = new()
                    {
                        VendorAccountNumber = _vendorAccountNumber,
                        VendorCode = _vendorCode,
                        VendorAddress = _vendorAddress,
                        VendorBankName = _vendorBankName,
                        VendorEmailId = _vendorEmailId,
                        VendorName = _vendorName,
                        VendorIfsccode = _vendorIfsccode,
                        VendorPhoneNo = _vendorPhoneNo,
                        VendorPinCode = _vendorPinCode,
                        LastUpdateBy = UserAccountModel.Username,
                        LastUpdatedDate = DateTime.UtcNow,
                        VendorGstnumber = _vendorGstnumber,
                        VendorId = SelectedVendor.VendorId,
                        VendorPan = _vendorPAN

                    };
                    await _vendorbusinessLogic.EditUpdateVendor(vendorModel);

                    Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Vendor Details Updated Successfully", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));

                    SuccessPopupViewModel.Instance.ShowPopup(Enums.NotificationType.Success, MessagesContants.SuccessVendorUpdated, true);
                }
                else
                {
                    VendorModel vendorModel = new()
                    {
                        VendorAccountNumber = _vendorAccountNumber,
                        VendorCode = _vendorCode,
                        VendorAddress = _vendorAddress,
                        VendorBankName = _vendorBankName,
                        VendorEmailId = _vendorEmailId,
                        VendorName = _vendorName,
                        VendorIfsccode = _vendorIfsccode,
                        VendorPhoneNo = _vendorPhoneNo,
                        VendorPinCode = _vendorPinCode,
                        CreatedBy = UserAccountModel.Username,
                        CreatedDate = DateTime.UtcNow,
                        VendorGstnumber = _vendorGstnumber,
                        VendorPan = _vendorPAN
                    };
                    await _vendorbusinessLogic.AddVendor(vendorModel);

                    Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Vendor Details Added Successfully", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));

                    SuccessPopupViewModel.Instance.ShowPopup(Enums.NotificationType.Success,MessagesContants.SuccessVendorAdded, true);
                }

                await HideVendorForm(this);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex,string.Format("Class: {0}, Method: {1} - Failed to save vendor details", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));

            }
        }

        private async Task HideVendorForm(object obj)
        {
            await _vendorViewModel.HideVendorForm(this);
        }
    }
}
