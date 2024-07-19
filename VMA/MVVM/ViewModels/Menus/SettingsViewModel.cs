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


        private ObservableCollection<Department> departments;
        public ObservableCollection<Department> Departments
        {
            get
            { return departments; }
            set
            {
                departments = value;
                OnPropertyChanged(nameof(Departments));
            }
        }

        private Department selectedDepartment;
        public Department SelectedDepartment
        {
            get
            { return selectedDepartment; }
            set
            {
                selectedDepartment = value;
                if (selectedDepartment != null)
                {
                    NeworExistingDepartment = selectedDepartment.DepartmentName;
                }
                else
                {
                    NeworExistingDepartment = string.Empty;
                }
                OnPropertyChanged(nameof(SelectedDepartment));
            }
        }

        private string neworExistingDepartment;

        public string NeworExistingDepartment
        {
            get { return neworExistingDepartment; }
            set
            {
                neworExistingDepartment = value;
                OnPropertyChanged(nameof(NeworExistingDepartment));
            }
        }

        #endregion


        /// <summary>
        /// New Document commmnad
        /// </summary>
        private ViewModelCommand addOrUpdateDepartment;

        /// <summary>
        /// Enroll Command
        /// </summary>
        public ICommand AddOrUpdateDepartment
        {
            get
            {
                if (this.addOrUpdateDepartment == null)
                {
                    this.addOrUpdateDepartment = new ViewModelCommand(c => this.AddOrUpdateDepartments(), x => this.CanAddOrUpdateDepartments());
                }

                return this.addOrUpdateDepartment;
            }
        }

        /// <summary>
        /// New Document commmnad
        /// </summary>
        private ViewModelCommand deleteDepartmentCommand;

        /// <summary>
        /// Enroll Command
        /// </summary>
        public ICommand DeleteDepartmentCommand
        {
            get
            {
                if (this.deleteDepartmentCommand == null)
                {
                    this.deleteDepartmentCommand = new ViewModelCommand(c => this.DeleteDepartment(c));
                }

                return this.deleteDepartmentCommand;
            }
        }

        public ICommand SubmitCommand { get; }

        public SettingsViewModel()
        {
            Departments = new ObservableCollection<Department>();
            Departments = [new Department() { Id = "1", DepartmentName = "IT" },
            new Department() {Id= "2", DepartmentName = "Hardware"},
            new Department() {Id = "3", DepartmentName="Software"}];
            // SubmitCommand = new ViewModelAsyncCommand<GstcalculationMasterModel>(SaveGst, ValidateGst);
            //_ = GetGSTDetails();
        }

        private void AddOrUpdateDepartments()
        {
            if (SelectedDepartment == null)
            {
                Departments.Add(new Department()
                {
                    DepartmentName = NeworExistingDepartment,
                    Id = Convert.ToString(Departments.Count + 1)
                });

                NeworExistingDepartment = string.Empty;
            }
            else
            {
                Departments.Where(x => x.Id == SelectedDepartment.Id).FirstOrDefault().DepartmentName = NeworExistingDepartment;
                SelectedDepartment = null;
            }
        }

        private bool CanAddOrUpdateDepartments()
        {
            return !string.IsNullOrEmpty(NeworExistingDepartment);
        }

        private void DeleteDepartment(object department)
        {
            Department localdepartment = (Department)department;

            Departments.Remove(localdepartment);
        }

        private bool ValidateGst()
        {
            return true;
        }

        private async Task SaveGst(object model)
        {
            //GstcalculationMasterModel gstcalculationMaster = new()
            //{
            //    CgstPercentage = Cgstpercentage,
            //    IgstPercentage = Igstpercentage,
            //    SgstPercentage = Sgstpercentage,
            //    CreatedBy = UserAccountModel.Username,
            //    LastUpdateBy = UserAccountModel.Username,
            //    LastUpdatedDate = DateTime.UtcNow,
            //    CreatedDate = DateTime.UtcNow,
            //    IsActive = true,
            //};
            //await _gstcalculationMasterBusinessLogic.AddGstMaster(gstcalculationMaster);
        }

        //public async Task GetGSTDetails()
        //{
        //    var latestGST = await _gstcalculationMasterBusinessLogic.GetAllGstMaster();
        //    Cgstpercentage = Convert.ToInt32(latestGST?.ToList()?.FirstOrDefault()?.CgstPercentage);
        //    Sgstpercentage = Convert.ToInt32(latestGST?.ToList()?.FirstOrDefault()?.SgstPercentage);
        //    Igstpercentage = Convert.ToInt32(latestGST?.ToList()?.FirstOrDefault()?.IgstPercentage);


        //}
    }

    public class Department : ViewModelBase
    {
        private string id;
        public string Id 
        { 
            get { return id; } 
            set 
            { 
                id = value; 
                OnPropertyChanged(nameof(Id)); 
            } 
        }

        private string departmentName;

        public string DepartmentName 
        { 
            get { return departmentName; } 
            set 
            { 
                departmentName = value; 
                OnPropertyChanged(nameof(DepartmentName)); 
            } 
        }
    }
}
