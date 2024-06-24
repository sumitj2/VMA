using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.Abstraction.VMA.Models;
using Database.VMA.Entities;
using Database.VMA.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VMA.MVVM.ViewModels.Add;

namespace VMA.MVVM.ViewModels.Menus
{
    public class VendorViewModel : ViewModelBase
    {
        private readonly IVendorBusinessLogic _vendorBusinessLogic;
        private MainViewModel _parentViewModel;

        //-> Commands
        private ViewModelCommand showVendorFormCommand;
        public ICommand ShowVendorFormCommand;
        //{
        //    get
        //    {
        //        showVendorFormCommand ??= new ViewModelCommand(x => ShowVendorForm());
        //        return showVendorFormCommand;
        //    }
        //}

        private ViewModelCommand hideVendorFormCommand;


        public ICommand HideVendorFormCommand;
        //{
        //    get
        //    {
        //        if (this.hideVendorFormCommand == null)
        //        {
        //            this.hideVendorFormCommand = new ViewModelCommand(x => HideVendorForm());

        //        }
        //        return this.hideVendorFormCommand;
        //    }
        //}

        private bool _isVendorFormVisible;
        public bool IsVendorFormVisible
        {
            get { return _isVendorFormVisible; }
            set
            {
                _isVendorFormVisible = value;
                OnPropertyChanged(nameof(IsVendorFormVisible));
            }
        }

        private ObservableCollection<VendorModel> _vendors;
        public ObservableCollection<VendorModel> Vendors
        {
            get { return _vendors; }
            set
            {
                _vendors = value;
                OnPropertyChanged(nameof(Vendors));
            }
        }

        public VendorViewModel(IVendorBusinessLogic vendorBusinessLogic, MainViewModel parentViewModel)
        {
            _vendorBusinessLogic = vendorBusinessLogic;
            _parentViewModel = parentViewModel;
            ShowVendorFormCommand = new ViewModelCommand(ShowVendorForm);
            HideVendorFormCommand = new ViewModelCommand(HideVendorForm);
            _ = GetVendors();
        }

        private async Task GetVendors()
        {
            var vendors = await _vendorBusinessLogic.GetAllVendor();
            Vendors = new ObservableCollection<VendorModel>(vendors);
        }

        private void ShowVendorForm(object obj)
        {
            _parentViewModel.CurrentChildView = new AddVendorViewModel(_vendorBusinessLogic, this);

        }

        private void HideVendorForm(object obj)
        {
            IsVendorFormVisible = true;
        }
    }
}
