using Database.VMA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Abstraction.VMA.Contract
{
    public interface IGstcalculationMasterRepository
    {
        public Task AddGstMaster(GstcalculationMaster GstcalculationMasterEntity);

        public Task EditUpdateGst(GstcalculationMaster GstcalculationMasterEntity);
        public Task<IEnumerable<GstcalculationMaster>> GetAllGstMaster();
        public Task<GstcalculationMaster?> GetGstMasterById(int srNo);

        public Task RemoveGstMaster(GstcalculationMaster GstcalculationMasterEntity);
    }
}
