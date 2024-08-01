using BusinessLogic.Abstraction.VMA.Contract;
using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using BusinessLogic.Abstraction.VMA.Models;
using Database.VMA.Entities;

namespace VMA.MVVM.ViewModels.Menus
{
    public class VendorService
    {
        public string VendorName { get; set; }
        public string ServiceName { get; set; }
        public double SanctionedAmt { get; set; }
        public double PaidAmt { get; set; }
    }
    public class HomeViewModel : ViewModelBase
    {
        #region Pie Chart

        private int _CountOfVendors;
        public int CountOfVendors
        {
            get { return _CountOfVendors; }
            set
            {
                _CountOfVendors = value;
                OnPropertyChanged(nameof(CountOfVendors));
            }
        }

        private int _CountOfServices;
        public int CountOfServices
        {
            get { return _CountOfServices; }
            set
            {
                _CountOfServices = value;
                OnPropertyChanged(nameof(CountOfServices));
            }
        }

        private decimal _TotalSanctionAmount;
        public decimal TotalSanctionAmount
        {
            get { return _TotalSanctionAmount; }
            set
            {
                _TotalSanctionAmount = value;
                OnPropertyChanged(nameof(TotalSanctionAmount));
            }
        }

        private decimal _TotalPaidAmount;
        public decimal TotalPaidAmount
        {
            get { return _TotalPaidAmount; }
            set
            {
                _TotalPaidAmount = value;
                OnPropertyChanged(nameof(TotalPaidAmount));
            }
        }

        private string? _financialYear;
        public string? FinancialYear
        {
            get { return _financialYear; }
            set
            {
                _financialYear = value;
                FyTitle = "FY-" + FinancialYear;
                OnPropertyChanged(nameof(FinancialYear));
            }
        }

        private string? _FyTitle;
        public string? FyTitle
        {
            get { return _FyTitle; }
            set
            {
                _FyTitle = value;
                OnPropertyChanged(nameof(FyTitle));
            }
        }

        private SeriesCollection _SeriesCollectionPieChart;
        public SeriesCollection SeriesCollectionPieChart
        {
            get { return _SeriesCollectionPieChart; }
            set
            {

                _SeriesCollectionPieChart = value;
                OnPropertyChanged(nameof(SeriesCollectionPieChart));
            }
        }
        public string[] LabelPieChart { get; set; }
        public Func<double, string> FormatterPieChart { get; set; }
        #endregion

        #region Bar Chart       
        private ObservableCollection<YearlyReportDataModel> _vendorServices;
        private ObservableCollection<string> _services;
        private string _selectedService;
        public ObservableCollection<YearlyReportDataModel> VendorServices
        {
            get { return _vendorServices; }
            set
            {
                _vendorServices = value;
                OnPropertyChanged(nameof(VendorServices));
            }
        }

        public ObservableCollection<string> Services
        {
            get { return _services; }
            set
            {
                _services = value;
                OnPropertyChanged(nameof(Services));
            }
        }

        public string SelectedService
        {
            get { return _selectedService; }
            set
            {
                _selectedService = value;
                OnPropertyChanged(nameof(SelectedService));
                UpdateChart();
            }
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

        private void UpdateChart()
        {
            var filteredVendorServices = VendorServices.Where(vs => vs.VendorServiceName == SelectedService).ToList();

            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Sanctioned Amount",
                    Values = new ChartValues<decimal>(filteredVendorServices.Select(vs => vs.ServiceSantionAmount))
                },
                new ColumnSeries
                {
                    Title = "Paid Amount",
                    Values = new ChartValues<decimal>(filteredVendorServices.Select(vs => vs.TotalVendorPaymentAmount))
                }
            };

            Labels = filteredVendorServices?.Select(vs => vs.VendorName).ToArray();
            Formatter = value => value.ToString("N");

            OnPropertyChanged(nameof(SeriesCollection));
            OnPropertyChanged(nameof(Labels));
            OnPropertyChanged(nameof(Formatter));
        }

        #endregion

        private readonly IHomePageBusinessLogic _homePageBusinessLogic;
        private readonly IConfigurationBusinessLogic _configurationBusinessLogic;


        public HomeViewModel(IHomePageBusinessLogic homePageBusinessLogic, IConfigurationBusinessLogic configurationBusinessLogic)
        {
            _homePageBusinessLogic = homePageBusinessLogic;
            _configurationBusinessLogic = configurationBusinessLogic;

            _ = CallAync();

            LabelPieChart = ["Sanctioned Amount", "Amount Paid"];
            FormatterPieChart = value => value.ToString("C");
        }

        private async Task LoadDataForBarChart()
        {
            List<YearlyReportDataModel> data=new List<YearlyReportDataModel>();
            await Task.Run(() =>
            {
                data = _homePageBusinessLogic.GetDashboardServicesBarChartDetails(FinancialYear).GetAwaiter().GetResult();
            });
            VendorServices = new ObservableCollection<YearlyReportDataModel>(data);
           
            Services = new ObservableCollection<string>(VendorServices.Select(vs => vs.VendorServiceName).Distinct());

            SelectedService = Services.FirstOrDefault();
        }

        private async Task CallAync()
        {
            await MainTask();
        }
        public async Task GetFinancilYearFromConfiguraton()
        {
            var allConfigurations = await _configurationBusinessLogic.GetConfigurationByKey("FinancialYear").ConfigureAwait(true);

            string? financialYear = allConfigurations.CfgValue;

            FinancialYear = financialYear;
        }
        public async Task MainTask()
        {
            await GetFinancilYearFromConfiguraton();
            await GetDashboardDetails(FinancialYear);
            await LoadDataForBarChart();
            await Task.Delay(1000);
        }
        private async Task GetDashboardDetails(string? FinancialYear)
        {
            var details = await _homePageBusinessLogic.GetDashboardDetails(FinancialYear).ConfigureAwait(true);
            if (details != null)
            {
                CountOfVendors = details.CountOfVendors;
                CountOfServices = details.CountOfServices;
                TotalPaidAmount = details.TotalPaidAmount;
                TotalSanctionAmount = details.TotalSanctionAmount;
            }
            SeriesCollectionPieChart = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Sanctioned Amount",
                    Values = new ChartValues<double> {Convert.ToDouble(TotalSanctionAmount)},
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Amount Paid",
                    Values = new ChartValues<double> { Convert.ToDouble(TotalPaidAmount) },
                    DataLabels = true
                }
            };
        }
    }
}
