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
    public class InvoiceDetailsBusinessLogic : IInvoiceDetailsBusinessLogic
    {
        private readonly IInvoiceDetailsRepository _invoiceDetailsRepository;
        public InvoiceDetailsBusinessLogic(IInvoiceDetailsRepository invoiceDetailsRepository)
        {
            _invoiceDetailsRepository = invoiceDetailsRepository;
        }
        public async Task<int> AddInvoice(InvoiceDetailsModel invoiceDetailModel)
        {
            InvoiceDetails invoiceDetailsEntity = new InvoiceDetails()
            {
                CreatedBy = invoiceDetailModel.CreatedBy,
                CreatedDate = invoiceDetailModel.CreatedDate,
                InvoiceDate = invoiceDetailModel.InvoiceDate,
                InvoiceId = invoiceDetailModel.InvoiceId,
                InvoiceNumber = invoiceDetailModel.InvoiceNumber,
                InvoiceParticulars = invoiceDetailModel.InvoiceParticulars,
                IsActive = invoiceDetailModel.IsActive,
                LastUpdateBy = invoiceDetailModel.LastUpdateBy,
                LastUpdatedDate = invoiceDetailModel.LastUpdatedDate
            };
            return await _invoiceDetailsRepository.AddInvoice(invoiceDetailsEntity);
        }
        public async Task EditUpdateInvoice(InvoiceDetailsModel invoiceDetailModel)
        {
            InvoiceDetails invoiceDetailsEntity = new()
            {
                CreatedBy = invoiceDetailModel.CreatedBy,
                CreatedDate = invoiceDetailModel.CreatedDate,
                InvoiceDate = invoiceDetailModel.InvoiceDate,
                InvoiceId = invoiceDetailModel.InvoiceId,
                InvoiceNumber = invoiceDetailModel.InvoiceNumber,
                InvoiceParticulars = invoiceDetailModel.InvoiceParticulars,
                IsActive = invoiceDetailModel.IsActive,
                LastUpdateBy = invoiceDetailModel.LastUpdateBy,
                LastUpdatedDate = invoiceDetailModel.LastUpdatedDate
            };
            try
            {
                await _invoiceDetailsRepository.EditUpdateInvoice(invoiceDetailsEntity);
            }
            catch (Exception ex)
            {

                throw;
            }
            await _invoiceDetailsRepository.EditUpdateInvoice(invoiceDetailsEntity);
        }
        public async Task<IEnumerable<InvoiceDetailsModel>> GetAllInvoices()
        {
            var res = await _invoiceDetailsRepository.GetAllInvoices();
            List<InvoiceDetailsModel> invoiceDetailsModels = [];
            foreach (var invoice in res)
            {
                invoiceDetailsModels.Add(new InvoiceDetailsModel()
                {
                    LastUpdateBy = invoice.LastUpdateBy,
                    LastUpdatedDate = invoice.LastUpdatedDate,
                    CreatedBy = invoice.CreatedBy,
                    CreatedDate = invoice.CreatedDate,
                    InvoiceDate = invoice.InvoiceDate,
                    InvoiceId = invoice.InvoiceId,
                    InvoiceNumber = invoice.InvoiceNumber,
                    InvoiceParticulars = invoice.InvoiceParticulars,
                    IsActive = invoice.IsActive,

                });
            }
            return invoiceDetailsModels;
        }
        public async Task<InvoiceDetailsModel?> GetInvoiceById(int invoiceId)
        {
            var res = await _invoiceDetailsRepository.GetInvoiceById(invoiceId);
            InvoiceDetailsModel invoiceDetailsModel = new InvoiceDetailsModel() 
            {
                IsActive = res?.IsActive,
                InvoiceParticulars  =res?.InvoiceParticulars,
                InvoiceNumber=res?.InvoiceNumber,
                InvoiceId=res!.InvoiceId,
                InvoiceDate=res!.InvoiceDate,
                CreatedBy=res?.CreatedBy,
                CreatedDate=res!.CreatedDate,
                LastUpdateBy=res?.LastUpdateBy,
                LastUpdatedDate=res?.LastUpdatedDate
            };
            return invoiceDetailsModel; 
        }

        public async Task RemoveInvoice(InvoiceDetailsModel invoiceDetailModel)
        {
            InvoiceDetails invoiceDetailsEntity=new InvoiceDetails()
            {
                LastUpdatedDate = invoiceDetailModel.LastUpdatedDate,
                LastUpdateBy = invoiceDetailModel.LastUpdateBy,
                CreatedDate = invoiceDetailModel.CreatedDate,
                CreatedBy = invoiceDetailModel.CreatedBy,
                InvoiceDate = invoiceDetailModel.InvoiceDate,   
                InvoiceId = invoiceDetailModel.InvoiceId,   
                InvoiceNumber = invoiceDetailModel.InvoiceNumber,
                InvoiceParticulars=invoiceDetailModel.InvoiceParticulars,
                IsActive=   invoiceDetailModel.IsActive
            };
            await _invoiceDetailsRepository.RemoveInvoice(invoiceDetailsEntity);
        }
    }
}
