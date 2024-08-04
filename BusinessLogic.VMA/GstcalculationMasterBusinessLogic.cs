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
    public class GstcalculationMasterBusinessLogic : IGstcalculationMasterBusinessLogic
    {
        private readonly IGstcalculationMasterRepository _gstcalculationMasterRepository;
        public GstcalculationMasterBusinessLogic(IGstcalculationMasterRepository gstcalculationMasterRepository)
        {
            _gstcalculationMasterRepository = gstcalculationMasterRepository;
        }
        public async Task AddGstMaster(GstcalculationMasterModel GstcalculationMasterModel)
        {
            GstcalculationMaster gstcalculationMasterEntity = new GstcalculationMaster()
            {
                GstCategoryName =  GstcalculationMasterModel.GstCategoryName,
                SgstPercentage = GstcalculationMasterModel.SgstPercentage,
                CgstPercentage = GstcalculationMasterModel.CgstPercentage,
                IgstPercentage = GstcalculationMasterModel.IgstPercentage,
                CreatedBy = GstcalculationMasterModel.CreatedBy,
                CreatedDate = GstcalculationMasterModel.CreatedDate,
                IsActive = GstcalculationMasterModel.IsActive
            };
            await _gstcalculationMasterRepository.AddGstMaster(gstcalculationMasterEntity);

        }
        public async Task EditUpdateGst(GstcalculationMasterModel GstcalculationMasterModel)
        {
            GstcalculationMaster gstcalculationMasterEntity = new GstcalculationMaster()
            {
                CgstPercentage = GstcalculationMasterModel.CgstPercentage,
                CreatedBy = GstcalculationMasterModel.CreatedBy,
                CreatedDate = GstcalculationMasterModel.CreatedDate,
                IgstPercentage = GstcalculationMasterModel.IgstPercentage,
                IsActive = GstcalculationMasterModel.IsActive,
                LastUpdateBy = GstcalculationMasterModel.LastUpdateBy,
                LastUpdatedDate = GstcalculationMasterModel.LastUpdatedDate,
                SgstPercentage = GstcalculationMasterModel.SgstPercentage
            };
            await _gstcalculationMasterRepository.EditUpdateGst(gstcalculationMasterEntity);
        }
        public async Task<IEnumerable<GstcalculationMasterModel>> GetAllGstMaster()
        {
            var gstmaster = await _gstcalculationMasterRepository.GetAllGstMaster();
            List<GstcalculationMasterModel> GstcalculationMasterModel = new List<GstcalculationMasterModel>();
            foreach (var gstmasterEntity in gstmaster)
            {
                GstcalculationMasterModel.Add(new GstcalculationMasterModel()
                {
                    SrNo = gstmasterEntity.SrNo,
                    LastUpdatedDate = gstmasterEntity.LastUpdatedDate,
                    CgstPercentage = gstmasterEntity.CgstPercentage,
                    CreatedBy = gstmasterEntity.CreatedBy,
                    CreatedDate = gstmasterEntity.CreatedDate,
                    IgstPercentage = gstmasterEntity.IgstPercentage,
                    IsActive = gstmasterEntity.IsActive,
                    LastUpdateBy = gstmasterEntity.LastUpdateBy,
                    SgstPercentage = gstmasterEntity.SgstPercentage,
                    GstCategoryName=gstmasterEntity.GstCategoryName
                });
            }
            return GstcalculationMasterModel;
        }
        public async Task<GstcalculationMasterModel?> GetGstMasterById(int srNo)
        {
            var result = await _gstcalculationMasterRepository.GetGstMasterById(srNo);
            GstcalculationMasterModel gstcalculationMasterModel = new GstcalculationMasterModel()
            {
                SgstPercentage = result?.CgstPercentage,
                CgstPercentage = result?.CgstPercentage,
                CreatedBy = result?.CreatedBy,
                CreatedDate = result?.CreatedDate,
                IgstPercentage = result?.IgstPercentage,
                IsActive = result?.IsActive,
                LastUpdateBy = result?.LastUpdateBy,
                LastUpdatedDate = result?.LastUpdatedDate,
                SrNo = result!.SrNo,
            };
            return gstcalculationMasterModel;
        }

        public async Task RemoveGstMaster(GstcalculationMasterModel gstCalculationModel)
        {
            GstcalculationMaster gstcalculationMasterModel = new GstcalculationMaster()
            {
                SgstPercentage = gstCalculationModel?.CgstPercentage,
                CgstPercentage = gstCalculationModel?.CgstPercentage,
                CreatedBy = gstCalculationModel?.CreatedBy,
                CreatedDate = gstCalculationModel?.CreatedDate,
                IgstPercentage = gstCalculationModel?.IgstPercentage,
                IsActive = gstCalculationModel?.IsActive,
                LastUpdateBy = gstCalculationModel?.LastUpdateBy,
                LastUpdatedDate = gstCalculationModel?.LastUpdatedDate,
                SrNo = gstCalculationModel!.SrNo,
            };
            await _gstcalculationMasterRepository.RemoveGstMaster(gstcalculationMasterModel);
        }
    }
}
