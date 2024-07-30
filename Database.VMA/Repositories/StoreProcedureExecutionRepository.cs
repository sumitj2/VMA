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

        public async Task<List<YearlyReportData>> GetVendorServiceDataAsync(string detailsYear)
        {
            var detailsYearParam = new SqlParameter("@DetailsYear", detailsYear ?? (object)DBNull.Value);

            return await _context.YearlyReportDatas
                .FromSqlRaw("EXECUTE GetVendorServiceData @DetailsYear", detailsYearParam)
                .ToListAsync();
        }
    }
}
