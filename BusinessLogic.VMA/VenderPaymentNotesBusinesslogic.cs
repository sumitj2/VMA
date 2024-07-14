using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.Abstraction.VMA.Models;
using Database.Abstraction.VMA.Contract;
using Database.VMA.Entities;

namespace Database.VMA.Repositories
{
    public class VenderPaymentNotesBusinesslogic : IVenderPaymentNotesBusinessLogic
    {
        private IVenderPaymentNotesRepository _venderPaymentNotesRepository;
        private IInvoiceDetailsBusinessLogic _invoiceDetailsBusinessLogic;
        public VenderPaymentNotesBusinesslogic(IVenderPaymentNotesRepository venderPaymentNotesRepository, IInvoiceDetailsBusinessLogic invoiceDetailsBusinessLogic)
        {
            _venderPaymentNotesRepository = venderPaymentNotesRepository;
            _invoiceDetailsBusinessLogic = invoiceDetailsBusinessLogic;
        }

        public async Task AddPaymentNotes(VenderPaymentNoteModel paymentNotesModel)
        {
            VenderPaymentNote vendorEntity = new()
            {
                CreatedBy = paymentNotesModel.CreatedBy,
                CreatedDate = DateTime.Now,
                IsActive = paymentNotesModel.IsActive,
                NoteId = paymentNotesModel.NoteId,
                PaymentNoteDate = (DateTime)paymentNotesModel.PaymentNoteDate,
                PaymentNoteNo = paymentNotesModel.PaymentNoteNo,
                FkVendorId=paymentNotesModel.FkVendorId
            };
            await _venderPaymentNotesRepository.AddVendorPaymentNotes(vendorEntity);
        }
        public async Task EditUpdatePaymentNotes(VenderPaymentNoteModel paymentNotesModel)
        {
            var paymentNotesEntity = await _venderPaymentNotesRepository.GetVendorsPaymentNoteById(paymentNotesModel.NoteId);

            if (paymentNotesEntity != null)
            {
                paymentNotesEntity.LastUpdateBy = paymentNotesModel?.LastUpdateBy;
                paymentNotesEntity.LastUpdatedDate = paymentNotesModel?.LastUpdatedDate;
                paymentNotesEntity.IsActive = paymentNotesModel?.IsActive;
                paymentNotesEntity.PaymentNoteNo = paymentNotesModel?.PaymentNoteNo != null ? paymentNotesModel.PaymentNoteNo : "";
                paymentNotesEntity.PaymentNoteDate =Convert.ToDateTime(paymentNotesModel?.PaymentNoteDate);
                paymentNotesEntity.NoteId = paymentNotesModel!.NoteId;
                paymentNotesEntity.FkVendorId=paymentNotesModel.FkVendorId;
                await _venderPaymentNotesRepository.EditUpdateVendorPaymentNotes(paymentNotesEntity);
            }
        }
        public async Task<IEnumerable<VenderPaymentNoteModel>> GetAllPaymentNotes()
        {
            var repositoryResult = await _venderPaymentNotesRepository.GetAllPaymentDetailsWithServiceDetails().ConfigureAwait(true);
            List<VenderPaymentNoteModel> paymentNoteModel = [];
            if (repositoryResult != null)
            {
                foreach (var paymentNote in repositoryResult)
                {
                    paymentNoteModel.Add(new VenderPaymentNoteModel()
                    {
                        CreatedBy = paymentNote.CreatedBy,
                        CreatedDate = paymentNote.CreatedDate,
                        IsActive = paymentNote.IsActive,
                        LastUpdateBy = paymentNote.LastUpdateBy,
                        LastUpdatedDate = paymentNote.LastUpdatedDate,
                        PaymentNoteNo = paymentNote.PaymentNoteNo,
                        PaymentNoteDate = paymentNote.PaymentNoteDate,
                        NoteId = paymentNote.NoteId,
                        VendorId = paymentNote.VendorId,
                        VendorName = paymentNote.VendorName,
                        FkVendorId = paymentNote.VendorId,
                        VendorServiceId = paymentNote.VendorServiceId,
                        VendorServiceName= paymentNote.VendorServiceName
                    });
                }
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
                NoteId = repositoryResult!.NoteId,
                PaymentNoteDate = repositoryResult.PaymentNoteDate,
                PaymentNoteNo = repositoryResult.PaymentNoteNo
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
                PaymentNoteNo = paymentNoteModel?.PaymentNoteNo!=null?paymentNoteModel.PaymentNoteNo:"",
                PaymentNoteDate =Convert.ToDateTime(paymentNoteModel?.PaymentNoteDate),
                NoteId = paymentNoteModel!.NoteId
            };

            await _venderPaymentNotesRepository.RemoveVendorPaymentNote(paymentNoteEntity);
        }
    }
}
