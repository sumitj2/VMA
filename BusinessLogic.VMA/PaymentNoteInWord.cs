using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Abstraction.VMA.Contract;
using System.Drawing;
using System.Windows;
using Color = DocumentFormat.OpenXml.Wordprocessing.Color;
using BusinessLogic.Abstraction.VMA.Contract;
using DocumentFormat.OpenXml.Drawing.Charts;
using VMA.Constants;

namespace BusinessLogic.VMA
{
    public class PaymentNoteInWord : IPaymentNoteInWord
    {
        private readonly IVenderPaymentNotesRepository _venderPaymentNotesRepository;
        public PaymentNoteInWord(IVenderPaymentNotesRepository venderPaymentNotesRepository)
        {
            _venderPaymentNotesRepository = venderPaymentNotesRepository;
        }
        public async Task CreateAndOpenWordFile(string? serviceName, string? from, string? to, string? bodyTextBefore, string? bodyTextAfter, string? financilaYear, string? path)
        {
            if (path != null)
            {
                var result = await _venderPaymentNotesRepository.GetAllServicePayments(serviceName, financilaYear).ConfigureAwait(true);
                if (result == null || result?.Count == 0)
                {
                    MessageBox.Show(MessagesContants.NoPaymentFound + serviceName);
                }
                else
                {
                    string? location = path + "\\" + serviceName + "_PaymentNote.docx";
                    // Ensure the directory exists
                    Directory.CreateDirectory(Path.GetDirectoryName(location));

                    // Create and save the Word document
                    using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(location, WordprocessingDocumentType.Document))
                    {
                        // Add a main document part
                        MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                        mainPart.Document = new Document();
                        Body body = new();

                        #region Title

                        Paragraph headingParagraph = new Paragraph();
                        Run headingRun = new Run();
                        RunProperties runPropertiesheading = new();
                        FontSize paragraphFontSize1 = new() { Val = "40" }; // 14pt font
                        Bold paragraphBold1 = new(); // Bold text

                        runPropertiesheading.Append(paragraphBold1);
                        runPropertiesheading.Append(paragraphFontSize1);

                        headingRun.Append(new Text("Thane Bharat Sahakari Bank Ltd"));
                        headingParagraph.Append(headingRun);
                        body.Append(headingParagraph);
                        headingRun.PrependChild(runPropertiesheading);

                        // Add the subheading.
                        Paragraph subheadingParagraph = new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Left }));
                        Run subheadingRun = new Run(new RunProperties(new FontSize() { Val = "18" }));
                        subheadingRun.Append(new Text("(Scheduled Bank)"));
                        subheadingParagraph.Append(subheadingRun);
                        body.Append(subheadingParagraph);

                        #endregion

                        #region Header Table

                        // Add a table
                        Table tableHeader = new Table();

                        SetTableProperties(tableHeader);

                        // Add a couple of rows
                        TableRow rowHeader = new TableRow();
                        TableCellProperties cellProperties = new TableCellProperties(
                            new TableCellWidth() { Width = "3000", Type = TableWidthUnitValues.Dxa });
                        rowHeader.Append(cellProperties);
                        rowHeader.Append
                            (
                            new TableCell(new Paragraph(new Run(new Text("From:" + from)))),
                            new TableCell(new Paragraph(new Run(new Text("To: " + to))))
                            );
                        tableHeader.Append(rowHeader);
                        //subject 
                        TableRow rowHeader2 = new TableRow();

                        TableCell mergedCell1 = new TableCell(new TableCellProperties(new GridSpan() { Val = 2 }));
                        mergedCell1.Append(new Paragraph(new Run(new Text("Sub : Payment To be made to  M/S " + result?.FirstOrDefault()?.VendorName + " " + result?.FirstOrDefault()?.VendorServiceName))));
                        rowHeader2.Append(mergedCell1);
                       
                        tableHeader.Append(rowHeader2);


                        TableRow rowHeader3 = new TableRow();
                        TableCell mergedCell2 = new TableCell(new TableCellProperties(new GridSpan() { Val = 2 }));
                        mergedCell2.Append(new Paragraph(new Run(new Text("Ref :" + result?.FirstOrDefault()?.ServiceSantionedBy + " " + result?.FirstOrDefault()?.SantionedDate.ToString()))));
                        rowHeader3.Append(mergedCell2);
                        tableHeader.Append(rowHeader3);

                        TableRow rowHeader4 = new TableRow();
                        rowHeader4.Append(
                            new TableCell(new Paragraph(new Run(new Text("Date :" + DateTime.Now)))),//Row 2, Cell 1
                            new TableCell(new Paragraph(new Run(new Text("Pay Note : " + result?.FirstOrDefault()?.PaymentNoteNo))))

                        );
                        tableHeader.Append(rowHeader4);

                        body.AppendChild(tableHeader);

                        #endregion

                        #region Paregraph

                        // Add a paragraph with custom text
                        Paragraph paragraphBodyBefore = new Paragraph();

                        // Create RunProperties and set the font size, bold, and other properties
                        RunProperties paragraphRunProperties = new RunProperties();


                        // Create a Run and apply the RunProperties
                        Run paragraphRun = new Run(new Text(bodyTextBefore));
                        paragraphRun.PrependChild(paragraphRunProperties);

                        // Add the Run to the Paragraph
                        paragraphBodyBefore.Append(paragraphRun);

                        // Add the Paragraph to the Body
                        body.Append(paragraphBodyBefore);

                        #endregion

                        #region Invoice

                        Table tableInvocie = new Table();

                        SetTableProperties(tableInvocie);

                        // Add a couple of rows
                        TableRow rowInvoice = new();
                        rowInvoice.Append
                        (
                          new TableCell(new Paragraph(new Run(new Text("SrNo ")))),
                          new TableCell(new Paragraph(new Run(new Text("Invoice No")))),
                          new TableCell(new Paragraph(new Run(new Text("Date ")))),
                          new TableCell(new Paragraph(new Run(new Text("Description")))),
                          new TableCell(new Paragraph(new Run(new Text("Qty ")))),
                          new TableCell(new Paragraph(new Run(new Text("Rate")))),
                          new TableCell(new Paragraph(new Run(new Text("Amount ")))),
                          new TableCell(new Paragraph(new Run(new Text("18% GST")))),
                          new TableCell(new Paragraph(new Run(new Text("Amount "))))
                        );
                        tableInvocie.Append(rowInvoice);
                        int srNo = 1;
                        if (result != null)
                        {
                            foreach (var invoice in result)
                            {
                                TableRow rowInvoice1 = new();
                                rowInvoice1.Append
                                (
                                  new TableCell(new Paragraph(new Run(new Text(srNo.ToString())))),
                                  new TableCell(new Paragraph(new Run(new Text(invoice?.InvoiceNumber ?? "")))),
                                  new TableCell(new Paragraph(new Run(new Text(invoice?.InvoiceDate.ToString() ?? "")))),
                                  new TableCell(new Paragraph(new Run(new Text(invoice?.InvoiceParticulars ?? "")))),
                                  new TableCell(new Paragraph(new Run(new Text(invoice?.QuantityOfUnit.ToString() ?? "")))),
                                  new TableCell(new Paragraph(new Run(new Text(invoice?.RatePerUnit ?? "")))),
                                  new TableCell(new Paragraph(new Run(new Text(invoice?.VendorPaymentAmount.ToString() ?? "")))),
                                  new TableCell(new Paragraph(new Run(new Text(invoice?.TotalGST.ToString() ?? "")))),
                                  new TableCell(new Paragraph(new Run(new Text(invoice?.TotalAmountPaid.ToString() ?? ""))))
                                );
                                srNo++;
                                tableInvocie.Append(rowInvoice1);
                            }
                        }
                        body.Append(tableInvocie);
                        #endregion

                        #region Paregraph

                        // Add a paragraph with custom text
                        Paragraph paragraphBodyAfter = new Paragraph();

                        // Create RunProperties and set the font size, bold, and other properties
                        RunProperties paragraphRunPropertiesAfter = new();

                        // Create a Run and apply the RunProperties
                        Run paragraphRunAfter = new Run(new Text(bodyTextAfter));
                        paragraphRun.PrependChild(paragraphRunPropertiesAfter);

                        // Add the Run to the Paragraph
                        paragraphBodyAfter.Append(paragraphRunAfter);

                        // Add the Paragraph to the Body
                        body.Append(paragraphBodyAfter);

                        #endregion

                        #region Footer Table
                        // Add a table
                        Table tableFooter = new();

                        SetTableProperties(tableFooter);

                        // Add a couple of rows
                        TableRow rowFooter1 = new TableRow();
                        rowFooter1.Append
                            (
                            new TableCell(new Paragraph(new Run(new Text("UTR No: ")))),
                            new TableCell(new Paragraph(new Run(new Text(result.Count() != 0 ? result?.FirstOrDefault().VendorPaymentUtrnumber : null)))),
                            new TableCell(new Paragraph(new Run(new Text("Amount: ")))),
                            new TableCell(new Paragraph(new Run(new Text(" "))))
                            );
                        tableFooter.Append(rowFooter1);
                        //subject 
                        TableRow rowFooter2 = new TableRow();
                        rowFooter2.Append
                            (
                            new TableCell(new Paragraph(new Run(new Text("Date")))),
                            new TableCell(new Paragraph(new Run(new Text("      ")))),
                            new TableCell(new Paragraph(new Run(new Text("TDS")))),
                            new TableCell(new Paragraph(new Run(new Text(result?.FirstOrDefault()?.VendorPaymentTdsamount.ToString()))))
                            );
                        tableFooter.Append(rowFooter2);

                        TableRow row2 = new TableRow();
                        row2.Append
                            (
                            new TableCell(new Paragraph(new Run(new Text("      ")))),
                            new TableCell(new Paragraph(new Run(new Text("      ")))),
                            new TableCell(new Paragraph(new Run(new Text("Total Amount Paid")))),
                            new TableCell(new Paragraph(new Run(new Text("      "))))
                            );
                        tableFooter.Append(row2);

                        body.AppendChild(tableFooter);

                        #endregion

                        mainPart.Document.Append(body);
                        mainPart.Document.Save();
                    }

                    // Open the Word file
                    OpenWordFile(location);
                }
            }
        }

        public async Task CreateAndOpenWordFile_New(string? serviceName, string? from, string? to, string? bodyTextBefore, string? bodyTextAfter, string? financilaYear, string? path)
        {
            var result = await _venderPaymentNotesRepository.GetAllServicePayments(serviceName, financilaYear).ConfigureAwait(true);

            var location = path + "\\" + serviceName + "_PaymentNote.docx";
            // Ensure the directory exists
            Directory.CreateDirectory(Path.GetDirectoryName(location));
            // Create a Wordprocessing document.
            using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(location, DocumentFormat.OpenXml.WordprocessingDocumentType.Document))
            {
                // Add a main document part.
                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body body = mainPart.Document.AppendChild(new Body());

                // Add the heading.
                Paragraph headingParagraph = new Paragraph();
                Run headingRun = new Run();
                headingRun.Append(new Text("Thane Bharat Sahakari Bank Ltd"));
                headingParagraph.Append(headingRun);
                body.Append(headingParagraph);

                // Add the subheading.
                Paragraph subheadingParagraph = new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Right }));
                Run subheadingRun = new Run(new RunProperties(new FontSize() { Val = "18" }));
                subheadingRun.Append(new Text("(Scheduled Bank)"));
                subheadingParagraph.Append(subheadingRun);
                body.Append(subheadingParagraph);

                // Create the first table.
                Table firstTable = new Table();
                SetTableProperties(firstTable);

                // First row.
                TableRow firstTableRow1 = new TableRow();
                firstTableRow1.Append(CreateTableCell("From", "1250"));

                TableCell mergedCell1 = new TableCell(new TableCellProperties(new GridSpan() { Val = 2 }, new TableCellWidth() { Width = "2500", Type = TableWidthUnitValues.Dxa }));
                mergedCell1.Append(new Paragraph(new Run(new Text("Sub : Payment To be made to Forebs Technosys"))));
                firstTableRow1.Append(mergedCell1);

                firstTableRow1.Append(CreateTableCell("Date: 3rd June 2024", "1250"));
                firstTable.Append(firstTableRow1);

                // Second row.
                TableRow firstTableRow2 = new TableRow();
                firstTableRow2.Append(CreateTableCell("Sr. Officer-Central office", "1250"));

                TableCell mergedCell2 = new TableCell(new TableCellProperties(new GridSpan() { Val = 2 }, new TableCellWidth() { Width = "2500", Type = TableWidthUnitValues.Dxa }));
                mergedCell2.Append(new Paragraph(new Run(new Text("Ref: Board Of Director"))));
                firstTableRow2.Append(mergedCell2);

                firstTableRow2.Append(CreateTableCell("Paynote: IT/PN/2024-2025", "1250"));
                firstTable.Append(firstTableRow2);

                body.Append(firstTable);

                // Add a paragraph.
                body.Append(new Paragraph(new Run(new Text("Theri board of director"))));

                // Create the second table.
                Table secondTable = new Table();
                SetTableProperties(secondTable);

                // Second table header row.
                TableRow headerRow = new TableRow();
                headerRow.Append(CreateTableCell("Sr No", "555"));
                headerRow.Append(CreateTableCell("Invoice No", "555"));
                headerRow.Append(CreateTableCell("Date", "555"));
                headerRow.Append(CreateTableCell("Description", "555"));
                headerRow.Append(CreateTableCell("Qty", "555"));
                headerRow.Append(CreateTableCell("Rate", "555"));
                headerRow.Append(CreateTableCell("Amount", "555"));
                headerRow.Append(CreateTableCell("18% GST", "555"));
                headerRow.Append(CreateTableCell("Amount", "555"));
                secondTable.Append(headerRow);


                // Populate second table rows from the database.

                foreach (var item in result)
                {
                    TableRow dataRow = new TableRow();
                    dataRow.Append(CreateTableCell(item.SrNo.ToString(), "555"));
                    dataRow.Append(CreateTableCell(item.InvoiceNumber, "555"));
                    dataRow.Append(CreateTableCell(item.InvoiceDate.ToString(), "555"));
                    dataRow.Append(CreateTableCell(item.InvoiceParticulars, "555"));
                    dataRow.Append(CreateTableCell(item.QuantityOfUnit.ToString(), "555"));
                    dataRow.Append(CreateTableCell(item.RatePerUnit.ToString(), "555"));
                    dataRow.Append(CreateTableCell(item.VendorPaymentAmount.ToString(), "555"));
                    dataRow.Append(CreateTableCell(item.TotalGST.ToString(), "555"));
                    dataRow.Append(CreateTableCell(item.TotalAmountPaid.ToString(), "555"));
                    secondTable.Append(dataRow);
                }

                body.Append(secondTable);

                // Add the second paragraph.
                body.Append(new Paragraph(new Run(new Text("2nd paragraph"))));

                // Add the third paragraph.
                body.Append(new Paragraph(new Run(new Text("Chief Manager-IT"))));

                // Create the third table.
                Table thirdTable = new Table();
                SetTableProperties(thirdTable);

                // Third table rows.
                TableRow thirdTableRow1 = new TableRow();
                thirdTableRow1.Append(CreateTableCell("URT No:", "1666"));
                thirdTableRow1.Append(CreateTableCell("Date:", "1666"));
                thirdTableRow1.Append(CreateTableCell("Blank", "1666"));
                thirdTable.Append(thirdTableRow1);

                TableRow thirdTableRow2 = new TableRow();
                thirdTableRow2.Append(CreateTableCell("", "1666"));
                thirdTableRow2.Append(CreateTableCell("", "1666"));
                thirdTableRow2.Append(CreateTableCell("", "1666"));
                thirdTable.Append(thirdTableRow2);

                TableRow thirdTableRow3 = new TableRow();
                thirdTableRow3.Append(CreateTableCell("Amount", "1666"));
                thirdTableRow3.Append(CreateTableCell("TDS", "1666"));
                thirdTableRow3.Append(CreateTableCell("Total Amount Paid", "1666"));
                thirdTable.Append(thirdTableRow3);

                TableRow thirdTableRow4 = new TableRow();
                thirdTableRow4.Append(CreateTableCell("", "1666"));
                thirdTableRow4.Append(CreateTableCell("", "1666"));
                thirdTableRow4.Append(CreateTableCell("", "1666"));
                thirdTable.Append(thirdTableRow4);

                body.Append(thirdTable);

                // Save the changes to the document.
                mainPart.Document.Save();
            }
        }
        private TableCell CreateTableCell(string text, string width)
        {
            TableCell cell = new TableCell();
            TableCellProperties cellProperties = new TableCellProperties(
                new TableCellWidth() { Width = width, Type = TableWidthUnitValues.Dxa }
            );
            cell.Append(cellProperties);
            cell.Append(new Paragraph(new Run(new Text(text))));
            return cell;
        }
        private void SetTableProperties(Table table)
        {
            TableProperties tableProperties = new TableProperties(
                new TableWidth() { Width = "10000", Type = TableWidthUnitValues.Dxa },
                new TableBorders(
                    new TopBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                    new BottomBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                    new LeftBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                    new RightBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                    new InsideHorizontalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                    new InsideVerticalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 }
                )
            );
            table.AppendChild(tableProperties);
        }
        private void OpenWordFile(string filePath)
        {
            try
            {
                Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during opening the file
                Debug.WriteLine($"{MessagesContants.ErrorOpeningFile} {ex.Message}");
            }
        }
    }

}
