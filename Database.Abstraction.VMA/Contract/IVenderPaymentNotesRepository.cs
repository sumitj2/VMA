using Database.VMA.Entities;
using Database.VMA.Entities.CustomEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Abstraction.VMA.Contract
{
    public interface IVenderPaymentNotesRepository
    {

        public Task AddVendorPaymentNotes(VenderPaymentNote VenderPaymentNoteEntity);
        public Task EditUpdateVendorPaymentNotes(VenderPaymentNote VenderPaymentNoteEntity);
        public Task<IEnumerable<VenderPaymentNote>> GetAllVendorsPaymentNotes();
        public Task<VenderPaymentNote?> GetVendorsPaymentNoteById(int vendorId);
        public Task RemoveVendorPaymentNote(VenderPaymentNote VenderPaymentNoteEntity);
        public Task<List<PaymentNotesWithInvoiceServiceDetails>> GetAllPaymentDetailsWithServiceDetails();
        public Task<List<ExportPaymentNoteData>> GetAllPaymentDetailsWithServiceDetailsToExport();
    }
}
