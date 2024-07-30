using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.Abstraction.VMA.Models;
using Database.Abstraction.VMA.Contract;
using Database.VMA.Entities.CustomEntities;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BusinessLogic.VMA
{
    public class YearlyMonthlyReportPDF : IYearlyMonthlyReportPDF
    {
        private readonly IStoreProcedureExecutionRepository _storeProcedureExecutionRepository;
        public YearlyMonthlyReportPDF(IStoreProcedureExecutionRepository storeProcedureExecutionRepository)
        {
            _storeProcedureExecutionRepository = storeProcedureExecutionRepository;
        }

        public Task GenerateMonthlyReport(string? financilaYear, string? Month, string? path)
        {
            return null;
        }

        private void GeneratePdf(List<YearlyReportData> Data,string? path,string financilaYear)
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
            int count = 1;
            // Add data rows
            foreach (var item in Data)
            {
                row = table.AddRow();
                row.Cells[0].AddParagraph(count.ToString());
                row.Cells[1].AddParagraph(item?.VendorName);
                row.Cells[2].AddParagraph(item.VendorServiceName);
                row.Cells[3].AddParagraph(item?.ServiceSantionAmount.ToString());
                row.Cells[4].AddParagraph(item?.TotalVendorPaymentAmount.ToString());
                row.Cells[5].AddParagraph(item?.RemainingAmount.ToString());
                count++;
            }

            // Render the document
            var pdfRenderer = new PdfDocumentRenderer(true) { Document = document };
            pdfRenderer.RenderDocument();
            path = path+"\\" + financilaYear+DateTime.Now.ToString("ff") + "YearlyRepor.pdf";
            pdfRenderer.PdfDocument.Save(path);
            OpenPdf(path);
        }
        public void OpenPdf(string filePath)
        {
            // Ensure the file path is valid
            if (System.IO.File.Exists(filePath))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true // Ensures the file is opened with the default application
                });
            }
            else
            {
                // Handle file not found scenario
                MessageBox.Show("File not found.");
            }
        }
        public async Task GenerateYearlyReport(string? financilaYear, string? path)
        {
            List<PDFYearlyData> pdfData = [];
            var payments = await _storeProcedureExecutionRepository.GetYearlyReportDataAsync(financilaYear).ConfigureAwait(true);

            GeneratePdf(payments, path,financilaYear);
        }
    }
}
