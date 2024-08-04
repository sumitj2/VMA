using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.Abstraction.VMA.Models;
using BusinessLogic.VMA;
using Database.VMA.Entities.CustomEntities;
using Database.VMA.Repositories;
using Serilog;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace VMA.MVVM.ViewModels.Menus
{
    public class ReportsViewModel : ViewModelBase
    {
        #region Observable collections for Combo box

        private string? pathWord;

        public string? PathWord
        {
            get { return pathWord; }
            set { pathWord = value; }
        }

        private string? pathExcel;

        public string? PathExcel
        {
            get { return pathExcel; }
            set { pathExcel = value; }
        }


        private VendorDetailModel _SelectedVendorDetailService;
        public VendorDetailModel SelectedVendorDetailService
        {
            get { return _SelectedVendorDetailService; }
            set
            {
                _SelectedVendorDetailService = value;
                if (SelectedVendorDetailService != null)
                {
                    BeforeInvocie += SelectedVendorDetailService.VendorServiceName;
                    AfterInvoice += SelectedVendorDetailService.VendorServiceName;
                    OnPropertyChanged(nameof(SelectedVendorDetailService));
                }
            }
        }

        private string? _From;

        public string? From
        {
            get { return _From; }
            set
            {
                _From = value;
                OnPropertyChanged(nameof(From));
            }
        }
        private string? _To;

        public string? To
        {
            get { return _To; }
            set
            {
                _To = value;
                OnPropertyChanged(nameof(To));
            }
        }

        private DateOnly _NoteGenerationDate;

        public DateOnly NoteGenerationDate
        {
            get { return _NoteGenerationDate; }
            set
            {
                _NoteGenerationDate = value;
                OnPropertyChanged(nameof(NoteGenerationDate));
            }
        }


        private string? beforeInvocie;

        public string? BeforeInvocie
        {
            get { return beforeInvocie; }
            set
            {
                beforeInvocie = value;
                OnPropertyChanged(nameof(BeforeInvocie));
            }
        }
        private string? afterInvoice;

        public string? AfterInvoice
        {
            get { return afterInvoice; }
            set
            {
                afterInvoice = value;
                OnPropertyChanged(nameof(AfterInvoice));
            }
        }

        private string? _vendorPaymentYear;
        public string? VendorPaymentYear
        {
            get { return _vendorPaymentYear; }
            set
            {
                _vendorPaymentYear = value;
                OnPropertyChanged(nameof(VendorPaymentYear));
            }
        }

        private string _SelctedVendorServiceName;
        public string SelctedVendorServiceName
        {
            get { return _SelctedVendorServiceName; }
            set
            {
                _SelctedVendorServiceName = value;
                OnPropertyChanged(nameof(SelctedVendorServiceName));
            }
        }

        private string _SelectedVendorName;
        public string SelectedVendorName
        {
            get { return _SelectedVendorName; }
            set
            {
                _SelectedVendorName = value;
                OnPropertyChanged(nameof(SelectedVendorName));
            }
        }

        private VendorModel _SelectedVendorModel;
        public VendorModel SelectedVendorModel
        {
            get { return _SelectedVendorModel; }
            set
            {
                _SelectedVendorModel = value;

                if (SelectedVendorModel != null)
                {
                    OnPropertyChanged(nameof(SelectedVendorModel));
                    BeforeInvocie += " " + SelectedVendorModel.VendorName;
                    AfterInvoice += " " + SelectedVendorModel.VendorName;
                    _ = LoadVendorServiceDetails(SelectedVendorModel.VendorId);
                }
            }
        }

        private ObservableCollection<VendorDetailModel> _VendorServiceDetails;

        public ObservableCollection<VendorDetailModel> VendorServiceDetails
        {
            get { return _VendorServiceDetails; }
            set
            {
                _VendorServiceDetails = value;
                OnPropertyChanged(nameof(VendorServiceDetails));
            }
        }

        private ObservableCollection<VendorModel> _vendorModels;
        public ObservableCollection<VendorModel> VendorModels
        {
            get { return _vendorModels; }
            set
            {
                _vendorModels = value;
                OnPropertyChanged(nameof(VendorModels));
            }
        }
        #endregion

        public ICommand ExportPaymentNoteCommand { get; }
        public ICommand GenerateCommand { get; }
        public ICommand MonthlyReportCommand { get; }
        public ICommand YearlyReportCommand { get; }

        public readonly IReportExportToExcelPaymentNote _reportExportToExcelPaymentNote;
        public readonly IVendorBusinessLogic _vendorBusinessLogic;
        public readonly IVendorDetailsBusinessLogic _vendorDetailsBusinessLogic;
        public readonly IConfigurationBusinessLogic _configurationBusinessLogic;
        public readonly IPaymentNoteInWord _paymentNoteInWord;
        public readonly IYearlyMonthlyReportPDF _yearlyReportPDF;
        public ReportsViewModel(IReportExportToExcelPaymentNote reportExportToExcelPaymentNote, IVendorBusinessLogic vendorBusinessLogic, IVendorDetailsBusinessLogic vendorDetailsBusinessLogic, IConfigurationBusinessLogic configurationBusinessLogic, IPaymentNoteInWord paymentNoteInWord, IYearlyMonthlyReportPDF yearlyReportPDF)
        {
            Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Into the constructor", this.GetType().Name, MethodBase.GetCurrentMethod().Name));
            To = "Sr.Officer - Central Office";
            From = "Chief Manager - IT";
            _reportExportToExcelPaymentNote = reportExportToExcelPaymentNote;
            _vendorBusinessLogic = vendorBusinessLogic;
            _vendorDetailsBusinessLogic = vendorDetailsBusinessLogic;
            _configurationBusinessLogic = configurationBusinessLogic;
            _paymentNoteInWord = paymentNoteInWord;
            _yearlyReportPDF = yearlyReportPDF;

            ExportPaymentNoteCommand = new ViewModelAsyncCommand<Database.VMA.Entities.CustomEntities.ExportPaymentNoteData>(ExportPaymentNote);
            GenerateCommand = new ViewModelAsyncCommand<CreateWordDocumentPaymentNote>(GeneratePaymentNote);
            MonthlyReportCommand = new ViewModelAsyncCommand<Database.VMA.Entities.CustomEntities.ExportPaymentNoteData>(GenerateMonthlyReport);
            YearlyReportCommand = new ViewModelAsyncCommand<Database.VMA.Entities.CustomEntities.ExportPaymentNoteData>(GenerateYearlyReport);
            _ = CallAync();
        }

        private async Task GenerateYearlyReport(Database.VMA.Entities.CustomEntities.ExportPaymentNoteData note)
        {
            if (pathWord == null || pathWord.Length == 0)
            {
                SuccessPopupViewModel.Instance.ShowPopup(Enums.NotificationType.Failure, "Please Set file storage location in settings", false, true);
            }
            else
            {
                await _yearlyReportPDF.GenerateYearlyReport(_vendorPaymentYear, pathWord).ConfigureAwait(true);
            }
        }

        private async Task GenerateMonthlyReport(Database.VMA.Entities.CustomEntities.ExportPaymentNoteData note)
        {
            if (pathWord == null || pathWord.Length == 0)
            {
                SuccessPopupViewModel.Instance.ShowPopup(Enums.NotificationType.Failure, "Please Set file storage location in settings", false, true);
            }
            else
            {
               // await _yearlyReportPDF.GenerateMonthlyReport(_vendorPaymentYear, "month_need_to_pass", pathExcel).ConfigureAwait(true);
            }
        }

        private async Task GeneratePaymentNote(CreateWordDocumentPaymentNote note)
        {
            if (pathWord == null || pathWord.Length == 0)
            {
                SuccessPopupViewModel.Instance.ShowPopup(Enums.NotificationType.Failure, "Please Set file storage location in settings", false, true);
            }
            else
            {
                await _paymentNoteInWord.CreateAndOpenWordFile(SelectedVendorDetailService.VendorServiceName, From, To, BeforeInvocie + "The summary of the invoice is as under", AfterInvoice + " " + To, _vendorPaymentYear, pathWord).ConfigureAwait(true);
            }
        }

        private async Task CallAync()
        {
            await MainTask();
        }

        public async Task MainTask()
        {
            await GetAllConfigurations();
            await LoadVendors();
        }

        private async Task ExportPaymentNote(Database.VMA.Entities.CustomEntities.ExportPaymentNoteData data)
        {
            await _reportExportToExcelPaymentNote.ExportPaymentNotes(_vendorPaymentYear, pathExcel).ConfigureAwait(true);
        }

        private async Task LoadVendors()
        {
            var vendors = await _vendorBusinessLogic.GetAllVendor().ConfigureAwait(true);
            VendorModels = new ObservableCollection<VendorModel>(vendors);
        }
        private async Task LoadVendorServiceDetails(int vendorId)
        {
            var vendorServiceDetails = await _vendorDetailsBusinessLogic.GetAllVendorDetails().ConfigureAwait(true);
            VendorServiceDetails = new ObservableCollection<VendorDetailModel>(vendorServiceDetails.Where(x => x.VendorId == vendorId));
        }

        public async Task GetAllConfigurations()
        {
            var allConfigurations = await _configurationBusinessLogic.GetConfigurations().ConfigureAwait(true);
            VendorPaymentYear = allConfigurations.FirstOrDefault(x => x.Cfgkey == "FinancialYear")?.CfgValue;
            pathWord = allConfigurations.FirstOrDefault(x => x.Cfgkey == "FilePathWord")?.CfgValue;

            pathExcel = allConfigurations.FirstOrDefault(x => x.Cfgkey == "FilePathExcel")?.CfgValue;
            if (pathWord == null || pathExcel == null)
            {
                SuccessPopupViewModel.Instance.ShowPopup(Enums.NotificationType.Failure, "Please Set file storage location in settings", false, true);
            }

        }
    }
}
