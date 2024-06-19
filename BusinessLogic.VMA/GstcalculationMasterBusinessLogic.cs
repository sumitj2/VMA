using BusinessLogic.Abstraction.VMA.Contract;
using Database.Abstraction.VMA.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.VMA.Repositories
{
    public class GstcalculationMasterBusinessLogic : IGstcalculationMasterBusinessLogic
    {
        private readonly IGstcalculationMasterRepository _gstcalculationMasterRepository;
        public GstcalculationMasterBusinessLogic(IGstcalculationMasterRepository gstcalculationMasterRepository)
        {
            _gstcalculationMasterRepository = gstcalculationMasterRepository;
        }
    }
}
