using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Abstraction.VMA.Models
{
    public class DashboardDetailsModel
    {
        /// <summary>
        /// Gets or sets the count of unique vendors.
        /// </summary>
        public int CountOfVendors { get; set; }

        /// <summary>
        /// Gets or sets the count of unique services.
        /// </summary>
        public int CountOfServices { get; set; }

        /// <summary>
        /// Gets or sets the total sanctioned amount.
        /// </summary>
        public decimal TotalSanctionAmount { get; set; }

        /// <summary>
        /// Gets or sets the total amount paid.
        /// </summary>
        public decimal TotalPaidAmount { get; set; }
    }
}
