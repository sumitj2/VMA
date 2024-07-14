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
            var result = await GetVendorsPaymentNoteByIVendorId(VenderPaymentNoteEntity.NoteId);
            if (result != null)
            {
                _context.VenderPaymentNotes.Update(result);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<PaymentNotesWithInvoiceServiceDetails>> GetAllPaymentDetailsWithServiceDetails()
        {
            var paymentNoteWithAllDetails = from paymentNote in _context.VenderPaymentNotes
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

                                            };
            return await paymentNoteWithAllDetails.ToListAsync();
            return null;
        }

        public async Task<List<ExportPaymentNoteData>> GetAllPaymentDetailsWithServiceDetailsToExport()
        {
            //var paymentNoteWithAllDetails = from paymentNote in _context.VenderPaymentNotes
            //                                join payment in _context.VendorPayments
            //                                on paymentNote.NoteId equals payment.VendorPaymentId
            //                                join invoice in _context.InvoiceDetails
            //                                on paymentNote.FkInvoiceId equals invoice.InvoiceId
            //                                join details in _context.VendorDetails
            //                                on payment.FkVendorDetailId equals details.VendorDetailId
            //                                join service in _context.VendorServices
            //                                on details.FkVendorServiceId equals service.VendorServiceId
            //                                join vendor in _context.Vendors
            //                                on service.FkVendorId equals vendor.VendorId
            //                                where paymentNote.IsActive == true
            //                                select new ExportPaymentNoteData
            //                                {

            //                                    VendorServiceName = service.VendorServiceName,

            //                                    InvoiceDate = invoice.InvoiceDate,

            //                                    InvoiceNumber = invoice.InvoiceNumber,
            //                                    InvoiceParticulars = invoice.InvoiceParticulars,

            //                                    PaymentNoteDate = paymentNote.PaymentNoteDate,
            //                                    PaymentNoteNo = paymentNote.PaymentNoteNo,
            //                                    ServiceSantionAmount = details.ServiceSantionAmount,
            //                                    ServiceSantionedBy = details.ServiceSantionedBy,
            //                                    VendorPaymentYearRange = payment.VendorPaymentYear,
            //                                    VendorPaymentUtrnumber = payment.VendorPaymentUtrnumber,
            //                                    VendorDetailCategory = details.VendorDetailCategory,
            //                                    ServiceType = details.ServiceType,
            //                                    VendorPaymentAmount = payment.VendorPaymentAmount,
            //                                    VendorPaymentRtgsAmount = payment.VendorPaymentRtgsAmount,
            //                                    VendorPaymentTdsamount = payment.VendorPaymentTdsamount,
            //                                    VendorPaymentRtgsDate = payment.VendorPaymentRtgsDate,
            //                                    VendorName = vendor.VendorName


            //                                };
            //return await paymentNoteWithAllDetails.ToListAsync();
            return null;
        }
        public async Task<IEnumerable<VenderPaymentNote>> GetAllVendorsPaymentNotes()
        {
            return await _context.VenderPaymentNotes.Where(x => x.IsActive == true).ToListAsync();
        }
        public async Task<VenderPaymentNote?> GetVendorsPaymentNoteByIVendorId(int vendorId)
        {
            return await _context.VenderPaymentNotes.Where(x => x.IsActive == true && x.FkVendorId == vendorId).FirstOrDefaultAsync();
        }

        public async Task RemoveVendorPaymentNote(VenderPaymentNote VenderPaymentNoteEntity)
        {
            _context.VenderPaymentNotes.Remove(VenderPaymentNoteEntity);
            await _context.SaveChangesAsync();
        }
    }
}
