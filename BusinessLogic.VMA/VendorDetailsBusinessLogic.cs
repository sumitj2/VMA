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
                LastUpdateBy = vendorDetailModel.LastUpdateBy,
                LastUpdatedDate = DateTime.UtcNow,
                QuantityOfUnit = vendorDetailModel.QuantityOfUnit,
                FkVendorServiceId = vendorDetailModel.FkVendorServiceId,
                RatePerUnit = vendorDetailModel.RatePerUnit,
                ServiceEndDate = vendorDetailModel.ServiceEndDate,
                ServicePaymentType = vendorDetailModel.ServicePaymentType,
                ServiceSantionAmount = vendorDetailModel.ServiceSantionAmount,
                ServiceSantionedBy = vendorDetailModel.ServiceSantionedBy,
                ServiceStartDate = vendorDetailModel.ServiceStartDate,
                ServiceType = vendorDetailModel.ServiceType,
                VendorDetailCategory = vendorDetailModel.VendorDetailCategory,
                VendorDetailId = vendorDetailModel.VendorDetailId
            };
            await _vendorDetailsRepository.AddVendorDetails(vendorDetailEntity);
        }
        public async Task EditUpdateVendorDetails(VendorDetailModel vendorDetailModel)
        {
            VendorDetail vendorDetailEntity = new()
            {
                CreatedBy = vendorDetailModel.CreatedBy,
                CreatedDate = vendorDetailModel.CreatedDate,
                IsActive = vendorDetailModel.IsActive,
                LastUpdateBy = vendorDetailModel.LastUpdateBy,
                LastUpdatedDate = vendorDetailModel.LastUpdatedDate,
                VendorDetailId = vendorDetailModel.VendorDetailId,
                VendorDetailCategory = vendorDetailModel.VendorDetailCategory,
                ServiceType = vendorDetailModel.ServiceType,
                ServiceStartDate = vendorDetailModel.ServiceStartDate,
                ServiceSantionedBy = vendorDetailModel.ServiceSantionedBy,
                ServiceSantionAmount = vendorDetailModel.ServiceSantionAmount,
                ServicePaymentType = vendorDetailModel.ServiceType,
                ServiceEndDate = vendorDetailModel.ServiceEndDate,
                RatePerUnit = vendorDetailModel.RatePerUnit,
                FkVendorServiceId = vendorDetailModel.FkVendorServiceId,
                QuantityOfUnit = vendorDetailModel.QuantityOfUnit

            };
            await _vendorDetailsRepository.EditUpdateVendorDetail(vendorDetailEntity);
        }
        public async Task<IEnumerable<VendorDetailModel>> GetAllVendorDetails()
        {
            var repositoryResult = await _vendorDetailsRepository.GetVendorDetailsWithService();
            IList<VendorDetailModel> result = new List<VendorDetailModel>();
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
                    CreatedDate = data.CreatedDate,
                    FkVendorServiceId = data.FkVendorServiceId,
                    IsActive = data.IsActive,
                    LastUpdateBy = data.LastUpdateBy,
                    LastUpdatedDate = data.LastUpdatedDate,
                    ServiceSantionedBy = data.ServiceSantionedBy,
                    ServiceStartDate = data.ServiceStartDate,
                    VendorDetailCategory = data.VendorDetailCategory,
                    VendorServiceId = data.VendorServiceId,
                    VendorServiceName = data.VendorServiceName
                });
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
                VendorDetailId = res!.VendorDetailId
            };

            return vendorPayment;
        }
        public async Task RemoveVendorDetails(VendorDetailModel VendorPaymentModel)
        {
            VendorDetail vendorDetailEntity = new()
            {
                VendorDetailId= VendorPaymentModel.VendorDetailId,
                ServiceType= VendorPaymentModel.ServiceType,
                ServiceSantionAmount=VendorPaymentModel.ServiceSantionAmount,
                ServicePaymentType= VendorPaymentModel.ServicePaymentType,
                RatePerUnit=VendorPaymentModel.RatePerUnit,
                QuantityOfUnit=VendorPaymentModel.QuantityOfUnit,
                CreatedBy=VendorPaymentModel.CreatedBy,
                CreatedDate=VendorPaymentModel.CreatedDate,
                FkVendorServiceId=VendorPaymentModel.FkVendorServiceId,
                IsActive=VendorPaymentModel.IsActive,   
                LastUpdateBy=VendorPaymentModel.LastUpdateBy,
                LastUpdatedDate=VendorPaymentModel.LastUpdatedDate,
                ServiceEndDate=VendorPaymentModel.ServiceEndDate,
                ServiceSantionedBy= VendorPaymentModel.ServiceSantionedBy,
                ServiceStartDate=VendorPaymentModel.ServiceStartDate,   
                VendorDetailCategory= VendorPaymentModel.VendorDetailCategory   
            };
            await _vendorDetailsRepository.RemoveVendorDetails(vendorDetailEntity);
        }
    }
}
