using Database.Abstraction.VMA.Contract;
using Database.VMA.Entities.CustomEntities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.VMA.Repositories
{
    public class StoreProcedureExecutionRepository : IStoreProcedureExecutionRepository
    {
        private readonly VendorManagementDbContext _context;
        public StoreProcedureExecutionRepository(VendorManagementDbContext context)
        {
            _context = context;
        }

        public async Task<List<YearlyReportData>> GetYearlyReportDataAsync(string? detailsYear)
        {
            var detailsYearParam = new SqlParameter("@DetailsYear", detailsYear ?? (object)DBNull.Value);            
            return
                await _context.YearlyReportData
                .FromSqlRaw("EXECUTE GetYearlyAllServiceReport @DetailsYear", detailsYearParam)
                .AsNoTracking() // Optional: To avoid tracking the entities
                .ToListAsync();
        }

        public async Task<DashboardDetails?> GetDashboardDetailsAsync(string? detailsYear)
        {
            // Use FromSqlRaw to execute the stored procedure and map the result to DTO
            var result = await _context.DashboardData
                .FromSqlRaw("EXECUTE GetDashboardDetails @DetailsYear = {0}", detailsYear)
                .ToListAsync();

            return result.FirstOrDefault();
        }
    }
}
