using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.Abstraction.VMA.Models;
using Database.Abstraction.VMA.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.VMA
{
    public class YearlyMonthlyReportPDF : IYearlyMonthlyReportPDF
    {
        private readonly IVenderPaymentNotesRepository _venderPaymentNotesRepository;
        public YearlyMonthlyReportPDF(IVenderPaymentNotesRepository venderPaymentNotesRepository)
        {
            _venderPaymentNotesRepository = venderPaymentNotesRepository;
        }

        public Task GenerateMonthlyReport(string? financilaYear, string? Month, string? path)
        {
            return null;
        }

        public async Task GenerateYearlyReport(string? financilaYear, string? path)
        {
            List<PDFYearlyData> pdfData = [];
            var payments = await _venderPaymentNotesRepository.GetAllPaymentDetailsWithServiceDetailsToExport(financilaYear).ConfigureAwait(true);
            var orderByPayments = payments
                .OrderBy(x => x.PaymentNoteNo)
                .ThenBy(x => x.VendorPaymentDate)
                .GroupBy(g => g.VendorServiceName)
                .Select(s => new PDFYearlyData
                {
                    ServiceName = s.Key,
                    PaidAmount = s.Sum(x => x.VendorPaymentAmount),
                    VendorName = s?.FirstOrDefault(x => x.VendorServiceName == s.Key)?.VendorName,
                    SanctionedAmount = s?.FirstOrDefault(x => x.VendorServiceName == s.Key)?.ServiceSantionAmount,
                    PendingAmount = s?.FirstOrDefault(x => x.VendorServiceName == s.Key)?.ServiceSantionAmount - s.Sum(x => x.VendorPaymentAmount),
                    SrNo = Convert.ToInt32(Index.Start.Value) + 1

                }).ToList();
            int srNo = 1;
            foreach (var payment in orderByPayments) 
            {
                pdfData.Add(new PDFYearlyData() 
                {
                    SrNo=srNo,
                  //  ServiceName=payment.Key

                });
            }
            
        }
    }
}
