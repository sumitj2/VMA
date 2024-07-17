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
    public class VendorPaymentRepository : IVendorPaymentRepository
    {
        private readonly VendorManagementDbContext _context;
        public VendorPaymentRepository(VendorManagementDbContext context)
        {
            _context = context;
        }
        public async Task AddVendorPayment(VendorPayment VendorPaymentEntity)
        {
            await _context.AddAsync(VendorPaymentEntity).ConfigureAwait(true);
            await _context.SaveChangesAsync();
        }
        public async Task EditUpdateVendorPayment(VendorPayment VendorPaymentEntity)
        {
            _context.VendorPayments.Update(VendorPaymentEntity);
            await _context.SaveChangesAsync();
        }
        public async Task<List<VendorPaymentWithService>> GetPaymentDetailsWithServiceDetailsByVednorDetailId(int? vendorDetailId)
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
                                      where payment.FkVendorDetailId == vendorDetailId && payment.IsActive == true
                                      select new VendorPaymentWithService
                                      {
                                          CreatedBy = payment.CreatedBy,
                                          CreatedDate = payment.CreatedDate,
                                          IsActive = payment.IsActive,
                                          LastUpdateBy = payment.LastUpdateBy,
                                          LastUpdatedDate = payment.LastUpdatedDate,
                                          FkVendorDetailId = payment.FkVendorDetailId,
                                          BankBranchName = payment.BankBranchName,
                                          IsPaymentForBranch = payment.IsPaymentForBranch,
                                          PaymentCode = payment.PaymentCode,
                                          VendorPaymentAmount = payment.VendorPaymentAmount,
                                          VendorPaymentCgst = payment.VendorPaymentCgst,
                                          VendorPaymentDate = payment.VendorPaymentDate,
                                          VendorPaymentId = payment.VendorPaymentId,
                                          VendorPaymentIsGst = payment.VendorPaymentIsGst,
                                          VendorPaymentIsTdsapplicable = payment.VendorPaymentIsTdsapplicable,
                                          Notes = payment.Notes,
                                          VendorPaymentRtgsAmount = payment.VendorPaymentRtgsAmount,
                                          VendorPaymentRtgsDate = payment.VendorPaymentRtgsDate,
                                          VendorPaymentSgst = payment.VendorPaymentSgst,
                                          VendorPaymentTdsamount = payment.VendorPaymentTdsamount,
                                          VendorPaymentTotalAmountPaid = payment.VendorPaymentTotalAmountPaid,
                                          VendorPaymentUtrnumber = payment.VendorPaymentUtrnumber,
                                          PaymentYear = payment.PaymentYear,
                                          ServicePaymentType = details.ServicePaymentType,
                                          ServiceSantionAmount = details.ServiceSantionAmount,
                                          VendorServiceId = service.VendorServiceId,
                                          VendorServiceName = service.VendorServiceName,
                                          FkInvoiceId = payment.FkInvoiceId,
                                          FkGstmasterSrNo = payment.FkGstmasterSrNo,
                                          VendorId = vendor.VendorId,
                                          FkNoteId = payment.FkNoteId,
                                          VendorName = vendor.VendorName,
                                          VendorPaymentIgst = payment.VendorPaymentIgst,
                                          InvoiceDate = invoice.InvoiceDate,
                                          InvoiceId = invoice.InvoiceId,
                                          InvoiceNumber = invoice.InvoiceNumber,
                                          InvoiceParticulars = invoice.InvoiceParticulars,
                                          NoteId = paymentNote.NoteId,
                                          PaymentNoteNo = paymentNote.PaymentNoteNo
                                      };
            return await productsWithVendors.ToListAsync();
        }
        public async Task<List<VendorPaymentWithService>> GetAllPaymentDetailsWithServiceDetails()
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
                                      select new VendorPaymentWithService
                                      {
                                          CreatedBy = payment.CreatedBy,
                                          CreatedDate = payment.CreatedDate,
                                          IsActive = payment.IsActive,
                                          LastUpdateBy = payment.LastUpdateBy,
                                          LastUpdatedDate = payment.LastUpdatedDate,
                                          FkVendorDetailId = payment.FkVendorDetailId,
                                          BankBranchName = payment.BankBranchName,
                                          IsPaymentForBranch = payment.IsPaymentForBranch,
                                          PaymentCode = payment.PaymentCode,
                                          VendorPaymentAmount = payment.VendorPaymentAmount,
                                          VendorPaymentCgst = payment.VendorPaymentCgst,
                                          VendorPaymentDate = payment.VendorPaymentDate,
                                          VendorPaymentId = payment.VendorPaymentId,
                                          VendorPaymentIsGst = payment.VendorPaymentIsGst,
                                          VendorPaymentIsTdsapplicable = payment.VendorPaymentIsTdsapplicable,
                                          Notes = payment.Notes,
                                          VendorPaymentRtgsAmount = payment.VendorPaymentRtgsAmount,
                                          VendorPaymentRtgsDate = payment.VendorPaymentRtgsDate,
                                          VendorPaymentSgst = payment.VendorPaymentSgst,
                                          VendorPaymentTdsamount = payment.VendorPaymentTdsamount,
                                          VendorPaymentTotalAmountPaid = payment.VendorPaymentTotalAmountPaid,
                                          VendorPaymentUtrnumber = payment.VendorPaymentUtrnumber,
                                          PaymentYear = payment.PaymentYear,
                                          ServicePaymentType = details.ServicePaymentType,
                                          ServiceSantionAmount = details.ServiceSantionAmount,
                                          VendorServiceId = service.VendorServiceId,
                                          VendorServiceName = service.VendorServiceName,
                                          FkInvoiceId = payment.FkInvoiceId,
                                          FkGstmasterSrNo = payment.FkGstmasterSrNo,
                                          VendorId = vendor.VendorId,
                                          FkNoteId = payment.FkNoteId,
                                          VendorName = vendor.VendorName,
                                          VendorPaymentIgst = payment.VendorPaymentIgst,
                                          InvoiceDate = invoice.InvoiceDate,
                                          InvoiceId = invoice.InvoiceId,
                                          InvoiceNumber = invoice.InvoiceNumber,
                                          InvoiceParticulars = invoice.InvoiceParticulars,
                                          NoteId = paymentNote.NoteId,
                                          PaymentNoteNo = paymentNote.PaymentNoteNo
                                      };
            return await productsWithVendors.ToListAsync();
        }
        public async Task<IEnumerable<VendorPayment>> GetAllVendorPayment()
        {
            return await _context.VendorPayments.Where(x => x.IsActive == true).ToListAsync();
        }
        public async Task<VendorPayment?> GetVendorPaymentById(int vendorDetailId)
        {
            return await _context.VendorPayments.Where(x => x.IsActive == true && x.VendorPaymentId == vendorDetailId).FirstOrDefaultAsync();
        }

        public async Task RemoveVendorPayment(VendorPayment VendorPaymentEntity)
        {
            _context.VendorPayments.Remove(VendorPaymentEntity);
            await _context.SaveChangesAsync();
        }
    }
}
