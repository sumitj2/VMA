using Database.Abstraction.VMA.Contract;
using Database.VMA.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.VMA.Repositories
{
    public class GstcalculationMasterRepository : IGstcalculationMasterRepository
    {
        private readonly VendorManagementDbContext _context;
        public GstcalculationMasterRepository(VendorManagementDbContext context)
        {
            _context = context;
        }

        public async Task AddGstMaster(GstcalculationMaster GstcalculationMasterEntity)
        {
            await _context.AddAsync(GstcalculationMasterEntity).ConfigureAwait(true);
            await _context.SaveChangesAsync();
        }
        public async Task EditUpdateGst(GstcalculationMaster GstcalculationMasterEntity)
        {
            var result = await GetGstMasterById(GstcalculationMasterEntity.SrNo);
            if (result != null)
            {
                _context.GstcalculationMasters.Update(result);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<GstcalculationMaster>> GetAllGstMaster()
        {
            return await _context.GstcalculationMasters.Where(x => x.IsActive == true).ToListAsync();
        }
        public async Task<GstcalculationMaster?> GetGstMasterById(int srNo)
        {
            return await _context.GstcalculationMasters.Where(x => x.IsActive == true && x.SrNo == srNo).FirstOrDefaultAsync();
        }

        public async Task RemoveGstMaster(GstcalculationMaster GstcalculationMasterEntity)
        {
            _context.GstcalculationMasters.Remove(GstcalculationMasterEntity);
            await _context.SaveChangesAsync();
        }
    }
}
