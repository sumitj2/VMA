using BusinessLogic.Abstraction.VMA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Abstraction.VMA.Contract
{
    public interface IVenderPaymentNotesBusinessLogic
    {
        public Task AddPaymentNotes(VenderPaymentNoteModel paymentNotesModel);
        public Task EditUpdatePaymentNotes(VenderPaymentNoteModel paymentNotesModel);
        public Task<IEnumerable<VenderPaymentNoteModel>> GetAllPaymentNotes();
        public Task<VenderPaymentNoteModel?> GetPaymentNoteById(int vendorId);
        public Task RemovePaymentNote(VenderPaymentNoteModel paymentNoteModel);
    }
}
