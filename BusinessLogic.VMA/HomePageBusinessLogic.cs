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
    }
}
