using BusinessLogic.Abstraction.VMA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Abstraction.VMA.Contract
{
    public interface IGstcalculationMasterBusinessLogic

    {

        public Task AddGstMaster(GstcalculationMasterModel GstcalculationMasterModel);
        public Task EditUpdateGst(GstcalculationMasterModel GstcalculationMasterModel);
        public Task<IEnumerable<GstcalculationMasterModel>> GetAllGstMaster();
        public Task<GstcalculationMasterModel?> GetGstMasterById(int srNo);
        public Task RemoveGstMaster(GstcalculationMasterModel GstcalculationMasterModel);
    }
}
