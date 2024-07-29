using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Abstraction.VMA.Contract
{
    public interface IYearlyMonthlyReportPDF
    {
        public Task GenerateYearlyReport(string? financilaYear, string? path);
        public Task GenerateMonthlyReport(string? financilaYear,string? Month, string? path);
    }
}
