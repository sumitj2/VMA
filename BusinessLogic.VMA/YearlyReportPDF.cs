using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.Abstraction.VMA.Models;
using Database.Abstraction.VMA.Contract;
using Database.VMA.Entities.CustomEntities;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VMA.Constants;
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

        private void GeneratePdf(List<YearlyReportData> Data, string? path, string? financilaYear)
        {
            if (Data == null || Data.Count == 0)
            {
                MessageBox.Show(MessagesContants.NoPaymentFound);
                return;
            }

            var document = new Document();
            var section = document?.AddSection();

            // Page setup for better centering
            section.PageSetup.LeftMargin = "2cm";
            section.PageSetup.RightMargin = "2cm";
            section.PageSetup.TopMargin = "2cm";
            section.PageSetup.BottomMargin = "2cm";

            // Define a heading style
            Style headingStyle = document.Styles["Heading1"];
            headingStyle.Font.Name = "Verdana";
            headingStyle.Font.Size = 20;
            headingStyle.Font.Bold = true;

            // Add a heading
            Paragraph heading = section.AddParagraph(MessagesContants.BankName, "Heading1");
            heading.Format.Alignment = ParagraphAlignment.Center;
            section.AddParagraph(); // Adds an empty paragraph to create space

            // Add the additional paragraph
            Paragraph additionalText = section.AddParagraph(MessagesContants.YearlyReport);
            additionalText.Format.Alignment = ParagraphAlignment.Center;

            section.AddParagraph();

            // Create and center the table
            var table = section.AddTable();
            table.Borders.Width = 0.75;
            table.Borders.Color = Colors.Black;
            table.Format.Alignment = ParagraphAlignment.Center;  // Align table to center

            // Define columns with auto-sizing widths
            var columnWidths = new[] { "1cm", "3.5cm", "3.5cm", "3.5cm", "2cm", "3.5cm" };
            foreach (var width in columnWidths)
            {
                var column = table.AddColumn(width);
                column.Format.Alignment = ParagraphAlignment.Center;
            }

            // Add header row with background color
            var row = table.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;
            row.Shading.Color = Colors.LightGray;

            row.Cells[0]?.AddParagraph("Sr No");
            row.Cells[1]?.AddParagraph("Vendor Name");
            row.Cells[2]?.AddParagraph("Service Name");
            row.Cells[3]?.AddParagraph("Sanctioned Amt");
            row.Cells[4]?.AddParagraph("Paid Amt");
            row.Cells[5]?.AddParagraph("Pending Amt");

            int count = 1;
            decimal? santionedAmtTotal = 0;
            decimal? totalAmountPaid = 0;
            decimal? totalRemainingAmt = 0;

            // Add data rows
            foreach (var item in Data)
            {
                row = table.AddRow();
                row.Cells[0]?.AddParagraph(count.ToString());
                row.Cells[1]?.AddParagraph(item?.VendorName ?? "");
                row.Cells[2]?.AddParagraph(item?.VendorServiceName ?? "");
                row.Cells[3]?.AddParagraph(item?.ServiceSantionAmount?.ToString("N2") ?? "0");
                row.Cells[4]?.AddParagraph(item?.TotalVendorPaymentAmount?.ToString("N2") ?? "0");
                row.Cells[5]?.AddParagraph(item?.RemainingAmount?.ToString("N2") ?? "0");

                count++;
                santionedAmtTotal += item?.ServiceSantionAmount ?? 0;
                totalAmountPaid += item?.TotalVendorPaymentAmount ?? 0;
                totalRemainingAmt += item?.RemainingAmount ?? 0;
            }

            // Add total row
            row = table.AddRow();
            row.Cells[0]?.AddParagraph("Total");
            row.Cells[1].MergeRight = 1; // Merge Vendor Name and Service Name cells
            row.Cells[3]?.AddParagraph(santionedAmtTotal?.ToString("N2") ?? "");
            row.Cells[4]?.AddParagraph(totalAmountPaid?.ToString("N2") ?? "");
            row.Cells[5]?.AddParagraph(totalRemainingAmt?.ToString("N2") ?? "");

            // Render the document to PDF
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(true) { Document = document };
            pdfRenderer.RenderDocument();

            string fileName = Path.Combine(path, $"YearlyReport_{financilaYear}.pdf");
            pdfRenderer.PdfDocument.Save(fileName);
            OpenPdf(fileName);
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
                MessageBox.Show(MessagesContants.FileNotFound);
            }
        }
        public async Task GenerateYearlyReport(string? financilaYear, string? path)
        {
            List<PDFYearlyData> pdfData = [];
            var payments = await _storeProcedureExecutionRepository.GetYearlyReportDataAsync(financilaYear).ConfigureAwait(true);
            try
            {
                GeneratePdf(payments, path, financilaYear);
            }
            catch (Exception ex)
            {
                throw new Exception("While GenerateYearlyReport-> GeneratePDF exception occurs : " + ex);
            }
        }
    }
}
