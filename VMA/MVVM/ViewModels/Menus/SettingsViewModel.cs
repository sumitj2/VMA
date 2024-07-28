using Azure;
using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.Abstraction.VMA.Models;
using Serilog;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text.Json;
using System.Windows.Input;

namespace VMA.MVVM.ViewModels.Menus
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly IConfigurationBusinessLogic _configBusinessLogic;

        #region Properties

        private string? _financialYear;
        public string? FinancialYear
        {
            get { return _financialYear; }
            set
            {
                _financialYear = value;
                OnPropertyChanged(nameof(FinancialYear));
            }
        }

        private string? _noteId;
        public string? NoteId
        {
            get { return _noteId; }
            set
            {
                _noteId = value;
                OnPropertyChanged(nameof(NoteId));
            }
        }


        private ObservableCollection<Department>? departments;
        public ObservableCollection<Department>? Departments
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

        private ObservableCollection<Expenditure>? expenditures;
        public ObservableCollection<Expenditure>? Expenditures
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

        private ObservableCollection<Sanction>? sanctions;
        public ObservableCollection<Sanction>? Sanctions
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
        public Sanction? SelectedSanction
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

        #region FIle Storage
        private string? _filePathExcel;

        public string? FilePathExcel
        {
            get { return _filePathExcel; }
            set
            {
                _filePathExcel = value;
                OnPropertyChanged(nameof(FilePathExcel));
            }
        }

        private string? _filePathWord;

        public string? FilePathWord
        {
            get { return _filePathWord; }
            set
            {
                _filePathWord = value;
                OnPropertyChanged(nameof(FilePathWord));
            }
        }

        private async Task BrowseExcelLocation(object obj)
        {
            // Implement file browsing logic here
            // For example, using OpenFileDialog
            var openFileDialog = new Microsoft.Win32.OpenFolderDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                FilePathExcel = openFileDialog.FolderName;
            }

            bool? getExcelFileLocation = AllConfigurations?.Any(x => x?.Cfgkey == nameof(FilePathExcel));
            if (FilePathExcel != null)
            {
                if (getExcelFileLocation == true)
                {
                    string operation = "";
                    await SaveGenralSettings(AllConfigurations.FirstOrDefault(x => x.Cfgkey == nameof(FilePathExcel)).Id, nameof(FilePathExcel), FilePathExcel, operation).ConfigureAwait(true);
                }
                else
                {
                    string operation = "add";
                    await SaveGenralSettings(0, nameof(FilePathExcel), FilePathExcel, operation).ConfigureAwait(true);
                }
            }

        }

        private async Task BrowseWordLocation(object obj)
        {
            // Implement file browsing logic here
            // For example, using OpenFileDialog
            var openFileDialog = new Microsoft.Win32.OpenFolderDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                FilePathWord = openFileDialog.FolderName;
            }
            bool? getWordFileLocation = AllConfigurations?.Any(x => x?.Cfgkey == nameof(FilePathWord));
            if (FilePathWord != null)
            {
                if (getWordFileLocation == true)
                {
                    string operation = "";
                    await SaveGenralSettings(AllConfigurations.FirstOrDefault(x => x.Cfgkey == nameof(FilePathWord)).Id, nameof(FilePathWord), FilePathWord, operation).ConfigureAwait(true);
                }
                else
                {
                    string operation = "add";
                    await SaveGenralSettings(0, nameof(FilePathWord), FilePathWord, operation).ConfigureAwait(true);
                }
            }

        }

        public ICommand BrowseExcelLocationCommand { get; }

        public ICommand BrowseWordLocationCommand { get; }

        //public ICommand SaveLocationCommand { get; }

        #endregion

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

        public ICommand SubmitNoteIdCommand { get; }

        public ICommand SubmitFinancialYearCommand { get; }

        public SettingsViewModel(IConfigurationBusinessLogic configBusinessLogic)
        {
            Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Into the constructor", this.GetType().Name, MethodBase.GetCurrentMethod().Name));

            _configBusinessLogic = configBusinessLogic;

            _ = GetAllConfigurations();

            SubmitFinancialYearCommand = new ViewModelAsyncCommand<object>(SaveFinancialYearSettings, ValidateGeneralSettings);
            SubmitNoteIdCommand = new ViewModelAsyncCommand<object>(SaveNoteFormat);
            BrowseExcelLocationCommand = new ViewModelAsyncCommand<object>(BrowseExcelLocation);
            BrowseWordLocationCommand = new ViewModelAsyncCommand<object>(BrowseWordLocation);
        }
        private async Task SaveNoteFormat(object model)
        {
            bool getNoteId = AllConfigurations.Any(x => x?.Cfgkey == nameof(NoteId));
            string operation = "";
            if (getNoteId)
            {
                await SaveGenralSettings(AllConfigurations.FirstOrDefault(x => x.Cfgkey == nameof(NoteId)).Id, nameof(NoteId), NoteId, operation);
            }
            else
            {
                operation = "add";

                await SaveGenralSettings(0, nameof(NoteId), NoteId, operation);
            }
        }
        private async Task SaveFinancialYearSettings(object model)
        {
            bool getFinancialYear = AllConfigurations.Any(x => x?.Cfgkey == nameof(FinancialYear));

            string operation = "";

            if (getFinancialYear)
            {
                await SaveGenralSettings(AllConfigurations.FirstOrDefault(x => x.Cfgkey == nameof(FinancialYear)).Id, nameof(FinancialYear), FinancialYear, operation);
            }
            else
            {
                operation = "add";
                await SaveGenralSettings(0, nameof(FinancialYear), FinancialYear, operation);
            }
        }

        private ObservableCollection<ConfigurationModel>? _allConfigurations;
        public ObservableCollection<ConfigurationModel>? AllConfigurations
        {
            get
            { return _allConfigurations; }
            set
            {
                _allConfigurations = value;
                OnPropertyChanged(nameof(AllConfigurations));
            }
        }

        public async Task GetAllConfigurations()
        {
            Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Getting All configurations", this.GetType().Name, MethodBase.GetCurrentMethod().Name));

            try
            {
                var allConfigurations = await _configBusinessLogic.GetConfigurations().ConfigureAwait(true);
                AllConfigurations = new ObservableCollection<ConfigurationModel>(allConfigurations);

                if (allConfigurations != null)
                {
                    string? departmentConfigJson = allConfigurations.FirstOrDefault(x => x.Cfgkey == nameof(Department))?.CfgValue;
                    string? expenditureConfigJson = allConfigurations.FirstOrDefault(x => x.Cfgkey == nameof(Expenditure))?.CfgValue;
                    string? sanctionConfigJson = allConfigurations.FirstOrDefault(x => x.Cfgkey == nameof(Sanction))?.CfgValue;
                    string? financialYear = allConfigurations.FirstOrDefault(x => x.Cfgkey == nameof(FinancialYear))?.CfgValue;
                    string? noteID = allConfigurations.FirstOrDefault(x => x.Cfgkey == nameof(NoteId))?.CfgValue;

                    NoteId = noteID;
                    FinancialYear = financialYear;

                    if (!string.IsNullOrEmpty(departmentConfigJson))
                    {
                        Departments = JsonSerializer.Deserialize<ObservableCollection<Department>>(departmentConfigJson);
                    }
                    else
                    {
                        Departments = [];
                    }

                    if (!string.IsNullOrEmpty(expenditureConfigJson))
                    {
                        Expenditures = JsonSerializer.Deserialize<ObservableCollection<Expenditure>>(expenditureConfigJson);
                    }
                    else
                    {
                        Expenditures = [];
                    }

                    if (!string.IsNullOrEmpty(sanctionConfigJson))
                    {
                        Sanctions = JsonSerializer.Deserialize<ObservableCollection<Sanction>>(sanctionConfigJson);
                    }
                    else
                    {
                        Sanctions = [];
                    }

                    Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Loaded All configurations", this.GetType().Name, MethodBase.GetCurrentMethod().Name));

                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, string.Format("Class: {0}, Method: {1} - Failed to load All configurations", this.GetType().Name, MethodBase.GetCurrentMethod().Name));

                SuccessPopupViewModel.Instance.ShowPopup(Enums.NotificationType.Failure, "Failed to load All configurations, Please contact to Administrator", false, true);
            }
        }
        private async void AddOrUpdateDepartments()
        {
            Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Into AddOrUpdate Departments", this.GetType().Name, MethodBase.GetCurrentMethod().Name));

            bool isDepartmentExist = Departments?.Count(x => x.DepartmentName == NeworExistingDepartment) > 0;

            string operation = "";
            if (Departments != null && Departments.Any())
            {
                if (!isDepartmentExist)
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

                    SaveConfiguration(AllConfigurations.FirstOrDefault(x => x.Cfgkey == nameof(Department)).Id, nameof(Department), Departments, operation);
                }
                else
                {
                    SuccessPopupViewModel.Instance.ShowPopup(Enums.NotificationType.Alert, "Department is already exist, please try different", false, true);
                }
            }
            else
            {
                Departments?.Add(new Department()
                {
                    DepartmentName = NeworExistingDepartment,
                    Id = Convert.ToString(Departments.Count + 1)
                });

                operation = "add";

                await SaveConfiguration(0, nameof(Department), Departments, operation);
            }

            Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Done AddOrUpdate Departments", this.GetType().Name, MethodBase.GetCurrentMethod().Name));

        }

        private bool CanAddOrUpdateDepartments()
        {
            return !string.IsNullOrEmpty(NeworExistingDepartment);
        }

        private void DeleteDepartment(object department)
        {
            Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Into Delete Department", this.GetType().Name, MethodBase.GetCurrentMethod().Name));

            string operation = "";
            Department localdepartment = (Department)department;

            Departments.Remove(localdepartment);

            SaveConfiguration(AllConfigurations.FirstOrDefault(x => x.Cfgkey == "Department").Id, nameof(Department), Departments, operation);
        }

        private void AddOrUpdateExpenditures()
        {
            Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Into AddOrUpdate Expenditures", this.GetType().Name, MethodBase.GetCurrentMethod().Name));

            bool isExpenditureExist = Expenditures.Count(x => x.ExpenditureName == NeworExistingExpenditure) > 0;
            string operation = "";

            if (Expenditures != null && Expenditures.Any())
            {
                if (!isExpenditureExist)
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

                    SaveConfiguration(AllConfigurations.FirstOrDefault(x => x.Cfgkey == nameof(Expenditure)).Id, nameof(Expenditure), Expenditures, operation);
                }
                else
                {
                    SuccessPopupViewModel.Instance.ShowPopup(Enums.NotificationType.Alert, "Expenditure is already exist, please try different", false, true);
                }
            }
            else
            {
                Expenditures.Add(new Expenditure()
                {
                    ExpenditureName = NeworExistingExpenditure,
                    Id = Convert.ToString(Expenditures.Count + 1)
                });

                operation = "add";

                SaveConfiguration(0, nameof(Expenditure), Expenditures, operation);
            }

            Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Done AddOrUpdate Expenditures", this.GetType().Name, MethodBase.GetCurrentMethod().Name));

        }

        private bool CanAddOrUpdateExpenditures()
        {
            return !string.IsNullOrEmpty(NeworExistingExpenditure);
        }

        private async Task DeleteExpenditure(object expenditure)
        {
            Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Into Delete expenditure", this.GetType().Name, MethodBase.GetCurrentMethod().Name));

            string operation = "";
            Expenditure localexpenditure = (Expenditure)expenditure;

            Expenditures?.Remove(localexpenditure);

            await SaveConfiguration(AllConfigurations.FirstOrDefault(x => x.Cfgkey == nameof(Expenditure)).Id, nameof(Expenditure), Expenditures, operation);
        }

        private async void AddOrUpdateSanctions()
        {
            Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Into AddOrUpdate Sanctions", this.GetType().Name, MethodBase.GetCurrentMethod().Name));

            bool isSanctionExist = Sanctions?.Count(x => x.SanctionName == NeworExistingSanction) > 0;
            string operation = "";

            if (Sanctions != null && Sanctions.Any())
            {
                if (!isSanctionExist)
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
                        Sanctions.FirstOrDefault(x => x.Id == SelectedSanction.Id).SanctionName = NeworExistingSanction;
                        SelectedSanction = null;
                    }

                    await SaveConfiguration(AllConfigurations.FirstOrDefault(x => x.Cfgkey == nameof(Sanction)).Id, nameof(Sanction), Sanctions, operation);
                }
                else
                {
                    SuccessPopupViewModel.Instance.ShowPopup(Enums.NotificationType.Alert, "Sanction is already exist, please try different", false, true);
                }
            }
            else
            {
                Sanctions?.Add(new Sanction()
                {
                    SanctionName = NeworExistingSanction,
                    Id = Convert.ToString(Sanctions.Count + 1)
                });

                operation = "add";

                await SaveConfiguration(0, nameof(Sanction), Sanctions, operation);
            }

            Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Done AddOrUpdate Sanctions", this.GetType().Name, MethodBase.GetCurrentMethod().Name));

        }

        private bool CanAddOrUpdateSanctions()
        {
            return !string.IsNullOrEmpty(NeworExistingSanction);
        }

        private async Task DeleteSanction(object sanction)
        {
            Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Into Delete Sanction", this.GetType().Name, MethodBase.GetCurrentMethod().Name));

            string operation = "";
            Sanction localsanction = (Sanction)sanction;

            Sanctions?.Remove(localsanction);

            await SaveConfiguration(AllConfigurations.FirstOrDefault(x => x.Cfgkey == nameof(Sanction)).Id, nameof(Sanction), Sanctions, operation);
        }

        private bool ValidateGeneralSettings()
        {
            return true;
        }



        private async Task SaveConfiguration(int id, string cfgkey, object cfgvalue, string operation)
        {
            string cfgvaluejson = JsonSerializer.Serialize(cfgvalue);
            await Save(id, cfgkey, operation, cfgvaluejson);
        }

        private async Task Save(int id, string cfgkey, string operation, string cfgvaluejson)
        {
            try
            {
                Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Trying to save configuration", this.GetType().Name, MethodBase.GetCurrentMethod().Name));

                if (operation != "add")
                {
                    await _configBusinessLogic.UpdateOrDeleteConfiguration(new ConfigurationModel() { Id = id, Cfgkey = cfgkey, CfgValue = cfgvaluejson }).ConfigureAwait(true);
                }
                else
                {
                    await _configBusinessLogic.AddConfiguration(new ConfigurationModel() { Id = id, Cfgkey = cfgkey, CfgValue = cfgvaluejson });
                }

                Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Configuration has been saved successfully", this.GetType().Name, MethodBase.GetCurrentMethod().Name));

                await GetAllConfigurations();
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, string.Format("Class: {0}, Method: {1} - Failed to save configuration", this.GetType().Name, MethodBase.GetCurrentMethod().Name));

                SuccessPopupViewModel.Instance.ShowPopup(Enums.NotificationType.Failure, "Failed to save configuration, please try again or contact to administrator", false, true);
            }
        }

        private async Task SaveGenralSettings(int id, string cfgkey, string cfgvalue, string operation)
        {
            await Save(id, cfgkey, operation, cfgvalue);

        }

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
        private string? id;
        public string? Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        private string? expenditureName;

        public string? ExpenditureName
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
        private string? id;
        public string? Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        private string? sanctionName;

        public string? SanctionName
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
