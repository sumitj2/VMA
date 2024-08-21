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

        public Task AddVendorPaymentNotes(VendorPaymentNote VenderPaymentNoteEntity);
        public Task EditUpdateVendorPaymentNotes(VendorPaymentNote VenderPaymentNoteEntity);
        public Task<IEnumerable<VendorPaymentNote>> GetAllVendorsPaymentNotes();
        public Task<VendorPaymentNote?> GetVendorsPaymentNoteByIVendorId(int? vendorId);
        public Task RemoveVendorPaymentNote(VendorPaymentNote VenderPaymentNoteEntity);
        public Task<List<PaymentNotesWithInvoiceServiceDetails>> GetAllPaymentDetailsWithServiceDetails();
        public Task<List<ExportPaymentNoteData>> GetAllPaymentDetailsWithServiceDetailsToExport(string? finacialYear);
        Task<List<CreateWordDocumentPaymentNote>> GetAllServicePayments(List<string> serviceNameList, string? financialYear, string vendorName,string paymentNoteNo);
        Task<int> GetAllVendorsPaymentNotesCount();
    }
}
