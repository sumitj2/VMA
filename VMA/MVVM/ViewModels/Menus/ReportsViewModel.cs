using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.Abstraction.VMA.Models;
using BusinessLogic.VMA;
using Database.VMA.Entities.CustomEntities;
using Database.VMA.Repositories;
using Serilog;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Input;

namespace VMA.MVVM.ViewModels.Menus
{
    public class ReportsViewModel : ViewModelBase
    {
        #region Observable collections for Combo box

        private VendorDetailModel _SelectedVendorDetailService;
        public VendorDetailModel SelectedVendorDetailService
        {
            get { return _SelectedVendorDetailService; }
            set
            {
                _SelectedVendorDetailService = value;
                if (SelectedVendorDetailService != null)
                {
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
        public ICommand GenerateCommand { get;  }
        public readonly IReportExportToExcelPaymentNote _reportExportToExcelPaymentNote;
        public readonly IVendorBusinessLogic _vendorBusinessLogic;
        public readonly IVendorDetailsBusinessLogic _vendorDetailsBusinessLogic;
        public readonly IConfigurationBusinessLogic _configurationBusinessLogic;
        public readonly IPaymentNoteInWord _paymentNoteInWord;
        public ReportsViewModel(IReportExportToExcelPaymentNote reportExportToExcelPaymentNote, IVendorBusinessLogic vendorBusinessLogic, IVendorDetailsBusinessLogic vendorDetailsBusinessLogic, IConfigurationBusinessLogic configurationBusinessLogic,IPaymentNoteInWord paymentNoteInWord )
        {
            Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Into the constructor", this.GetType().Name, MethodBase.GetCurrentMethod().Name));

            _reportExportToExcelPaymentNote = reportExportToExcelPaymentNote;
            _vendorBusinessLogic = vendorBusinessLogic;
            _vendorDetailsBusinessLogic = vendorDetailsBusinessLogic;
            _configurationBusinessLogic = configurationBusinessLogic;
            _paymentNoteInWord = paymentNoteInWord;
            ExportPaymentNoteCommand = new ViewModelAsyncCommand<Database.VMA.Entities.CustomEntities.ExportPaymentNoteData>(ExportPaymentNote);
            GenerateCommand = new ViewModelAsyncCommand<CreateWordDocumentPaymentNote>(GeneratePaymentNote);
            _ = CallAync();
        }

        private async Task GeneratePaymentNote(CreateWordDocumentPaymentNote note)
        {
            await _paymentNoteInWord.CreateAndOpenWordFile(SelectedVendorDetailService.VendorServiceName, From,To,BeforeInvocie,AfterInvoice);
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
            await _reportExportToExcelPaymentNote.ExportPaymentNotes().ConfigureAwait(true);
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

            string? financialYear = allConfigurations.FirstOrDefault(x => x.Cfgkey == "FinancialYear")?.CfgValue;

            VendorPaymentYear = financialYear;
        }
    }
}
