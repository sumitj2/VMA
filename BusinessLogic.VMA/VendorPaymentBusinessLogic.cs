using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.Abstraction.VMA.Models;
using Database.Abstraction.VMA.Contract;
using Database.VMA.Entities;

namespace Database.VMA.Repositories
{
    public class VendorPaymentBusinessLogic : IVendorPaymentBusinessLogic
    {
        private IVendorPaymentRepository _vendorPaymentRepository;
        private readonly IInvoiceDetailsBusinessLogic _invoiceDetailsBusinessLogic;
        public VendorPaymentBusinessLogic(IVendorPaymentRepository vendorPaymentRepository, IInvoiceDetailsBusinessLogic invoiceDetailsBusinessLogic)
        {
            _vendorPaymentRepository = vendorPaymentRepository;
            _invoiceDetailsBusinessLogic = invoiceDetailsBusinessLogic;
        }

        public async Task<string> GeneratePaymentCode(VendorDetailModel? vendorDetailModel)
        {
            string paymentcode = "";
            //int counter = 1;
            //var res = await GetAllVendorPayment().ConfigureAwait(false);
            //var checkPaymentForService = res.Where(x => x.VendorServiceId == vendorDetailModel?.FkVendorServiceId);
            //if (!checkPaymentForService.Any())
            //{
            //    paymentcode = string.Join("_", vendorDetailModel?.VendorServiceName?.Replace(" ", ""), vendorDetailModel?.ServicePaymentType, counter);

            //}
            //else
            //{
            //    //Get all payment code 
            //    var paymentCode = checkPaymentForService.OrderBy(x => x.CreatedDate)?.FirstOrDefault()?.PaymentCode;

            //    //Split payment code by "_"
            //    var result = paymentCode?.Split("_");

            //    //Get last number of splited value to increase counter
            //    //0-ServiceName
            //    //1-PaymentType
            //    //2-Counter

            //    if (result != null)
            //    {
            //        paymentcode = string.Join("_", result[0], result[1], Convert.ToInt32(result[2]) + 1);
            //    }
            //    //Need to write logic on payment method
            //}

            return paymentcode;
        }



        public async Task AddVendorPayment(VendorPaymentModel VendorPaymentModel)
        {
            InvoiceDetailsModel invoiceDetailsModel = new InvoiceDetailsModel() 
            {
                CreatedBy = VendorPaymentModel.CreatedBy,
                CreatedDate = DateTime.UtcNow,
                IsActive=true,
                InvoiceDate = VendorPaymentModel.InvoiceDate,
                InvoiceNumber = VendorPaymentModel.InvoiceNumber,
                InvoiceParticulars=VendorPaymentModel.InvoiceParticulars                
            };
            var invoiceId = await _invoiceDetailsBusinessLogic.AddInvoice(invoiceDetailsModel);
            VendorPayment vendorPayment = new()
            {
                BankBranchName = VendorPaymentModel.BankBranchName,
                CreatedBy = VendorPaymentModel.CreatedBy,
                CreatedDate = DateTime.UtcNow,
                FkVendorDetailId = (int)VendorPaymentModel.FkVendorDetailId,
                IsActive = (bool)VendorPaymentModel.IsActive,
                LastUpdateBy = VendorPaymentModel.LastUpdateBy,
                LastUpdatedDate = DateTime.UtcNow,
                VendorPaymentAmount = VendorPaymentModel.VendorPaymentAmount,
                VendorPaymentCgst = VendorPaymentModel.VendorPaymentCgst,
                VendorPaymentDate = (DateOnly)VendorPaymentModel.VendorPaymentDate,
                VendorPaymentId = VendorPaymentModel.VendorPaymentId,
                VendorPaymentIsGst = VendorPaymentModel.VendorPaymentIsGst,
                Notes = VendorPaymentModel.Notes,
                VendorPaymentRtgsAmount = VendorPaymentModel.VendorPaymentRtgsAmount,
                VendorPaymentRtgsDate = VendorPaymentModel.VendorPaymentRtgsDate,
                VendorPaymentSgst = VendorPaymentModel.VendorPaymentSgst,
                VendorPaymentTdsamount = VendorPaymentModel.VendorPaymentTdsamount,
                VendorPaymentTotalAmountPaid = VendorPaymentModel.VendorPaymentTotalAmountPaid,
                VendorPaymentUtrnumber = VendorPaymentModel.VendorPaymentUtrnumber,
                PaymentYear = VendorPaymentModel.PaymentYear,
                PaymentCode = VendorPaymentModel.PaymentCode,
                FkNoteId = VendorPaymentModel.FkNoteId,
                FkGstmasterSrNo = VendorPaymentModel.FkGstmasterSrNo,
                VendorPaymentIsTdsapplicable = VendorPaymentModel.VendorPaymentIsTdsapplicable,
                IsPaymentForBranch = VendorPaymentModel.IsPaymentForBranch,
                VendorPaymentIgst = VendorPaymentModel.VendorPaymentIgst,
                FkInvoiceId = invoiceId

            };
            await _vendorPaymentRepository.AddVendorPayment(vendorPayment);
        }
        public async Task EditUpdateVendorPayment(VendorPaymentModel VendorPaymentEntity)
        {
            var entity = await _vendorPaymentRepository.GetVendorPaymentById(VendorPaymentEntity.VendorPaymentId);
            var invoiceEntity = await _invoiceDetailsBusinessLogic.GetInvoiceById(VendorPaymentEntity.InvoiceId);

            if (invoiceEntity != null)
            {
                invoiceEntity.InvoiceParticulars = VendorPaymentEntity.InvoiceParticulars;
                invoiceEntity.InvoiceNumber = VendorPaymentEntity.InvoiceNumber;
                invoiceEntity.InvoiceDate = VendorPaymentEntity.InvoiceDate;
                invoiceEntity.LastUpdateBy = VendorPaymentEntity.LastUpdateBy;
                invoiceEntity.LastUpdatedDate = VendorPaymentEntity.LastUpdatedDate;
                invoiceEntity.IsActive = true;

                await _invoiceDetailsBusinessLogic.EditUpdateInvoice(invoiceEntity);
            }

            if (entity != null)
            {
                entity.PaymentYear = VendorPaymentEntity.PaymentYear;
                entity.FkNoteId = VendorPaymentEntity.FkNoteId;
                entity.FkVendorDetailId = VendorPaymentEntity.FkVendorDetailId;
                entity.Notes = VendorPaymentEntity.Notes;

                entity.VendorPaymentDate = VendorPaymentEntity.VendorPaymentDate;
                entity.VendorPaymentAmount = VendorPaymentEntity.VendorPaymentAmount;
                entity.VendorPaymentTotalAmountPaid = VendorPaymentEntity.VendorPaymentTotalAmountPaid;

                entity.VendorPaymentIsGst = VendorPaymentEntity.VendorPaymentIsGst;
                entity.FkGstmasterSrNo = VendorPaymentEntity.FkGstmasterSrNo;
                entity.VendorPaymentIsTdsapplicable = VendorPaymentEntity.VendorPaymentIsTdsapplicable;
                entity.IsPaymentForBranch = VendorPaymentEntity.IsPaymentForBranch;
                entity.BankBranchName = VendorPaymentEntity.BankBranchName;

                entity.VendorPaymentSgst = VendorPaymentEntity.VendorPaymentSgst;
                entity.VendorPaymentCgst = VendorPaymentEntity.VendorPaymentCgst;
                entity.VendorPaymentIgst = VendorPaymentEntity.VendorPaymentIgst;

                entity.VendorPaymentTdsamount = VendorPaymentEntity.VendorPaymentTdsamount;
                entity.VendorPaymentUtrnumber = VendorPaymentEntity.VendorPaymentUtrnumber;
                entity.VendorPaymentRtgsAmount = VendorPaymentEntity.VendorPaymentRtgsAmount;
                entity.VendorPaymentRtgsDate = VendorPaymentEntity.VendorPaymentRtgsDate;

                entity.CreatedBy = VendorPaymentEntity.CreatedBy;
                entity.CreatedDate = VendorPaymentEntity.CreatedDate;
               

                entity.VendorPaymentId = VendorPaymentEntity.VendorPaymentId;
                entity.PaymentCode = VendorPaymentEntity.PaymentCode;
                entity.FkInvoiceId = (int)VendorPaymentEntity.InvoiceId;

                entity.IsActive = (bool)VendorPaymentEntity.IsActive;
                entity.LastUpdateBy = VendorPaymentEntity.LastUpdateBy;
                entity.LastUpdatedDate = VendorPaymentEntity.LastUpdatedDate;

                await _vendorPaymentRepository.EditUpdateVendorPayment(entity);
            }
        }
        public async Task<IEnumerable<VendorPaymentModel>> GetAllVendorPayment()
        {
            var repositoryResult = await _vendorPaymentRepository.GetAllPaymentDetailsWithServiceDetails();
            IList<VendorPaymentModel> result = [];
            foreach (var data in repositoryResult)
            {
                result.Add(new VendorPaymentModel()
                {
                    BankBranchName = data.BankBranchName,
                    CreatedBy = data.CreatedBy,
                    CreatedDate = data.CreatedDate,
                    FkVendorDetailId = data.FkVendorDetailId,
                    IsActive = data.IsActive,
                    LastUpdateBy = data.LastUpdateBy,
                    LastUpdatedDate = data.LastUpdatedDate,
                    VendorPaymentAmount = data.VendorPaymentAmount,
                    VendorPaymentCgst = data.VendorPaymentCgst,
                    VendorPaymentDate = data.VendorPaymentDate,
                    VendorPaymentId = data.VendorPaymentId,
                    VendorPaymentIsGst = data.VendorPaymentIsGst,
                    Notes = data.Notes,
                    VendorPaymentRtgsAmount = data.VendorPaymentRtgsAmount,
                    VendorPaymentRtgsDate = data.VendorPaymentRtgsDate,
                    VendorPaymentSgst = data.VendorPaymentSgst,
                    VendorPaymentTdsamount = data.VendorPaymentTdsamount,
                    VendorPaymentTotalAmountPaid = data.VendorPaymentTotalAmountPaid,
                    VendorPaymentUtrnumber = data.VendorPaymentUtrnumber,
                    PaymentYear = data.PaymentYear,
                    FkInvoiceId = data.FkInvoiceId,
                    FkNoteId = data.FkNoteId,
                    PaymentCode = data.PaymentCode,
                    IsPaymentForBranch = data.IsPaymentForBranch,
                    VendorPaymentIsTdsapplicable = data.VendorPaymentIsTdsapplicable,
                    VendorName = data.VendorName,
                    InvoiceNumber = data.InvoiceNumber,
                    FkGstmasterSrNo = data.FkGstmasterSrNo,
                    InvoiceDate = data.InvoiceDate,
                    InvoiceParticulars = data.InvoiceParticulars,
                    InvoiceId = data.InvoiceId,
                    ServicePaymentType = data.ServicePaymentType,
                    ServiceSantionAmount = data.ServiceSantionAmount,
                    VendorId = data.VendorId,
                    VendorPaymentIgst = data.VendorPaymentIgst,
                    VendorServiceId = data.VendorServiceId,
                    VendorServiceName = data.VendorServiceName,
                    PaymentNoteNo = data.PaymentNoteNo,
                    NoteId = data.NoteId
                });
            }
            return result;
        }
        public async Task<VendorPaymentModel?> GetVendorPaymentById(int vendorDetailId)
        {

            var res = await _vendorPaymentRepository.GetVendorPaymentById(vendorDetailId);
            VendorPaymentModel vendorPayment = new()
            {
                BankBranchName = res?.BankBranchName,
                CreatedBy = res?.CreatedBy,
                CreatedDate = res?.CreatedDate,
                FkVendorDetailId = res.FkVendorDetailId,
                IsActive = res.IsActive,
                LastUpdateBy = res?.LastUpdateBy,
                LastUpdatedDate = res?.LastUpdatedDate,
                VendorPaymentAmount = res.VendorPaymentAmount,
                VendorPaymentCgst = res?.VendorPaymentCgst,
                VendorPaymentDate = res.VendorPaymentDate,
                VendorPaymentId = res!.VendorPaymentId,
                VendorPaymentIsGst = res?.VendorPaymentIsGst,
                VendorPaymentRtgsAmount = res?.VendorPaymentRtgsAmount,
                VendorPaymentRtgsDate = res?.VendorPaymentRtgsDate,
                VendorPaymentSgst = res?.VendorPaymentSgst,
                VendorPaymentTdsamount = res?.VendorPaymentTdsamount,
                VendorPaymentTotalAmountPaid = res?.VendorPaymentTotalAmountPaid,
                VendorPaymentUtrnumber = res?.VendorPaymentUtrnumber,
                PaymentYear = res.PaymentYear
            };

            return vendorPayment;
        }
        public async Task RemoveVendorPayment(VendorPaymentModel VendorPaymentModel)
        {
            VendorPayment vendorPayment = new()
            {
                PaymentYear = VendorPaymentModel.PaymentYear,
                VendorPaymentTotalAmountPaid = VendorPaymentModel.VendorPaymentTotalAmountPaid,
                VendorPaymentUtrnumber = VendorPaymentModel.VendorPaymentUtrnumber,
                VendorPaymentTdsamount = VendorPaymentModel.VendorPaymentTdsamount,
                BankBranchName = VendorPaymentModel.BankBranchName,
                CreatedBy = VendorPaymentModel.CreatedBy,
                CreatedDate = VendorPaymentModel.CreatedDate,
                FkVendorDetailId = (int)VendorPaymentModel.FkVendorDetailId,
                IsActive = (bool)VendorPaymentModel.IsActive,
                LastUpdateBy = VendorPaymentModel.LastUpdateBy,
                LastUpdatedDate = VendorPaymentModel.LastUpdatedDate,
                VendorPaymentAmount = VendorPaymentModel.VendorPaymentAmount,
                VendorPaymentCgst = VendorPaymentModel.VendorPaymentCgst,
                VendorPaymentDate = VendorPaymentModel.VendorPaymentDate,
                VendorPaymentId = VendorPaymentModel.VendorPaymentId,
                VendorPaymentIsGst = VendorPaymentModel.VendorPaymentIsGst,

                VendorPaymentRtgsAmount = VendorPaymentModel.VendorPaymentRtgsAmount,
                VendorPaymentRtgsDate = VendorPaymentModel.VendorPaymentRtgsDate,
                VendorPaymentSgst = VendorPaymentModel.VendorPaymentSgst
            };
            await _vendorPaymentRepository.RemoveVendorPayment(vendorPayment);
        }
    }
}
