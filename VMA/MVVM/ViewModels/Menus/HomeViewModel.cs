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

namespace VMA.MVVM.ViewModels.Menus
{
    
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
        private async Task CallAync()
        {
            await MainTask();
        }
        public async Task GetFinancilYearFromConfiguraton()
        {
            var allConfigurations = await _configurationBusinessLogic.GetConfigurationByKey("FinancialYear").ConfigureAwait(true);

            string? financialYear = allConfigurations.CfgValue; //= allConfigurations.FirstOrDefault(x => x.Cfgkey == "FinancialYear")?.CfgValue;

            FinancialYear = financialYear;
        }
        public async Task MainTask()
        {
            await GetFinancilYearFromConfiguraton();
            await GetDashboardDetails(FinancialYear);
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
