using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.Abstraction.VMA.Models;
using BusinessLogic.VMA;
using Database.VMA.Entities.CustomEntities;
using Database.VMA.Repositories;
using Serilog;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using VMA.Constants;

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
        private bool _IsPaymentTypeYes;
        public bool IsPaymentTypeYes
        {
            get { return _IsPaymentTypeYes; }
            set
            {
                _ = ClearForm(null);
                _IsPaymentTypeYes = value;
                OnPropertyChanged(nameof(IsPaymentTypeYes));
            }
        }

        private bool _IsPaymentTypeNo;
        public bool IsPaymentTypeNo
        {
            get { return _IsPaymentTypeNo; }
            set
            {
                _ = ClearForm(null);
                _IsPaymentTypeNo = value;
                OnPropertyChanged(nameof(IsPaymentTypeNo));
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
                    _ = ClearForm(null);
                    OnPropertyChanged(nameof(SelectedVendorModel));
                    BeforeInvocie += " " + SelectedVendorModel.VendorName+ " ";
                    AfterInvoice += " " + SelectedVendorModel.VendorName + " ";
                    _ = LoadVendorServiceDetails(SelectedVendorModel.VendorId,VendorPaymentYear);
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

        #region Command

        public ICommand ExportPaymentNoteCommand { get; }
        public ICommand GenerateCommand { get; }
        public ICommand MonthlyReportCommand { get; }
        public ICommand YearlyReportCommand { get; }
        public ICommand ClearFormCommand { get; }

        #endregion

        public readonly IVenderPaymentNotesBusinessLogic _venderPaymentNotesBusinessLogic;
        public readonly IReportExportToExcelPaymentNote _reportExportToExcelPaymentNote;
        public readonly IVendorBusinessLogic _vendorBusinessLogic;
        public readonly IVendorDetailsBusinessLogic _vendorDetailsBusinessLogic;
        public readonly IConfigurationBusinessLogic _configurationBusinessLogic;
        public readonly IPaymentNoteInWord _paymentNoteInWord;
        public readonly IYearlyMonthlyReportPDF _yearlyReportPDF;
        public ReportsViewModel(IReportExportToExcelPaymentNote reportExportToExcelPaymentNote, IVendorBusinessLogic vendorBusinessLogic, IVendorDetailsBusinessLogic vendorDetailsBusinessLogic, IConfigurationBusinessLogic configurationBusinessLogic, IPaymentNoteInWord paymentNoteInWord, IYearlyMonthlyReportPDF yearlyReportPDF, IVenderPaymentNotesBusinessLogic venderPaymentNotesBusinessLogic)
        {
            _venderPaymentNotesBusinessLogic = venderPaymentNotesBusinessLogic;
            IsPaymentTypeNo = true;
            Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Into the constructor", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));
            To = MessagesContants.ReportTo;
            From = MessagesContants.ReportFrom;
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
            ClearFormCommand = new ViewModelAsyncCommand<Database.VMA.Entities.CustomEntities.ExportPaymentNoteData>(ClearForm);
            _ = CallAync();
        }

        private async Task ClearForm(object o)
        {
            await Task.Run(() =>
            {
                SelectedVendorName = null;
                SelctedVendorServiceName = null;
                BeforeInvocie = "";
                AfterInvoice = "";
                NoteGenerationDate = DateOnly.MinValue;
                //  IsPaymentTypeNo = true;
            });
        }

        private async Task GenerateYearlyReport(Database.VMA.Entities.CustomEntities.ExportPaymentNoteData note)
        {
            if (pathWord == null || pathWord.Length == 0)
            {
                SuccessPopupViewModel.Instance.ShowPopup(Enums.NotificationType.Failure, MessagesContants.MsgStorageLocationNotFound, false, true);
            }
            else
            {
                Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Into the Function GenerateYearlyReport for Year: {2}", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name, _vendorPaymentYear));

                await _yearlyReportPDF.GenerateYearlyReport(_vendorPaymentYear, pathWord).ConfigureAwait(true);
            }
        }

        private async Task GenerateMonthlyReport(Database.VMA.Entities.CustomEntities.ExportPaymentNoteData note)
        {
            if (pathWord == null || pathWord.Length == 0)
            {
                SuccessPopupViewModel.Instance.ShowPopup(Enums.NotificationType.Failure, MessagesContants.MsgStorageLocationNotFound, false, true);
            }
            else
            {
                await _yearlyReportPDF.GenerateMonthlyReport(_vendorPaymentYear, "month_need_to_pass", pathExcel).ConfigureAwait(true);
            }
        }

        private async Task GeneratePaymentNote(CreateWordDocumentPaymentNote note)
        {
            if (pathWord == null || pathWord.Length == 0)
            {
                SuccessPopupViewModel.Instance.ShowPopup(Enums.NotificationType.Failure, MessagesContants.MsgStorageLocationNotFound, false, true);
            }
            else
            {
                if (SelectedVendorDetailService?.VendorServiceName != null || !IsPaymentTypeYes)
                {
                    var paymentNoteNo = await _venderPaymentNotesBusinessLogic.GetPaymentNoteByVendorIdAndDetailServiceId(Convert.ToInt32(SelectedVendorModel?.VendorId),Convert.ToInt32(SelectedVendorDetailService?.VendorDetailId), _vendorPaymentYear);
                    if (paymentNoteNo != null)
                    {
                        if (IsPaymentTypeYes)
                        {
                            await _paymentNoteInWord.CreateAndOpenWordFile(VendorServiceDetails?.Select(x => x.VendorServiceName).ToList(), From, To, BeforeInvocie + "The summary of the invoice is as under", AfterInvoice + " ", _vendorPaymentYear, pathWord, SelectedVendorModel.VendorName, paymentNoteNo?.PaymentNoteNo, NoteGenerationDate).ConfigureAwait(true);
                        }
                        else
                        {
                            await _paymentNoteInWord.CreateAndOpenWordFile(new List<string>() { SelectedVendorDetailService.VendorServiceName }, From, To, BeforeInvocie + "The summary of the invoice is as under", AfterInvoice + " ", _vendorPaymentYear, pathWord, SelectedVendorModel.VendorName, paymentNoteNo?.PaymentNoteNo, NoteGenerationDate).ConfigureAwait(true);
                        }
                    }
                    else
                    {
                        SuccessPopupViewModel.Instance.ShowPopup(Enums.NotificationType.Failure, "Payment Note is not generated for this service", false, true);
                    }
                }
                else
                {
                    SuccessPopupViewModel.Instance.ShowPopup(Enums.NotificationType.Failure, MessagesContants.MsgSelectService, false, true);
                }
            }
            await ClearForm(note);
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
            Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Into the function ExportPaymentNote", this.GetType().Name, MethodBase.GetCurrentMethod()?.Name));
            await _reportExportToExcelPaymentNote.ExportPaymentNotes(_vendorPaymentYear, pathExcel).ConfigureAwait(true);
        }

        private async Task LoadVendors()
        {
            var vendors = await _vendorBusinessLogic.GetAllVendor().ConfigureAwait(true);
            VendorModels = new ObservableCollection<VendorModel>(vendors.ToList().OrderBy(x => x.VendorName));
        }
        private async Task LoadVendorServiceDetails(int vendorId, string detailYear)
        {
            var vendorServiceDetails = await _vendorDetailsBusinessLogic.GetAllVendorDetails(detailYear).ConfigureAwait(true);
            if (IsPaymentTypeYes)
            {
                VendorServiceDetails = new ObservableCollection<VendorDetailModel>(vendorServiceDetails.Where(x => x.VendorId == vendorId && x.ServicePaymentType == GeneralConstants.PaymentTypeNone));
            }
            else
            {
                VendorServiceDetails = new ObservableCollection<VendorDetailModel>(vendorServiceDetails.Where(x => x.VendorId == vendorId && x.ServicePaymentType != GeneralConstants.PaymentTypeNone));
            }
        }

        public async Task GetAllConfigurations()
        {
            var allConfigurations = await _configurationBusinessLogic.GetConfigurations().ConfigureAwait(true);
            VendorPaymentYear = allConfigurations.FirstOrDefault(x => x.Cfgkey == GeneralConstants.CFGKeyFinacialYear)?.CfgValue;
            pathWord = allConfigurations.FirstOrDefault(x => x.Cfgkey == GeneralConstants.CFGKeyWordPath)?.CfgValue;

            pathExcel = allConfigurations.FirstOrDefault(x => x.Cfgkey == GeneralConstants.CFGKeyExcelPath)?.CfgValue;
            if (pathWord == null || pathExcel == null)
            {
                SuccessPopupViewModel.Instance.ShowPopup(Enums.NotificationType.Failure, MessagesContants.MsgStorageLocationNotFound, false, true);
            }

        }
    }
}
