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
        public async Task<int?> AddInvoice(InvoiceDetailsModel invoiceDetailModel)
        {
            InvoiceDetail invoiceDetailsEntity = new InvoiceDetail()
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
            return await _invoiceDetailsRepository.AddInvoice(invoiceDetailsEntity).ConfigureAwait(true);
        }
        public async Task EditUpdateInvoice(InvoiceDetailsModel invoiceDetailModel)
        {
            InvoiceDetail invoiceDetailsEntity = new()
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

            await _invoiceDetailsRepository.EditUpdateInvoice(invoiceDetailsEntity).ConfigureAwait(true);
        }
        public async Task<IEnumerable<InvoiceDetailsModel>> GetAllInvoices()
        {
            var res = await _invoiceDetailsRepository.GetAllInvoices().ConfigureAwait(true);
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
        public async Task<InvoiceDetailsModel?> GetInvoiceById(int? invoiceId)
        {
            var invoice = await _invoiceDetailsRepository.GetInvoiceById(invoiceId).ConfigureAwait(true);
            if (invoice != null)
            {
                InvoiceDetailsModel invoiceDetailsModel = new InvoiceDetailsModel()
                {
                    IsActive = invoice?.IsActive,
                    InvoiceParticulars = invoice?.InvoiceParticulars,
                    InvoiceNumber = invoice?.InvoiceNumber,
                    InvoiceId = invoice!.InvoiceId,
                    InvoiceDate = invoice!.InvoiceDate,
                    CreatedBy = invoice?.CreatedBy,
                    CreatedDate = invoice!.CreatedDate,
                    LastUpdateBy = invoice?.LastUpdateBy,
                    LastUpdatedDate = invoice?.LastUpdatedDate
                };
                return invoiceDetailsModel;
            }
            return null;
        }

        public async Task RemoveInvoice(InvoiceDetailsModel invoiceDetailModel)
        {
            InvoiceDetail invoiceDetailsEntity = new InvoiceDetail()
            {
                LastUpdatedDate = invoiceDetailModel.LastUpdatedDate,
                LastUpdateBy = invoiceDetailModel.LastUpdateBy,
                CreatedDate = invoiceDetailModel.CreatedDate,
                CreatedBy = invoiceDetailModel.CreatedBy,
                InvoiceDate = invoiceDetailModel.InvoiceDate,
                InvoiceId = invoiceDetailModel.InvoiceId,
                InvoiceNumber = invoiceDetailModel.InvoiceNumber,
                InvoiceParticulars = invoiceDetailModel.InvoiceParticulars,
                IsActive = invoiceDetailModel.IsActive
            };
            await _invoiceDetailsRepository.RemoveInvoice(invoiceDetailsEntity).ConfigureAwait(true);
        }
    }
}
