using Database.VMA.Entities.CustomEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Abstraction.VMA.Contract
{
    public interface IStoreProcedureExecutionRepository
    {
        Task<List<YearlyReportData>> GetYearlyReportDataAsync(string? detailsYear);

        Task<DashboardDetails?> GetDashboardDetailsAsync(string? detailsYear);
    }
}
