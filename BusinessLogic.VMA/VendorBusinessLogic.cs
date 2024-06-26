using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.Abstraction.VMA.Models;
using Database.Abstraction.VMA.Contract;
using Database.VMA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.VMA.Repositories
{
    public class VendorBusinessLogic : IVendorBusinessLogic
    {

        private readonly IVendorRepository _vendorRepository;
        public VendorBusinessLogic(IVendorRepository vendorRepository)
        {
            _vendorRepository = vendorRepository;
        }
        public async Task AddVendor(VendorModel vendorModel)
        {
            Vendor vendorEntity = new()
            {

                CreatedBy = vendorModel.CreatedBy,
                CreatedDate = DateTime.UtcNow,
                IsActive = true,
                LastUpdateBy = vendorModel.LastUpdateBy,
                LastUpdatedDate = DateTime.UtcNow,
                VendorAccountNumber = vendorModel.VendorAccountNumber,
                VendorName = vendorModel.VendorName,
                VendorAddress = vendorModel.VendorAddress,
                VendorCode = vendorModel.VendorCode,
                VendorBankName = vendorModel.VendorBankName,
                VendorEmailId = vendorModel.VendorEmailId,
                VendorGstnumber = vendorModel.VendorGstnumber,
                VendorId = vendorModel.VendorId,
                VendorIfsccode = vendorModel.VendorIfsccode,
                VendorPhoneNo = vendorModel.VendorPhoneNo,
                VendorPinCode = vendorModel.VendorPinCode
            };
            await _vendorRepository.AddVendors(vendorEntity);
        }
        public async Task EditUpdateVendor(VendorModel vendorModel)
        {
            Vendor vendorEntity = new()
            {
                CreatedBy = vendorModel?.CreatedBy,
                CreatedDate = vendorModel?.CreatedDate,
                IsActive = vendorModel?.IsActive,
                LastUpdateBy = vendorModel?.LastUpdateBy,
                LastUpdatedDate = vendorModel?.LastUpdatedDate,
                VendorPinCode = vendorModel?.VendorPinCode,
                VendorPhoneNo = vendorModel?.VendorPhoneNo,
                VendorIfsccode = vendorModel?.VendorIfsccode,
                VendorId = vendorModel!.VendorId,
                VendorGstnumber = vendorModel?.VendorGstnumber,
                VendorAccountNumber = vendorModel?.VendorAccountNumber,
                VendorAddress = vendorModel?.VendorAddress,
                VendorBankName = vendorModel?.VendorBankName,
                VendorCode = vendorModel?.VendorCode,
                VendorEmailId = vendorModel?.VendorEmailId,
                VendorName = vendorModel?.VendorName
            };
            await _vendorRepository.EditUpdateVendors(vendorEntity);

        }
        public async Task<IEnumerable<VendorModel>> GetAllVendor()
        {
            var repositoryResult = await _vendorRepository.GetAllVendors();
            List<VendorModel> services = [];
            foreach (var vendor in repositoryResult)
            {
                services.Add(new VendorModel()
                {
                    CreatedBy = vendor.CreatedBy,
                    CreatedDate = vendor.CreatedDate,
                    IsActive = vendor.IsActive,
                    LastUpdateBy = vendor.LastUpdateBy,
                    LastUpdatedDate = vendor.LastUpdatedDate,
                    VendorName = vendor.VendorName,
                    VendorEmailId = vendor.VendorEmailId,
                    VendorCode = vendor.VendorCode,
                    VendorBankName = vendor.VendorBankName,
                    VendorAddress = vendor.VendorAddress,
                    VendorAccountNumber = vendor.VendorAccountNumber,
                    VendorGstnumber = vendor.VendorGstnumber,
                    VendorId = vendor.VendorId,
                    VendorIfsccode = vendor.VendorIfsccode,
                    VendorPhoneNo = vendor.VendorPhoneNo,
                    VendorPinCode = vendor.VendorPinCode
                });
            }
            return services;
        }
        public async Task<VendorModel?> GetVendorById(int vendorId)
        {

            var repositoryResult = await _vendorRepository.GetVendorsById(vendorId);
            VendorModel vendorModel = new()
            {
                CreatedBy = repositoryResult?.CreatedBy,
                CreatedDate = repositoryResult?.CreatedDate,
                IsActive = repositoryResult?.IsActive,
                LastUpdateBy = repositoryResult?.LastUpdateBy,
                LastUpdatedDate = repositoryResult?.LastUpdatedDate,
                VendorName = repositoryResult?.VendorName,
                VendorPinCode = repositoryResult?.VendorPinCode,
                VendorPhoneNo = repositoryResult?.VendorPhoneNo,
                VendorIfsccode = repositoryResult?.VendorIfsccode,
                VendorId = repositoryResult!.VendorId,
                VendorGstnumber = repositoryResult?.VendorGstnumber,
                VendorAccountNumber = repositoryResult?.VendorAccountNumber,
                VendorAddress = repositoryResult?.VendorAddress,
                VendorBankName = repositoryResult?.VendorBankName,
                VendorCode = repositoryResult?.VendorCode,
                VendorEmailId = repositoryResult?.VendorEmailId

            };
            return vendorModel;

        }
        public async Task RemoveVendorService(VendorModel serviceModel)
        {
            Vendor vendorService = new()
            {
                CreatedBy = serviceModel?.CreatedBy,
                CreatedDate = serviceModel?.CreatedDate,
                IsActive = serviceModel?.IsActive,
                LastUpdateBy = serviceModel?.LastUpdateBy,
                LastUpdatedDate = serviceModel?.LastUpdatedDate,
                VendorEmailId = serviceModel?.VendorEmailId,
                VendorCode = serviceModel?.VendorCode,
                VendorBankName = serviceModel?.VendorBankName,
                VendorAddress = serviceModel?.VendorAddress,
                VendorAccountNumber = serviceModel?.VendorAccountNumber,
                VendorGstnumber = serviceModel?.VendorGstnumber,
                VendorId = serviceModel!.VendorId,
                VendorIfsccode = serviceModel?.VendorIfsccode,
                VendorName = serviceModel?.VendorName,
                VendorPhoneNo = serviceModel?.VendorPhoneNo,
                VendorPinCode = serviceModel?.VendorPinCode
            };

            await _vendorRepository.RemoveVendor(vendorService);
        }
    }
}
