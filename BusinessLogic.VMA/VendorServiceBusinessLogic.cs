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
    public class VendorServiceBusinessLogic : IVendorServiceBusinessLogic
    {
        private IVendorServiceRepository _vendorServiceRepository;
        public VendorServiceBusinessLogic(IVendorServiceRepository vendorServiceRepository)
        {
            _vendorServiceRepository=vendorServiceRepository;
        }
        public async Task AddVendorService(VendorServiceModel vendorServiceModel)
        {
            VendorService vendorService = new()
            {                
                FkVendorId = vendorServiceModel.FkVendorId,
                CreatedBy = vendorServiceModel.CreatedBy,
                CreatedDate = DateTime.Now,
                IsActive = vendorServiceModel.IsActive,
                LastUpdateBy = vendorServiceModel.LastUpdateBy,
                LastUpdatedDate = DateTime.Now,
                VendorServiceName=vendorServiceModel.VendorServiceName                
            };
            await _vendorServiceRepository.AddVendorService(vendorService);
        }
        public async Task EditUpdateVendorService(VendorServiceModel serviceModel)
        {
            VendorService vendorService = new() 
            {
                CreatedBy=serviceModel?.CreatedBy,
                CreatedDate=serviceModel?.CreatedDate,
                FkVendorId=serviceModel?.FkVendorId,
                IsActive=serviceModel?.IsActive,  
                LastUpdateBy=serviceModel?.LastUpdateBy,
                LastUpdatedDate=serviceModel?.LastUpdatedDate,    
                VendorServiceId=serviceModel!.VendorServiceId,   
                VendorServiceName = serviceModel?.VendorServiceName 
            };
           await _vendorServiceRepository.EditUpdateVendorService(vendorService);

        }
        public async Task<IEnumerable<VendorServiceModel>> GetAllVendorServices()
        {
            var repositoryResult= await _vendorServiceRepository.GetVendorWithService();
            List<VendorServiceModel> services = [];
            foreach(var service in repositoryResult)
            {
                services.Add(new VendorServiceModel() 
                {
                    CreatedBy=service.CreatedBy,
                    VendorServiceName=service.VendorServiceName,
                    CreatedDate=service.CreatedDate,
                    FkVendorId=service.FkVendorId,
                    IsActive=service.IsActive,
                    LastUpdateBy=service.LastUpdateBy,
                    LastUpdatedDate=service.LastUpdatedDate,
                    VendorServiceId = service.VendorServiceId  ,
                    VendorName = service.VendorName,
                    VendorId=service.VendorId,
                    VendorCode=service.VendorCode,
                });
            }
            return services;
        }
        public async Task<VendorServiceModel?> GetVendorServiceById(int vendorId)
        {
            
            var repositoryResult= await _vendorServiceRepository.GetVendorServiceById(vendorId);
            VendorServiceModel vendorServiceModel = new() 
            {
                CreatedBy=repositoryResult?.CreatedBy,
                CreatedDate=repositoryResult?.CreatedDate,
                FkVendorId=repositoryResult?.FkVendorId,
                IsActive=repositoryResult?.IsActive,  
                LastUpdateBy=repositoryResult?.LastUpdateBy,
                LastUpdatedDate=repositoryResult?.LastUpdatedDate,    
                VendorServiceId=repositoryResult!.VendorServiceId,   
                VendorServiceName = repositoryResult?.VendorServiceName 
            };
            return vendorServiceModel;

        }
        public async Task RemoveVendorService(VendorServiceModel serviceModel)
        {
            VendorService vendorService = new()
            {
                CreatedBy = serviceModel?.CreatedBy,
                CreatedDate = serviceModel?.CreatedDate,
                FkVendorId = serviceModel?.FkVendorId,
                IsActive = serviceModel?.IsActive,
                LastUpdateBy = serviceModel?.LastUpdateBy,
                LastUpdatedDate = serviceModel?.LastUpdatedDate,
                VendorServiceId = serviceModel!.VendorServiceId,
                VendorServiceName = serviceModel?.VendorServiceName
            };

            await _vendorServiceRepository.RemoveVendorService(vendorService);
        }
    }
}
