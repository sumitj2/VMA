using Database.Abstraction.VMA.Contract;
using Database.VMA.Entities;
using Database.VMA.Entities.CustomEntities;
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
        public async Task<List<PaymentNotesWithInvoiceServiceDetails>> GetAllPaymentDetailsWithServiceDetails()
        {
            var paymentNoteWithAllDetails = from paymentNote in _context.VenderPaymentNotes
                                            join payment in _context.VendorPayments
                                            on paymentNote.FkVendorPaymentId equals payment.VendorPaymentId
                                            join invoice in _context.InvoiceDetails
                                            on paymentNote.FkInvoiceId equals invoice.InvoiceId
                                            join details in _context.VendorDetails
                                            on payment.FkVendorDetailId equals details.VendorDetailId
                                            join service in _context.VendorServices
                                            on details.FkVendorServiceId equals service.VendorServiceId
                                            where paymentNote.IsActive==true
                                            select new PaymentNotesWithInvoiceServiceDetails
                                            {
                                                CreatedBy = paymentNote.CreatedBy,
                                                CreatedDate = paymentNote.CreatedDate,
                                                IsActive = paymentNote.IsActive,
                                                LastUpdateBy = paymentNote.LastUpdateBy,
                                                LastUpdatedDate = paymentNote.LastUpdatedDate,
                                                PaymentCode = payment.PaymentCode,
                                                VendorServiceId = service.VendorServiceId,
                                                VendorServiceName = service.VendorServiceName,
                                                FkInvoiceId = paymentNote.FkInvoiceId,
                                                FkVendorPaymentId = paymentNote.FkVendorPaymentId,
                                                InvoiceDate = invoice.InvoiceDate,
                                                InvoiceId = invoice.InvoiceId,
                                                InvoiceNumber = invoice.InvoiceNumber,
                                                InvoiceParticulars = invoice.InvoiceParticulars,
                                                NoteId = paymentNote.NoteId,
                                                PaymentNoteDate = paymentNote.PaymentNoteDate,
                                                PaymentNoteNo = paymentNote.PaymentNoteNo,
                                                VendorPaymentId = payment.VendorPaymentId
                                            };
            return await paymentNoteWithAllDetails.ToListAsync();
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
