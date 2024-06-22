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
    public class VenderPaymentNotesRepository : IVenderPaymentNotesRepository
    {
        private readonly VendorManagementDbContext _context;
        public VenderPaymentNotesRepository(VendorManagementDbContext context)
        {
            _context = context;
        }
        public async Task AddVendorPaymentNotes(VenderPaymentNote VenderPaymentNoteEntity)
        {
            await _context.AddAsync(VenderPaymentNoteEntity).ConfigureAwait(true);
            await _context.SaveChangesAsync();
        }
        public async Task EditUpdateVendorPaymentNotes(VenderPaymentNote VenderPaymentNoteEntity)
        {
            var result = await GetVendorsPaymentNoteById(VenderPaymentNoteEntity.NoteId);
            if (result != null)
            {
                _context.VenderPaymentNotes.Update(result);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<VenderPaymentNote>> GetAllVendorsPaymentNotes()
        {
            return await _context.VenderPaymentNotes.Where(x => x.IsActive == true).ToListAsync();
        }
        public async Task<VenderPaymentNote?> GetVendorsPaymentNoteById(int vendorId)
        {
            return await _context.VenderPaymentNotes.Where(x => x.IsActive == true && x.NoteId == vendorId).FirstOrDefaultAsync();
        }

        public async Task RemoveVendorPaymentNote(VenderPaymentNote VenderPaymentNoteEntity)
        {
            _context.VenderPaymentNotes.Remove(VenderPaymentNoteEntity);
            await _context.SaveChangesAsync();
        }
    }
}
