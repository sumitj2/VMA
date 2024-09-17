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
            VendorPaymentNote vendorEntity = new()
            {
                CreatedBy = paymentNotesModel.CreatedBy,
                CreatedDate = DateTime.Now,
                IsActive = paymentNotesModel.IsActive,
                //NoteId = paymentNotesModel.NoteId,
                PaymentNoteDate = Convert.ToDateTime(paymentNotesModel.PaymentNoteDate),
                PaymentNoteNo = paymentNotesModel.PaymentNoteNo,
                FkVendorId = paymentNotesModel.FkVendorId,
                PaymentNoteYear = paymentNotesModel.PaymentNoteYear,
                FkVendorDetailId = paymentNotesModel.FkVendorDetailId,
            };
            await _venderPaymentNotesRepository.AddVendorPaymentNotes(vendorEntity);
        }
        public async Task EditUpdatePaymentNotes(VenderPaymentNoteModel paymentNotesModel)
        {
            var paymentNotesEntity = await _venderPaymentNotesRepository.GetVendorsPaymentNoteByNoteId(paymentNotesModel?.NoteId);

            //make inactive
            if (paymentNotesEntity != null)
            {                
                paymentNotesEntity.IsActive = false;
                await _venderPaymentNotesRepository.EditUpdateVendorPaymentNotes(paymentNotesEntity).ConfigureAwait(true);
            }
            //add fresh entry
            paymentNotesModel.LastUpdatedDate=DateTime.Now;
            await AddPaymentNotes(paymentNotesModel);
        }
        public async Task<int> GetAllVendorsPaymentNotesCount()
        {
            return await _venderPaymentNotesRepository.GetAllVendorsPaymentNotesCount();
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
                        PaymentNoteDate = paymentNote.PaymentNoteDate.ToShortDateString(),
                        NoteId = paymentNote.NoteId,
                        VendorId = paymentNote.VendorId,
                        VendorName = paymentNote.VendorName,
                        FkVendorId = paymentNote.VendorId,
                        VendorServiceId = paymentNote.VendorServiceId,
                        VendorServiceName = paymentNote.VendorServiceName,
                        PaymentNoteYear = paymentNote.PaymentNoteYear,
                        FkVendorDetailId=paymentNote.FkVendorDetailId
                    });
                }
            }
            return paymentNoteModel.OrderBy(x=>x.PaymentNoteNo);
        }
        public async Task<VenderPaymentNoteModel?> GetPaymentNoteByVendorId(int vendorId)
        {
            var repositoryResult = await _venderPaymentNotesRepository.GetVendorsPaymentNoteByNoteId(vendorId).ConfigureAwait(true);
            if (repositoryResult != null)
            {
                VenderPaymentNoteModel vendorModel = new()
                {
                    CreatedBy = repositoryResult?.CreatedBy,
                    CreatedDate = repositoryResult?.CreatedDate,
                    IsActive = repositoryResult?.IsActive,
                    LastUpdateBy = repositoryResult?.LastUpdateBy,
                    LastUpdatedDate = repositoryResult?.LastUpdatedDate,
                    NoteId = repositoryResult!.NoteId,
                    PaymentNoteDate = repositoryResult.PaymentNoteDate.ToShortDateString(),
                    PaymentNoteNo = repositoryResult.PaymentNoteNo,
                    PaymentNoteYear = repositoryResult.PaymentNoteYear,
                    FkVendorDetailId=repositoryResult?.FkVendorDetailId
                };
                return vendorModel;
            }
            return null;
        }
        public async Task<VenderPaymentNoteModel?> GetPaymentNoteByVendorIdAndDetailServiceId(int? vendorId,int? detailServceId, string paymentNoteYear)
        {
            var repositoryResult = await _venderPaymentNotesRepository.GetVendorsPaymentNoteByVendorIdAndDetailServiceId(vendorId, detailServceId,paymentNoteYear).ConfigureAwait(true);
            if (repositoryResult != null)
            {
                VenderPaymentNoteModel vendorModel = new()
                {
                    CreatedBy = repositoryResult?.CreatedBy,
                    CreatedDate = repositoryResult?.CreatedDate,
                    IsActive = repositoryResult?.IsActive,
                    LastUpdateBy = repositoryResult?.LastUpdateBy,
                    LastUpdatedDate = repositoryResult?.LastUpdatedDate,
                    NoteId = repositoryResult!.NoteId,
                    PaymentNoteDate = repositoryResult.PaymentNoteDate.ToShortDateString(),
                    PaymentNoteNo = repositoryResult.PaymentNoteNo,
                    PaymentNoteYear = repositoryResult.PaymentNoteYear,
                    FkVendorDetailId = repositoryResult?.FkVendorDetailId
                };
                return vendorModel;
            }
            return null;

        }
        public async Task RemovePaymentNote(VenderPaymentNoteModel paymentNoteModel)
        {
            VendorPaymentNote paymentNoteEntity = new()
            {
                CreatedBy = paymentNoteModel?.CreatedBy,
                CreatedDate = paymentNoteModel?.CreatedDate,
                IsActive = paymentNoteModel?.IsActive,
                LastUpdateBy = paymentNoteModel?.LastUpdateBy,
                LastUpdatedDate = paymentNoteModel?.LastUpdatedDate,
                PaymentNoteNo = paymentNoteModel?.PaymentNoteNo != null ? paymentNoteModel.PaymentNoteNo : "",
                PaymentNoteDate = Convert.ToDateTime(paymentNoteModel?.PaymentNoteDate),
                PaymentNoteYear = paymentNoteModel?.PaymentNoteYear,
                NoteId = paymentNoteModel!.NoteId,
                FkVendorDetailId = paymentNoteModel?.FkVendorDetailId ,
                FkVendorId= paymentNoteModel?.FkVendorId,   
            };

            await _venderPaymentNotesRepository.RemoveVendorPaymentNote(paymentNoteEntity);
        }
    }
}
