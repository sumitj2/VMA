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
using Style = MigraDoc.DocumentObjectModel.Style;

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

        private void GeneratePdf(List<YearlyReportData> Data, string? path, string financilaYear)
        {
            var document = new Document();
            var section = document.AddSection();

            // Define a heading style
            Style headingStyle = document.Styles["Heading1"];
            headingStyle.Font.Name = "Verdana";
            headingStyle.Font.Size = 20;
            headingStyle.Font.Bold = true;

            // Add a heading
            Paragraph heading = section.AddParagraph("Thane Bharat Bank", "Heading1");
            heading.Format.Alignment = ParagraphAlignment.Center;
            section.AddParagraph(); // Adds an empty paragraph to create space

            // Add the additional paragraph
            Paragraph additionalText = section.AddParagraph("Yearly Vendor Service Report");
            additionalText.Format.Alignment = ParagraphAlignment.Center;


            section.AddParagraph();
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
            decimal? santionedAmtTotal = 0;
            decimal? TotalAmountPaid = 0;
            decimal? TotalRemainingAmt = 0;

            // Add data rows
            foreach (var item in Data)
            {
                row = table.AddRow();
                row.Cells[0].AddParagraph(count.ToString());
                row.Cells[1].AddParagraph(item?.VendorName);
                row.Cells[2].AddParagraph(item.VendorServiceName);
                row.Cells[3].AddParagraph(item?.ServiceSantionAmount.ToString());
                row.Cells[4].AddParagraph(item?.TotalVendorPaymentAmount != null ? item?.TotalVendorPaymentAmount.ToString():0.ToString());
                row.Cells[5].AddParagraph(item?.RemainingAmount.ToString());
                count++;
                santionedAmtTotal += item?.ServiceSantionAmount;
                TotalAmountPaid += item?.TotalVendorPaymentAmount != null ? item?.TotalVendorPaymentAmount : 0;
                TotalRemainingAmt += item?.RemainingAmount;
            }
            row = table.AddRow();
            row.Cells[0].AddParagraph("Total");
            row.Cells[1].AddParagraph("");
            row.Cells[2].AddParagraph("");
            row.Cells[3].AddParagraph(santionedAmtTotal.ToString());
            row.Cells[4].AddParagraph(TotalAmountPaid.ToString());
            row.Cells[5].AddParagraph(TotalRemainingAmt.ToString());

            // Render the document
            var pdfRenderer = new PdfDocumentRenderer(true) { Document = document };
            pdfRenderer.RenderDocument();
            path = path + "\\" + financilaYear + DateTime.Now.ToString("ff") + "YearlyRepor.pdf";
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

            GeneratePdf(payments, path, financilaYear);
        }
    }
}
