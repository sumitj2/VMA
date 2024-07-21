using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.Abstraction.VMA.Models;
using Database.VMA.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using System.Windows.Input;
using VMA.MVVM.Models;

namespace VMA.MVVM.ViewModels.Menus
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly IConfigurationBusinessLogic _configBusinessLogic;

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
                    BtnDepartmentContent = "Update";
                    NeworExistingDepartment = selectedDepartment.DepartmentName;
                }
                else
                {
                    BtnDepartmentContent = "Add";
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

        private ObservableCollection<Expenditure> expenditures;
        public ObservableCollection<Expenditure> Expenditures
        {
            get
            { return expenditures; }
            set
            {
                expenditures = value;
                OnPropertyChanged(nameof(Expenditures));
            }
        }

        private Expenditure selectedExpenditure;
        public Expenditure SelectedExpentidure
        {
            get
            { return selectedExpenditure; }
            set
            {
                selectedExpenditure = value;
                if (selectedExpenditure != null)
                {
                    BtnExpenditureContent = "Update";
                    NeworExistingExpenditure = selectedExpenditure.ExpenditureName;
                }
                else
                {
                    BtnExpenditureContent = "Add";
                    NeworExistingExpenditure = string.Empty;
                }
                OnPropertyChanged(nameof(SelectedExpentidure));
            }
        }

        private string neworExistingExpenditure;

        public string NeworExistingExpenditure
        {
            get { return neworExistingExpenditure; }
            set
            {
                neworExistingExpenditure = value;
                OnPropertyChanged(nameof(NeworExistingExpenditure));
            }
        }

        private ObservableCollection<Sanction> sanctions;
        public ObservableCollection<Sanction> Sanctions
        {
            get
            { return sanctions; }
            set
            {
                sanctions = value;
                OnPropertyChanged(nameof(Sanctions));
            }
        }

        private Sanction selectedSanction;
        public Sanction SelectedSanction
        {
            get
            { return selectedSanction; }
            set
            {
                selectedSanction = value;
                if (selectedSanction != null)
                {
                    BtnSanctionContent = "Update";
                    NeworExistingSanction = selectedSanction.SanctionName;
                }
                else
                {
                    BtnSanctionContent = "Add";
                    NeworExistingSanction = string.Empty;
                }
                OnPropertyChanged(nameof(SelectedSanction));
            }
        }

        private string btnDepartmentContent = "Add";

        public string BtnDepartmentContent
        {
            get { return btnDepartmentContent; }
            set
            {
                btnDepartmentContent = value;
                OnPropertyChanged(nameof(BtnDepartmentContent));
            }
        }

        private string btnExpenditureContent = "Add";

        public string BtnExpenditureContent
        {
            get { return btnExpenditureContent; }
            set
            {
                btnExpenditureContent = value;
                OnPropertyChanged(nameof(BtnExpenditureContent));
            }
        }

        private string btnSanctionContent = "Add";

        public string BtnSanctionContent
        {
            get { return btnSanctionContent; }
            set
            {
                btnSanctionContent = value;
                OnPropertyChanged(nameof(BtnSanctionContent));
            }
        }

        private string neworExistingSanction;

        public string NeworExistingSanction
        {
            get { return neworExistingSanction; }
            set
            {
                neworExistingSanction = value;
                OnPropertyChanged(nameof(NeworExistingSanction));
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

        /// <summary>
        /// New Document commmnad
        /// </summary>
        private ViewModelCommand addOrUpdateExpenditure;

        /// <summary>
        /// Enroll Command
        /// </summary>
        public ICommand AddOrUpdateExpenditure
        {
            get
            {
                if (this.addOrUpdateExpenditure == null)
                {
                    this.addOrUpdateExpenditure = new ViewModelCommand(c => this.AddOrUpdateExpenditures(), x => this.CanAddOrUpdateExpenditures());
                }

                return this.addOrUpdateExpenditure;
            }
        }

        /// <summary>
        /// New Document commmnad
        /// </summary>
        private ViewModelCommand deleteExpenditureCommand;

        /// <summary>
        /// Enroll Command
        /// </summary>
        public ICommand DeleteExpenditureCommand
        {
            get
            {
                if (this.deleteExpenditureCommand == null)
                {
                    this.deleteExpenditureCommand = new ViewModelCommand(c => this.DeleteExpenditure(c));
                }

                return this.deleteExpenditureCommand;
            }
        }

        /// <summary>
        /// New Document commmnad
        /// </summary>
        private ViewModelCommand addOrUpdateSanction;

        /// <summary>
        /// Enroll Command
        /// </summary>
        public ICommand AddOrUpdateSanction
        {
            get
            {
                if (this.addOrUpdateSanction == null)
                {
                    this.addOrUpdateSanction = new ViewModelCommand(c => this.AddOrUpdateSanctions(), x => this.CanAddOrUpdateSanctions());
                }

                return this.addOrUpdateSanction;
            }
        }

        /// <summary>
        /// New Document commmnad
        /// </summary>
        private ViewModelCommand deleteSanctionCommand;

        /// <summary>
        /// Enroll Command
        /// </summary>
        public ICommand DeleteSanctionCommand
        {
            get
            {
                if (this.deleteSanctionCommand == null)
                {
                    this.deleteSanctionCommand = new ViewModelCommand(c => this.DeleteSanction(c));
                }

                return this.deleteSanctionCommand;
            }
        }

        public ICommand SubmitCommand { get; }

        public SettingsViewModel(IConfigurationBusinessLogic configBusinessLogic)
        {
            _configBusinessLogic = configBusinessLogic;

            GetAllConfigurations();

            // SubmitCommand = new ViewModelAsyncCommand<GstcalculationMasterModel>(SaveGst, ValidateGst);
            //_ = GetGSTDetails();
        }


        public async void GetAllConfigurations()
        {
            var allConfigurations = await _configBusinessLogic.GetConfigurations().ConfigureAwait(true);

            string departmentConfigJson = allConfigurations.Where(x => x.Cfgkey == nameof(Department)).FirstOrDefault()?.CfgValue;
            string expenditureConfigJson = allConfigurations.Where(x => x.Cfgkey == nameof(Expenditure)).FirstOrDefault()?.CfgValue;
            string sanctionConfigJson = allConfigurations.Where(x => x.Cfgkey == nameof(Sanction)).FirstOrDefault()?.CfgValue;

            if (!string.IsNullOrEmpty(departmentConfigJson))
            {
                Departments = JsonSerializer.Deserialize<ObservableCollection<Department>>(departmentConfigJson);
            }
            else
            {
                Departments = new ObservableCollection<Department>();
            }

            if (!string.IsNullOrEmpty(expenditureConfigJson))
            {
                Expenditures = JsonSerializer.Deserialize<ObservableCollection<Expenditure>>(expenditureConfigJson);
            }
            else
            {
                Expenditures = new ObservableCollection<Expenditure>(); 
            }

            if (!string.IsNullOrEmpty(sanctionConfigJson))
            {
                Sanctions = JsonSerializer.Deserialize<ObservableCollection<Sanction>>(sanctionConfigJson);
            }
            else
            {
                Sanctions = new ObservableCollection<Sanction>();
            }
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

            string departmentjson = JsonSerializer.Serialize(Departments);

            ConfigurationModel configuration = new ConfigurationModel() { Cfgkey = nameof(Department), CfgValue = departmentjson };
            _configBusinessLogic.AddConfiguration(configuration);
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

        private void AddOrUpdateExpenditures()
        {
            if (SelectedExpentidure == null)
            {
                Expenditures.Add(new Expenditure()
                {
                    ExpenditureName = NeworExistingExpenditure,
                    Id = Convert.ToString(Expenditures.Count + 1)
                });

                NeworExistingExpenditure = string.Empty;
            }
            else
            {
                Expenditures.Where(x => x.Id == SelectedExpentidure.Id).FirstOrDefault().ExpenditureName = NeworExistingExpenditure;
                SelectedExpentidure = null;
            }

            string expenditurejson = JsonSerializer.Serialize(Expenditures);

            ConfigurationModel configuration = new ConfigurationModel() { Cfgkey = nameof(Expenditure), CfgValue = expenditurejson };
            _configBusinessLogic.UpdateOrDeleteConfiguration(configuration);
        }

        private bool CanAddOrUpdateExpenditures()
        {
            return !string.IsNullOrEmpty(NeworExistingExpenditure);
        }

        private void DeleteExpenditure(object expenditure)
        {
            Expenditure localexpenditure = (Expenditure)expenditure;

            Expenditures.Remove(localexpenditure);
        }

        private void AddOrUpdateSanctions()
        {
            if (SelectedSanction == null)
            {
                Sanctions.Add(new Sanction()
                {
                    SanctionName = NeworExistingSanction,
                    Id = Convert.ToString(Sanctions.Count + 1)
                });

                NeworExistingSanction = string.Empty;
            }
            else
            {
                Sanctions.Where(x => x.Id == SelectedSanction.Id).FirstOrDefault().SanctionName = NeworExistingSanction;
                SelectedSanction = null;
            }

            string sanctionjson = JsonSerializer.Serialize(Sanctions);

            ConfigurationModel configuration = new ConfigurationModel() { Cfgkey = nameof(Sanction), CfgValue = sanctionjson };
            _configBusinessLogic.UpdateOrDeleteConfiguration(configuration);
        }

        private bool CanAddOrUpdateSanctions()
        {
            return !string.IsNullOrEmpty(NeworExistingSanction);
        }

        private void DeleteSanction(object sanction)
        {
            Sanction localsanction = (Sanction)sanction;

            Sanctions.Remove(localsanction);
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

    public class Departments
    {
        public List<Department> departments;
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

    public class Expenditure : ViewModelBase
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

        private string expenditureName;

        public string ExpenditureName
        {
            get { return expenditureName; }
            set
            {
                expenditureName = value;
                OnPropertyChanged(nameof(ExpenditureName));
            }
        }
    }

    public class Sanction : ViewModelBase
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

        private string sanctionName;

        public string SanctionName
        {
            get { return sanctionName; }
            set
            {
                sanctionName = value;
                OnPropertyChanged(nameof(SanctionName));
            }
        }
    }

}
