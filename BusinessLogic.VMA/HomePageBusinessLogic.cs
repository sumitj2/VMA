using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.Abstraction.VMA.Models;
using Database.Abstraction.VMA.Contract;
using Database.VMA.Entities.CustomEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.VMA
{
    public class HomePageBusinessLogic : IHomePageBusinessLogic
    {
        private readonly IStoreProcedureExecutionRepository _storeProcedureExecutionRepository;
        public HomePageBusinessLogic(IStoreProcedureExecutionRepository storeProcedureExecutionRepository)
        {
            _storeProcedureExecutionRepository = storeProcedureExecutionRepository;
        }

        public async Task<DashboardDetailsModel> GetDashboardDetails(string? financialYear)
        {
            var result = await _storeProcedureExecutionRepository.GetDashboardDetailsAsync(financialYear).ConfigureAwait(true);
            DashboardDetailsModel dashboardDetailsModel = new();

            if (result != null)
            {
                dashboardDetailsModel.CountOfVendors = result.CountOfVendors;
                dashboardDetailsModel.CountOfServices = result.CountOfServices;
                dashboardDetailsModel.TotalPaidAmount = result.TotalPaidAmount;
                dashboardDetailsModel.TotalSanctionAmount = result.TotalSanctionAmount;

            }
            return dashboardDetailsModel;
        }

        public async Task<List<YearlyReportDataModel>> GetDashboardServicesBarChartDetails(string? financialYear)
        {
            var result = await _storeProcedureExecutionRepository.GetYearlyReportDataAsync(financialYear);
            List<YearlyReportDataModel> dashboardServiceModel = new();

            foreach (var yearRepor in result)
            {
                dashboardServiceModel.Add(new YearlyReportDataModel() 
                {
                    NumberOfTerms = yearRepor.ServicePaymentType == "None" ? yearRepor.TotalPaymentsMade : yearRepor.NumberOfTerms,
                    RemainingAmount = yearRepor.RemainingAmount,
                    RemainingTerms = yearRepor.ServicePaymentType=="None"? 0:yearRepor.RemainingTerms,
                    ServicePaymentType = yearRepor.ServicePaymentType,  
                    ServiceSantionAmount=yearRepor.ServiceSantionAmount,
                    TotalPaymentsMade = yearRepor.TotalPaymentsMade,
                    TotalVendorPaymentAmount = yearRepor.TotalVendorPaymentAmount != null?(decimal)yearRepor.TotalVendorPaymentAmount:0,
                    VendorName = yearRepor.VendorName,  
                    VendorServiceName=yearRepor.VendorServiceName ,
                    TotalTermsCompleted= yearRepor.NumberOfTerms- yearRepor.RemainingTerms
                });
            }
            return dashboardServiceModel;
        }
    }
}
