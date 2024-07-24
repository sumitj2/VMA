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
        public async Task AddVendorPaymentNotes(VendorPaymentNote VenderPaymentNoteEntity)
        {
            await _context.AddAsync(VenderPaymentNoteEntity).ConfigureAwait(true);
            await _context.SaveChangesAsync();
        }
        public async Task EditUpdateVendorPaymentNotes(VendorPaymentNote VenderPaymentNoteEntity)
        {
            var result = await GetVendorsPaymentNoteByIVendorId(VenderPaymentNoteEntity.NoteId);
            if (result != null)
            {
                _context.VendorPaymentNotes.Update(result);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<PaymentNotesWithInvoiceServiceDetails>> GetAllPaymentDetailsWithServiceDetails()
        {
            var paymentNoteWithAllDetails = from paymentNote in _context.VendorPaymentNotes
                                            join vendor in _context.Vendors
                                            on paymentNote.FkVendorId equals vendor.VendorId
                                            join vendorService in _context.VendorServices
                                            on vendor.VendorId equals vendorService.FkVendorId
                                            where paymentNote.IsActive == true
                                            select new PaymentNotesWithInvoiceServiceDetails
                                            {
                                                CreatedBy = paymentNote.CreatedBy,
                                                CreatedDate = paymentNote.CreatedDate,
                                                IsActive = paymentNote.IsActive,
                                                LastUpdateBy = paymentNote.LastUpdateBy,
                                                LastUpdatedDate = paymentNote.LastUpdatedDate,

                                                VendorServiceId = vendorService.VendorServiceId,
                                                VendorServiceName = vendorService.VendorServiceName,

                                                FkVendorId = vendorService.FkVendorId,
                                                VendorName = vendor.VendorName,
                                                VendorId = vendor.VendorId,
                                                NoteId = paymentNote.NoteId,
                                                PaymentNoteDate = paymentNote.PaymentNoteDate,
                                                PaymentNoteNo = paymentNote.PaymentNoteNo,
                                                PaymentNoteYear=paymentNote.PaymentNoteYear

                                            };
            return await paymentNoteWithAllDetails.ToListAsync();
        }

        public async Task<List<ExportPaymentNoteData>> GetAllPaymentDetailsWithServiceDetailsToExport()
        {
            var productsWithVendors = from payment in _context.VendorPayments
                                      join details in _context.VendorDetails
                                      on payment.FkVendorDetailId equals details.VendorDetailId
                                      join service in _context.VendorServices
                                      on details.FkVendorServiceId equals service.VendorServiceId
                                      join vendor in _context.Vendors
                                      on details.FkVendorId equals vendor.VendorId
                                      join invoice in _context.InvoiceDetails
                                      on payment.FkInvoiceId equals invoice.InvoiceId
                                      join paymentNote in _context.VendorPaymentNotes
                                      on payment.FkNoteId equals paymentNote.NoteId
                                      where payment.IsActive == true

                                      select new ExportPaymentNoteData
                                      {

                                          VendorServiceName = service.VendorServiceName,

                                          InvoiceDate = invoice.InvoiceDate,

                                          InvoiceNumber = invoice.InvoiceNumber,
                                          InvoiceParticulars = invoice.InvoiceParticulars,

                                          PaymentNoteDate = paymentNote.PaymentNoteDate,
                                          PaymentNoteNo = paymentNote.PaymentNoteNo,
                                          ServiceSantionAmount = details.ServiceSantionAmount,
                                          ServiceSantionedBy = details.ServiceSantionedBy,
                                          VendorPaymentYearRange = payment.PaymentYear,
                                          VendorPaymentUtrnumber = payment.VendorPaymentUtrnumber,
                                          VendorDetailCategory = details.VendorDetailCategory,
                                          ServiceType = details.ServiceType,
                                          VendorPaymentAmount = payment.VendorPaymentAmount,
                                          VendorPaymentRtgsAmount = payment.VendorPaymentRtgsAmount,
                                          VendorPaymentTdsamount = payment.VendorPaymentTdsamount,
                                          VendorPaymentRtgsDate = payment.VendorPaymentRtgsDate,
                                          VendorName = vendor.VendorName,
                                          VendorPaymentDate = payment.VendorPaymentDate,
                                      };
            return await productsWithVendors.ToListAsync();

        }
        public async Task<IEnumerable<VendorPaymentNote>> GetAllVendorsPaymentNotes()
        {
            return await _context.VendorPaymentNotes.Where(x => x.IsActive == true).ToListAsync();
        }
        public async Task<VendorPaymentNote?> GetVendorsPaymentNoteByIVendorId(int vendorId)
        {
            return await _context.VendorPaymentNotes.Where(x => x.IsActive == true && x.FkVendorId == vendorId).FirstOrDefaultAsync();
        }

        public async Task RemoveVendorPaymentNote(VendorPaymentNote VenderPaymentNoteEntity)
        {
            _context.VendorPaymentNotes.Remove(VenderPaymentNoteEntity);
            await _context.SaveChangesAsync();
        }
    }
}
