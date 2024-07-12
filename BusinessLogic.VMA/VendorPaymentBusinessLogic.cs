using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.Abstraction.VMA.Models;
using Database.Abstraction.VMA.Contract;
using Database.VMA.Entities;

namespace Database.VMA.Repositories
{
    public class VendorPaymentBusinessLogic : IVendorPaymentBusinessLogic
    {
        private IVendorPaymentRepository _vendorPaymentRepository;
        public VendorPaymentBusinessLogic(IVendorPaymentRepository vendorPaymentRepository)
        {
            _vendorPaymentRepository = vendorPaymentRepository;
        }

        public async Task<string> GeneratePaymentCode(VendorDetailModel? vendorDetailModel)
        {
            string paymentcode = "";
            int counter = 1;
            var res = await GetAllVendorPayment().ConfigureAwait(false);
            var checkPaymentForService = res.Where(x => x.VendorServiceId == vendorDetailModel?.FkVendorServiceId);
            if (!checkPaymentForService.Any())
            {
                paymentcode = string.Join("_", vendorDetailModel?.VendorServiceName?.Replace(" ", ""), vendorDetailModel?.ServicePaymentType, counter);

            }
            else
            {
                //Get all payment code 
                var paymentCode = checkPaymentForService.OrderBy(x => x.CreatedDate)?.FirstOrDefault()?.PaymentCode;

                //Split payment code by "_"
                var result = paymentCode?.Split("_");

                //Get last number of splited value to increase counter
                //0-ServiceName
                //1-PaymentType
                //2-Counter

                if (result != null)
                {
                    paymentcode = string.Join("_", result[0], result[1], Convert.ToInt32(result[2]) + 1);
                }
                //Need to write logic on payment method
            }

            return paymentcode;
        }



        public async Task AddVendorPayment(VendorPaymentModel VendorPaymentModel)
        {
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
                VendorPaymentDate = (DateTime)VendorPaymentModel.VendorPaymentDate,
                VendorPaymentId = VendorPaymentModel.VendorPaymentId,
                VendorPaymentIsGst = VendorPaymentModel.VendorPaymentIsGst,
                Notes = VendorPaymentModel.VendorPaymentNotesDetails,
                VendorPaymentRtgsAmount = VendorPaymentModel.VendorPaymentRtgsAmount,
                VendorPaymentRtgsDate = VendorPaymentModel.VendorPaymentRtgsDate,
                VendorPaymentSgst = VendorPaymentModel.VendorPaymentSgst,
                VendorPaymentTdsamount = VendorPaymentModel.VendorPaymentTdsamount,
                VendorPaymentTotalAmountPaid = VendorPaymentModel.VendorPaymentTotalAmountPaid,
                VendorPaymentUtrnumber = VendorPaymentModel.VendorPaymentUtrnumber,
                PaymentYear = VendorPaymentModel.VendorPaymentYear,
                PaymentCode = VendorPaymentModel.PaymentCode
            };
            await _vendorPaymentRepository.AddVendorPayment(vendorPayment);
        }
        public async Task EditUpdateVendorPayment(VendorPaymentModel VendorPaymentEntity)
        {
            var entity = await _vendorPaymentRepository.GetVendorPaymentById(VendorPaymentEntity.VendorPaymentId);
            if (entity != null)
            {
                entity.PaymentYear = VendorPaymentEntity.VendorPaymentYear;
                entity.VendorPaymentTotalAmountPaid = VendorPaymentEntity.VendorPaymentTotalAmountPaid;
                entity.VendorPaymentUtrnumber = VendorPaymentEntity.VendorPaymentUtrnumber;
                entity.VendorPaymentTdsamount = VendorPaymentEntity.VendorPaymentTdsamount;
                entity.BankBranchName = VendorPaymentEntity.BankBranchName;
                entity.CreatedBy = VendorPaymentEntity.CreatedBy;
                entity.CreatedDate = VendorPaymentEntity.CreatedDate;
                entity.FkVendorDetailId = (int)VendorPaymentEntity.FkVendorDetailId;
                entity.IsActive = (bool)VendorPaymentEntity.IsActive;
                entity.LastUpdateBy = VendorPaymentEntity.LastUpdateBy;
                entity.LastUpdatedDate = VendorPaymentEntity.LastUpdatedDate;
                entity.VendorPaymentAmount = VendorPaymentEntity.VendorPaymentAmount;
                entity.VendorPaymentCgst = VendorPaymentEntity.VendorPaymentCgst;
                entity.VendorPaymentDate = (DateTime)VendorPaymentEntity.VendorPaymentDate;
                entity.VendorPaymentId = VendorPaymentEntity.VendorPaymentId;
                entity.VendorPaymentIsGst = VendorPaymentEntity.VendorPaymentIsGst;
                entity.Notes = VendorPaymentEntity.VendorPaymentNotesDetails;
                entity.VendorPaymentRtgsAmount = VendorPaymentEntity.VendorPaymentRtgsAmount;
                entity.VendorPaymentRtgsDate = VendorPaymentEntity.VendorPaymentRtgsDate;
                entity.VendorPaymentSgst = VendorPaymentEntity.VendorPaymentSgst;
                entity.PaymentCode = VendorPaymentEntity.PaymentCode;
                entity.IsPaymentForBranch = VendorPaymentEntity.IsPaymentForBranch;
                entity.VendorPaymentIsTdsapplicable = VendorPaymentEntity.VendorPaymentIsTdsapplicable;

            }

            await _vendorPaymentRepository.EditUpdateVendorPayment(entity);
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
                    VendorPaymentNotesDetails = data.Notes,
                    VendorPaymentRtgsAmount = data.VendorPaymentRtgsAmount,
                    VendorPaymentRtgsDate = data.VendorPaymentRtgsDate,
                    VendorPaymentSgst = data.VendorPaymentSgst,
                    VendorPaymentTdsamount = data.VendorPaymentTdsamount,
                    VendorPaymentTotalAmountPaid = data.VendorPaymentTotalAmountPaid,
                    VendorPaymentUtrnumber = data.VendorPaymentUtrnumber,
                    VendorPaymentYear = data.PaymentYear,
                    VendorServiceName = data.VendorServiceName,
                    VendorServiceId = data.VendorServiceId,
                    ServiceSantionAmount = data.ServiceSantionAmount,
                    ServicePaymentType = data.ServicePaymentType,
                    PaymentCode = data.PaymentCode,
                    IsPaymentForBranch = data.IsPaymentForBranch,
                    VendorPaymentIsTdsapplicable = data.VendorPaymentIsTdsapplicable
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
                FkVendorDetailId = res?.FkVendorDetailId,
                IsActive = res?.IsActive,
                LastUpdateBy = res?.LastUpdateBy,
                LastUpdatedDate = res?.LastUpdatedDate,
                VendorPaymentAmount = res?.VendorPaymentAmount,
                VendorPaymentCgst = res?.VendorPaymentCgst,
                VendorPaymentDate = res?.VendorPaymentDate,
                VendorPaymentId = res!.VendorPaymentId,
                VendorPaymentIsGst = res?.VendorPaymentIsGst,                
                VendorPaymentRtgsAmount = res?.VendorPaymentRtgsAmount,
                VendorPaymentRtgsDate = res?.VendorPaymentRtgsDate,
                VendorPaymentSgst = res?.VendorPaymentSgst,
                VendorPaymentTdsamount = res?.VendorPaymentTdsamount,
                VendorPaymentTotalAmountPaid = res?.VendorPaymentTotalAmountPaid,
                VendorPaymentUtrnumber = res?.VendorPaymentUtrnumber,
                VendorPaymentYear = res?.PaymentYear
            };

            return vendorPayment;
        }
        public async Task RemoveVendorPayment(VendorPaymentModel VendorPaymentModel)
        {
            VendorPayment vendorPayment = new()
            {
                PaymentYear = VendorPaymentModel.VendorPaymentYear,
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
                VendorPaymentDate = (DateTime)VendorPaymentModel.VendorPaymentDate,
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
