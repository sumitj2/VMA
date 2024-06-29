
using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.Abstraction.VMA.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VMA.MVVM.ViewModels.Menus;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace VMA.MVVM.ViewModels.Add
{
    public class AddUpdateVendorViewModel : ViewModelBase
    {
        private readonly IVendorBusinessLogic _vendorbusinessLogic;
        private readonly VendorViewModel _vendorViewModel;
        private int _selectedTabIndex;        
        

        #region Command
        public ICommand BackCommand { get; set; }
        public ICommand NextCommand { get; set; }
        public ICommand HideVendorFormCommand { get; }
        public ICommand SubmitCommand { get; }
        public ICommand ClearFormCommand { get; }        
        public ICommand SwitchToTab1Command { get; }
        public ICommand SwitchToTab2Command { get; }

        #endregion

        #region Properties
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
       
        public VendorModel _selectedVendor { get; set; }       
        public string SaveButtonName
        {
            get => _saveButtonName;

            set
            {
                _saveButtonName = value;
                OnPropertyChanged(nameof(SaveButtonName));
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
                _vendorPinCode = value;
                OnPropertyChanged(nameof(VendorPinCode));
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
                _vendorPhoneNo = value;
                OnPropertyChanged(nameof(VendorPhoneNo));
            }
        }

        [EmailAddress(ErrorMessage = "Invalid email address")]
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
        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set
            {
                _selectedTabIndex = value;
                OnPropertyChanged(nameof(SelectedTabIndex));
                OnPropertyChanged(nameof(CanGoBack));
                OnPropertyChanged(nameof(CanGoNext));
            }
        }

        #endregion

        public AddUpdateVendorViewModel(IVendorBusinessLogic vendorBusinessLogic, VendorViewModel parentViewModel, VendorModel SelectedVendor)
        {
            _selectedVendor = SelectedVendor;
            if (SelectedVendor != null) 
            {
                SaveButtonName = "Update";
            }
            else
            {
                SaveButtonName = "Submit";
            }
            SelectedTabIndex = 0;
            VendorCode = Convert.ToString(parentViewModel.Vendors.Count + 1);

            PopulateValues();
            BackCommand = new ViewModelCommand(CanGoBack);
            NextCommand = new ViewModelCommand(CanGoNext);
             _vendorbusinessLogic = vendorBusinessLogic;
            _vendorViewModel = parentViewModel;
            HideVendorFormCommand = new ViewModelCommand(HideVendorForm);
            SubmitCommand = new ViewModelCommand(SaveVendor);
            ClearFormCommand = new ViewModelCommand(ClearValues);

            //Initialize command
        }

        private void CanGoBack(object obj)
        {
            SelectedTabIndex--;
        }

        private void CanGoNext(object obj)
        {
            SelectedTabIndex++;
        }

        private void PopulateValues()
        {
            if (_selectedVendor != null)
            {
                VendorAccountNumber = _selectedVendor.VendorAccountNumber;
                VendorCode = _selectedVendor.VendorCode;
                VendorAddress = _selectedVendor.VendorAddress;
                VendorBankName = _selectedVendor.VendorBankName;
                VendorEmailId = _selectedVendor.VendorEmailId;
                VendorName = _selectedVendor.VendorName;
                VendorIfsccode = _selectedVendor.VendorIfsccode;
                VendorPhoneNo = _selectedVendor.VendorPhoneNo;
                VendorPinCode = _selectedVendor.VendorPinCode;
                VendorGstnumber = _selectedVendor.VendorGstnumber;             
            }
        }

        private void ClearValues(object obj)
        {
            VendorAccountNumber = "";
            VendorCode = "";
            VendorAddress = "";
            VendorBankName = "";
            VendorEmailId = "";
            VendorName = "";
            VendorIfsccode = "";
            VendorPhoneNo = "";
            VendorPinCode = "";
            VendorGstnumber = "";
        }

        private void SaveVendor(object obj)
        {
            if (SaveButtonName == "Update")
            {
                VendorModel vendorModel = new VendorModel()
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
                    CreatedBy = Thread.CurrentPrincipal?.Identity?.Name ?? "",
                    VendorGstnumber = _vendorGstnumber,
                    VendorId = _selectedVendor.VendorId,
                   
                };
                _vendorbusinessLogic.EditUpdateVendor(vendorModel);
            }
            else
            {
                VendorModel vendorModel = new VendorModel()
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
                    CreatedBy = Thread.CurrentPrincipal?.Identity?.Name ?? "",
                    VendorGstnumber = _vendorGstnumber
                };
                _vendorbusinessLogic.AddVendor(vendorModel);
                //to do :refresh not happen on grid view
            }
            HideVendorForm(this);
        }

        private void HideVendorForm(object obj)
        {
            _vendorViewModel.HideVendorForm(this);
        }
    }
}
