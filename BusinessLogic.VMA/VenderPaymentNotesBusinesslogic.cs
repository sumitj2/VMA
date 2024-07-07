using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.Abstraction.VMA.Models;
using Database.Abstraction.VMA.Contract;
using Database.VMA.Entities;

namespace Database.VMA.Repositories
{
    public class VenderPaymentNotesBusinesslogic : IVenderPaymentNotesBusinessLogic
    {
        private IVenderPaymentNotesRepository _venderPaymentNotesRepository;
        public VenderPaymentNotesBusinesslogic(IVenderPaymentNotesRepository venderPaymentNotesRepository)
        {
            _venderPaymentNotesRepository = venderPaymentNotesRepository;
        }

        public async Task AddPaymentNotes(VenderPaymentNoteModel paymentNotesModel)
        {
            VenderPaymentNote vendorEntity = new()
            {
                CreatedBy = paymentNotesModel.CreatedBy,
                CreatedDate = DateTime.Now,
                IsActive = paymentNotesModel.IsActive,
                LastUpdateBy = paymentNotesModel.LastUpdateBy,
                LastUpdatedDate = DateTime.Now,
                FkInvoiceId = paymentNotesModel.FkInvoiceId,
                FkVendorPaymentId = paymentNotesModel.FkVendorPaymentId,
                NoteId = paymentNotesModel.NoteId,
                PaymentNoteDate = paymentNotesModel.PaymentNoteDate,
                PaymentNoteNo= paymentNotesModel.PaymentNoteNo                
            };
            await _venderPaymentNotesRepository.AddVendorPaymentNotes(vendorEntity);
        }
        public async Task EditUpdatePaymentNotes(VenderPaymentNoteModel paymentNotesModel)
        {
            VenderPaymentNote paymentNoteEntity = new()
            {
                CreatedBy = paymentNotesModel?.CreatedBy,
                CreatedDate = paymentNotesModel?.CreatedDate,
                IsActive = paymentNotesModel?.IsActive,
                LastUpdateBy = paymentNotesModel?.LastUpdateBy,
                LastUpdatedDate = paymentNotesModel?.LastUpdatedDate,
                PaymentNoteNo= paymentNotesModel?.PaymentNoteNo,
                PaymentNoteDate= paymentNotesModel?.PaymentNoteDate,
                NoteId=paymentNotesModel!.NoteId,
                FkVendorPaymentId= paymentNotesModel?.FkVendorPaymentId,
                FkInvoiceId=paymentNotesModel?.FkInvoiceId,
               
            };
            await _venderPaymentNotesRepository.EditUpdateVendorPaymentNotes(paymentNoteEntity);

        }
        public async Task<IEnumerable<VenderPaymentNoteModel>> GetAllPaymentNotes()
        {
            var repositoryResult = await _venderPaymentNotesRepository.GetAllPaymentDetailsWithServiceDetails();
            List<VenderPaymentNoteModel> paymentNoteModel = [];
            foreach (var paymentNote in repositoryResult)
            {
                paymentNoteModel.Add(new VenderPaymentNoteModel()
                {
                    CreatedBy = paymentNote.CreatedBy,
                    CreatedDate = paymentNote.CreatedDate,
                    IsActive = paymentNote.IsActive,
                    LastUpdateBy = paymentNote.LastUpdateBy,
                    LastUpdatedDate = paymentNote.LastUpdatedDate,
                    PaymentNoteNo= paymentNote.PaymentNoteNo,
                    PaymentNoteDate= paymentNote.PaymentNoteDate,
                    FkInvoiceId = paymentNote.FkInvoiceId,
                    FkVendorPaymentId = paymentNote.FkVendorPaymentId,
                    NoteId=paymentNote.NoteId ,
                    VendorPaymentId= paymentNote.VendorPaymentId,
                    InvoiceParticulars= paymentNote.InvoiceParticulars,
                    InvoiceNumber= paymentNote.InvoiceNumber,
                    InvoiceId=paymentNote.InvoiceId,
                    InvoiceDate= paymentNote.InvoiceDate,
                    PaymentCode= paymentNote.PaymentCode,
                    VendorServiceId= paymentNote.VendorServiceId,
                    VendorServiceName= paymentNote.VendorServiceName
                });
            }
            return paymentNoteModel;
        }
        public async Task<VenderPaymentNoteModel?> GetPaymentNoteById(int vendorId)
        {
            var repositoryResult = await _venderPaymentNotesRepository.GetVendorsPaymentNoteById(vendorId);
            VenderPaymentNoteModel vendorModel = new()
            {
                CreatedBy = repositoryResult?.CreatedBy,
                CreatedDate = repositoryResult?.CreatedDate,
                IsActive = repositoryResult?.IsActive,
                LastUpdateBy = repositoryResult?.LastUpdateBy,
                LastUpdatedDate = repositoryResult?.LastUpdatedDate,
                FkInvoiceId= repositoryResult?.FkInvoiceId,
                FkVendorPaymentId= repositoryResult?.FkVendorPaymentId, 
                NoteId=repositoryResult!.NoteId,
                PaymentNoteDate = repositoryResult?.PaymentNoteDate,
                PaymentNoteNo=repositoryResult?.PaymentNoteNo
            };
            return vendorModel;

        }
        public async Task RemovePaymentNote(VenderPaymentNoteModel paymentNoteModel)
        {
            VenderPaymentNote paymentNoteEntity = new()
            {
                CreatedBy = paymentNoteModel?.CreatedBy,
                CreatedDate = paymentNoteModel?.CreatedDate,
                IsActive = paymentNoteModel?.IsActive,
                LastUpdateBy = paymentNoteModel?.LastUpdateBy,
                LastUpdatedDate = paymentNoteModel?.LastUpdatedDate,
                PaymentNoteNo = paymentNoteModel?.PaymentNoteNo,
                PaymentNoteDate= paymentNoteModel?.PaymentNoteDate,
                FkInvoiceId = paymentNoteModel?.FkInvoiceId,
                FkVendorPaymentId = paymentNoteModel?.FkVendorPaymentId,
                NoteId=paymentNoteModel!.NoteId
            };

            await _venderPaymentNotesRepository.RemoveVendorPaymentNote(paymentNoteEntity);
        }
    }
}
