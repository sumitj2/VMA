
using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.Abstraction.VMA.Models;
using System;
using System.Collections.Generic;
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

        #region Command
        public ICommand HideVendorFormCommand { get; }
        public ICommand SubmitCommand { get; }

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

        public AddUpdateVendorViewModel(IVendorBusinessLogic vendorBusinessLogic, VendorViewModel parentViewModel)
        {
            _vendorbusinessLogic = vendorBusinessLogic;
            _vendorViewModel = parentViewModel;
            HideVendorFormCommand = new ViewModelCommand(HideVendorForm);
            SubmitCommand = new ViewModelCommand(saveVendor);
            //Initialize command
        }

        private void saveVendor(object obj)
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
                CreatedBy= Thread.CurrentPrincipal?.Identity?.Name ?? "",
                VendorGstnumber = _vendorGstnumber                
            };
            _vendorbusinessLogic.AddVendor(vendorModel);
        }

        private void HideVendorForm(object obj)
        {
            _vendorViewModel.HideVendorForm(this);
        }
    }
}
