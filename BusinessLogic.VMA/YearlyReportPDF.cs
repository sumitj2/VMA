using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.Abstraction.VMA.Models;
using Database.Abstraction.VMA.Contract;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
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

        private void GeneratePdf(List<PDFYearlyData> Data)
        {
            var document = new Document();
            var section = document.AddSection();
            var table = section.AddTable();
            table.Borders.Width = 0.75;

            // Define columns
            var columnWidths = new[] { "1cm", "4cm", "4cm", "2cm", "2cm", "2cm" };
            foreach (var width in columnWidths)
            {
                var column = table.AddColumn(width);
                column.Format.Alignment = ParagraphAlignment.Center;
            }

            // Add header row
            var row = table.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;
            row.Cells[0].AddParagraph("Sr No");
            row.Cells[1].AddParagraph("Vendor Name");
            row.Cells[2].AddParagraph("Service Name");
            row.Cells[3].AddParagraph("Sanctioned Amt");
            row.Cells[4].AddParagraph("Paid Amt");
            row.Cells[5].AddParagraph("Pending Amt");

            // Add data rows
            foreach (var item in Data)
            {
                row = table.AddRow();
                row.Cells[0].AddParagraph(item.SrNo.ToString());
                row.Cells[1].AddParagraph(item.VendorName);
                row.Cells[2].AddParagraph(item.ServiceName);
                row.Cells[3].AddParagraph(item?.SanctionedAmount?.ToString());
                row.Cells[4].AddParagraph(item?.PaidAmount?.ToString());
                row.Cells[5].AddParagraph(item?.PendingAmount?.ToString());
            }

            // Render the document
            var pdfRenderer = new PdfDocumentRenderer(true) { Document = document };
            pdfRenderer.RenderDocument();
            pdfRenderer.PdfDocument.Save("Report.pdf");
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
            GeneratePdf(orderByPayments);



        }
    }
}
