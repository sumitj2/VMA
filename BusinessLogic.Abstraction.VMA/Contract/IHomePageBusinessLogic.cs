using BusinessLogic.Abstraction.VMA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Abstraction.VMA.Contract
{
    public interface IHomePageBusinessLogic
    {
        Task<DashboardDetailsModel> GetDashboardDetails(string? financialYear);
    }
}
