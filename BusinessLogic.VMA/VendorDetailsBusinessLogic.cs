using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.Abstraction.VMA.Models;
using Database.Abstraction.VMA.Contract;
using Database.VMA.Entities;

namespace Database.VMA.Repositories
{
    public class VendorDetailsBusinessLogic : IVendorDetailsBusinessLogic
    {

        private IVendorDetailsRepository _vendorDetailsRepository;
        public VendorDetailsBusinessLogic(IVendorDetailsRepository vendorDetailsRepository)
        {
            _vendorDetailsRepository = vendorDetailsRepository;
        }
        public async Task AddVendorDetails(VendorDetailModel vendorDetailModel)
        {
            VendorDetail vendorDetailEntity = new()
            {
                CreatedBy = vendorDetailModel.CreatedBy,
                CreatedDate = DateTime.UtcNow,
                IsActive = vendorDetailModel.IsActive,
                QuantityOfUnit = vendorDetailModel.QuantityOfUnit,
                FkVendorServiceId = vendorDetailModel?.FkVendorServiceId,
                RatePerUnit = vendorDetailModel?.RatePerUnit,
                ServiceEndDate = vendorDetailModel?.ServiceEndDate,
                ServicePaymentType = vendorDetailModel?.ServicePaymentType,
                ServiceSantionAmount = vendorDetailModel?.ServiceSantionAmount,
                ServiceSantionedBy = vendorDetailModel?.ServiceSantionedBy,
                ServiceStartDate = vendorDetailModel?.ServiceStartDate,
                ServiceType = vendorDetailModel?.ServiceType,
                VendorDetailCategory = vendorDetailModel?.VendorDetailCategory,
                VendorDetailId = vendorDetailModel?.VendorDetailId,
                DetailsYear = vendorDetailModel?.DetailsYear,
                FkVendorId = vendorDetailModel?.FkVendorId,
                IsAmc = vendorDetailModel?.IsAmc,
                SantionedDate = vendorDetailModel?.SantionedDate,
                SantionedNoteNo = vendorDetailModel?.SantionedNoteNo,
                SlaexpireDate = vendorDetailModel?.SlaexpireDate,   
            };
            await _vendorDetailsRepository.AddVendorDetails(vendorDetailEntity);
        }
        public async Task EditUpdateVendorDetails(VendorDetailModel vendorDetailModel)
        {
            var detail = await _vendorDetailsRepository.GetVendorDetailsId(Convert.ToInt32(vendorDetailModel.VendorDetailId));
            if (detail != null)
            {
                detail.IsActive = true;
                detail.LastUpdateBy = vendorDetailModel.LastUpdateBy;
                detail.LastUpdatedDate = DateTime.UtcNow;
                detail.VendorDetailId = vendorDetailModel.VendorDetailId;
                detail.VendorDetailCategory = vendorDetailModel.VendorDetailCategory;
                detail.ServiceType = vendorDetailModel.ServiceType;
                detail.ServiceStartDate = vendorDetailModel.ServiceStartDate;
                detail.ServiceSantionedBy = vendorDetailModel.ServiceSantionedBy;
                detail.ServiceSantionAmount = vendorDetailModel.ServiceSantionAmount;
                detail.ServicePaymentType = vendorDetailModel.ServicePaymentType;
                detail.ServiceEndDate = vendorDetailModel.ServiceEndDate;
                detail.RatePerUnit = vendorDetailModel.RatePerUnit;
                detail.FkVendorServiceId = vendorDetailModel.FkVendorServiceId;
                detail.QuantityOfUnit = vendorDetailModel.QuantityOfUnit;
                detail.SantionedDate = vendorDetailModel.SantionedDate;
                detail.FkVendorId = vendorDetailModel.FkVendorId;
                detail.VendorDetailCategory = vendorDetailModel?.VendorDetailCategory;
                detail.FkVendorId = vendorDetailModel?.VendorId;
                detail.IsAmc = vendorDetailModel?.IsAmc;
                detail.SantionedNoteNo = vendorDetailModel?.SantionedNoteNo;
                detail.SlaexpireDate = vendorDetailModel?.SlaexpireDate;
                await _vendorDetailsRepository.EditUpdateVendorDetail(detail);
            }
        }
        public async Task<IEnumerable<VendorDetailModel>> GetAllVendorDetails()
        {
            var repositoryResult = await _vendorDetailsRepository.GetVendorDetailsWithService();
            IList<VendorDetailModel> result = [];
            if (repositoryResult != null)
            {
                foreach (var data in repositoryResult)
                {
                    result.Add(new VendorDetailModel()
                    {
                        RatePerUnit = data.RatePerUnit,
                        ServiceType = data.ServiceType,
                        ServiceSantionAmount = data.ServiceSantionAmount,
                        ServicePaymentType = data.ServicePaymentType,
                        ServiceEndDate = data.ServiceEndDate,
                        VendorDetailId = data.VendorDetailId,
                        QuantityOfUnit = data.QuantityOfUnit,
                        CreatedBy = data.CreatedBy,
                        CreatedDate = DateTime.UtcNow,
                        FkVendorServiceId = data.FkVendorServiceId,
                        IsActive = true,
                        ServiceSantionedBy = data.ServiceSantionedBy,
                        ServiceStartDate = data.ServiceStartDate,
                        VendorDetailCategory = data.VendorDetailCategory,
                        VendorName = data.VendorName,
                        VendorId = data.VendorId,
                        VendorCode = data.VendorCode,
                        SantionedType = data.SantionedType,
                        SantionedNoteNo = data.SantionedNoteNo,
                        SantionedDate = data.SantionedDate,
                        IsAmc = data.IsAmc,
                        DetailsYear = data.DetailsYear,
                        FkVendorId = data.FkVendorId,
                        LastUpdateBy = data.LastUpdateBy,
                        LastUpdatedDate = data.LastUpdatedDate,
                        VendorServiceId = data.VendorServiceId,
                        VendorServiceName = data.VendorServiceName,
                        SlaexpireDate=data.SlaexpireDate,
                    });
                }
            }
            return result;
        }
        public async Task<VendorDetailModel?> GetVendorDetailsById(int vendorDetailId)
        {

            var res = await _vendorDetailsRepository.GetVendorDetailsId(vendorDetailId);
            VendorDetailModel vendorPayment = new()
            {
                VendorDetailCategory = res?.VendorDetailCategory,
                ServiceStartDate = res?.ServiceStartDate,
                ServiceEndDate = res?.ServiceEndDate,
                ServiceSantionedBy = res?.ServiceSantionedBy,
                LastUpdatedDate = res?.LastUpdatedDate,
                LastUpdateBy = res?.LastUpdateBy,
                IsActive = res?.IsActive,
                FkVendorServiceId = res?.FkVendorServiceId,
                CreatedDate = res?.CreatedDate,
                CreatedBy = res?.CreatedBy,
                QuantityOfUnit = res?.QuantityOfUnit,
                RatePerUnit = res?.RatePerUnit,
                ServicePaymentType = res?.ServicePaymentType,
                ServiceSantionAmount = res?.ServiceSantionAmount,
                ServiceType = res?.ServiceType,
                VendorDetailId = res!.VendorDetailId,
                SlaexpireDate = res?.SlaexpireDate,
            };

            return vendorPayment;
        }
        public async Task RemoveVendorDetails(VendorDetailModel VendorPaymentModel)
        {
            VendorDetail vendorDetailEntity = new()
            {
                VendorDetailId = VendorPaymentModel.VendorDetailId,
                ServiceType = VendorPaymentModel.ServiceType,
                ServiceSantionAmount = VendorPaymentModel.ServiceSantionAmount,
                ServicePaymentType = VendorPaymentModel.ServicePaymentType,
                RatePerUnit = VendorPaymentModel.RatePerUnit,
                QuantityOfUnit = VendorPaymentModel.QuantityOfUnit,
                CreatedBy = VendorPaymentModel.CreatedBy,
                CreatedDate = VendorPaymentModel.CreatedDate,
                FkVendorServiceId = VendorPaymentModel.FkVendorServiceId,
                IsActive = VendorPaymentModel.IsActive,
                LastUpdateBy = VendorPaymentModel.LastUpdateBy,
                LastUpdatedDate = VendorPaymentModel.LastUpdatedDate,
                ServiceEndDate = VendorPaymentModel.ServiceEndDate,
                ServiceSantionedBy = VendorPaymentModel.ServiceSantionedBy,
                ServiceStartDate = VendorPaymentModel.ServiceStartDate,
                VendorDetailCategory = VendorPaymentModel.VendorDetailCategory,
                SlaexpireDate= VendorPaymentModel.SlaexpireDate,
            };
            await _vendorDetailsRepository.RemoveVendorDetails(vendorDetailEntity);
        }
    }
}
