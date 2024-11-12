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
using DocumentFormat.OpenXml.VariantTypes;
using Database.VMA.Entities.CustomEntities;

namespace BusinessLogic.VMA
{
    public class PaymentNoteInWord : IPaymentNoteInWord
    {
        private readonly IVenderPaymentNotesRepository _venderPaymentNotesRepository;
        public PaymentNoteInWord(IVenderPaymentNotesRepository venderPaymentNotesRepository)
        {
            _venderPaymentNotesRepository = venderPaymentNotesRepository;
        }
        private string GetUniqueFileName(string filePath)
        {
            string directory = Path.GetDirectoryName(filePath);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);

            int counter = 1;
            string newFilePath = filePath;

            // Loop until we find a filename that doesn't exist
            while (File.Exists(newFilePath))
            {
                newFilePath = Path.Combine(directory, $"{fileNameWithoutExtension} ({counter}){extension}");
                counter++;
            }

            return newFilePath;
        }
        public async Task CreateAndOpenWordFileForNone(List<string> serviceName, string? from, string? to, string? bodyTextBefore, string? bodyTextAfter, string? financilaYear, string? path, string vendorName, string paymentNoteNo, DateOnly noteGeneartionDate)
        {
            if (path != null)
            {
                var result = await _venderPaymentNotesRepository.GetAllServicePayments(serviceName, financilaYear, vendorName, paymentNoteNo).ConfigureAwait(true);
                if (result == null || result?.Count == 0)
                {
                    MessageBox.Show(MessagesContants.NoPaymentFound + string.Join(",", serviceName));
                }
                else
                {
                    string? location = path + "\\" + serviceName + "_PaymentNote.docx";

                    // Ensure the file does not get overridden by generating a unique file name
                    string uniqueLocation = GetUniqueFileName(location);

                    // Create and save the Word document
                    using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(uniqueLocation, WordprocessingDocumentType.Document))
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
                        mergedCell1.Append(new Paragraph(new Run(new Text("Sub : Payment To be made to  M/S " + result?.FirstOrDefault()?.VendorName))));
                        rowHeader2.Append(mergedCell1);

                        tableHeader.Append(rowHeader2);


                        TableRow rowHeader3 = new TableRow();
                        TableCell mergedCell2 = new TableCell(new TableCellProperties(new GridSpan() { Val = 2 }));
                        mergedCell2.Append(new Paragraph(new Run(new Text("Ref :" + result?.FirstOrDefault()?.ServiceSantionedBy + " dated " + result?.FirstOrDefault()?.SantionedDate.ToString()))));
                        rowHeader3.Append(mergedCell2);
                        tableHeader.Append(rowHeader3);

                        TableRow rowHeader4 = new TableRow();
                        rowHeader4.Append(
                            new TableCell(new Paragraph(new Run(new Text("Date :" + result.FirstOrDefault().VendorPaymentDate.ToString())))),//Row 2, Cell 1
                            new TableCell(new Paragraph(new Run(new Text("Payment Note No.: " + result?.FirstOrDefault()?.PaymentNoteNo))))

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
                          new TableCell(new Paragraph(new Run(new Text("Particular ")))),
                          new TableCell(new Paragraph(new Run(new Text("Description")))),
                          new TableCell(new Paragraph(new Run(new Text("Qty ")))),
                          new TableCell(new Paragraph(new Run(new Text("Rate")))),
                          new TableCell(new Paragraph(new Run(new Text("Amount ")))),
                          new TableCell(new Paragraph(new Run(new Text("18% GST")))),
                          new TableCell(new Paragraph(new Run(new Text("Amount "))))
                        );
                        tableInvocie.Append(rowInvoice);


                        //TableRow rowHeaderMege = new TableRow();
                        //TableCell mergedCellInvoice = new TableCell(new TableCellProperties(new GridSpan() { Val = 9 }));
                        //mergedCellInvoice.Append(new Paragraph(new Run(new Text(result.FirstOrDefault().InvoiceParticulars.ToString()))));
                        //rowHeaderMege.Append(mergedCellInvoice);

                        //tableInvocie.Append(rowHeaderMege);

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
                                  new TableCell(new Paragraph(new Run(new Text(Convert.ToDouble(invoice?.InvoiceDate).ToString() ?? "")))),
                                  new TableCell(new Paragraph(new Run(new Text(invoice?.InvoiceParticulars ?? "")))),
                                  new TableCell(new Paragraph(new Run(new Text(invoice?.QuantityOfUnit.ToString() ?? "")))),
                                  new TableCell(new Paragraph(new Run(new Text(invoice?.RatePerUnit ?? "")))),
                                  new TableCell(new Paragraph(new Run(new Text(Convert.ToDouble(invoice?.VendorPaymentAmount).ToString() ?? "")))),
                                  new TableCell(new Paragraph(new Run(new Text(Convert.ToDouble(invoice?.TotalGST).ToString() ?? "")))),
                                  new TableCell(new Paragraph(new Run(new Text(Convert.ToDouble(invoice?.TotalAmountPaid).ToString() ?? ""))))
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

                        #region To

                        // Add a paragraph with custom text
                        Paragraph paragraphBodyAfterTo = new Paragraph();

                        // Create RunProperties and set the font size, bold, and other properties
                        RunProperties paragraphRunPropertiesAfterTo = new();

                        // Create a Run and apply the RunProperties
                        Run paragraphRunAfterTo = new Run(new Text(to));
                        paragraphRun.PrependChild(paragraphRunPropertiesAfterTo);

                        // Add the Run to the Paragraph
                        paragraphBodyAfter.Append(paragraphRunAfterTo);

                        // Add the Paragraph to the Body
                        body.Append(paragraphBodyAfterTo);

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
                            new TableCell(new Paragraph(new Run(new Text(""))))
                            );
                        tableFooter.Append(rowFooter1);
                        //subject 
                        TableRow rowFooter2 = new TableRow();
                        rowFooter2.Append
                            (
                            new TableCell(new Paragraph(new Run(new Text("Date")))),
                            new TableCell(new Paragraph(new Run(new Text(result.FirstOrDefault().VendorPaymentDate.ToString())))),
                            new TableCell(new Paragraph(new Run(new Text("TDS")))),
                            new TableCell(new Paragraph(new Run(new Text(Convert.ToDecimal(result?.FirstOrDefault()?.VendorPaymentTdsamount).ToString()))))
                            );
                        tableFooter.Append(rowFooter2);

                        decimal amtPaid = Convert.ToDecimal(0) - Convert.ToDecimal(result?.FirstOrDefault()?.VendorPaymentTdsamount);
                        TableRow row2 = new TableRow();
                        row2.Append
                            (
                            new TableCell(new Paragraph(new Run(new Text("      ")))),
                            new TableCell(new Paragraph(new Run(new Text("      ")))),
                            new TableCell(new Paragraph(new Run(new Text("Total Amount Paid")))),
                            new TableCell(new Paragraph(new Run(new Text(amtPaid.ToString()))))
                            );
                        tableFooter.Append(row2);

                        body.AppendChild(tableFooter);

                        #endregion

                        mainPart.Document.Append(body);
                        mainPart.Document.Save();
                    }

                    // Open the Word file
                    OpenWordFile(uniqueLocation);
                }
            }
        }

        //public async Task CreateAndOpenWordFile(List<string> serviceName, string? from, string? to, string? bodyTextBefore, string? bodyTextAfter, string? financilaYear, string? path, string vendorName, string paymentNoteNo, DateOnly noteGeneartionDate)
        //{
        //    if (path != null)
        //    {
        //        var result = await _venderPaymentNotesRepository.GetAllServicePayments(serviceName, financilaYear, vendorName, paymentNoteNo).ConfigureAwait(true);
        //        if (result == null || result?.Count == 0)
        //        {
        //            MessageBox.Show(MessagesContants.NoPaymentFound + string.Join(",", serviceName));
        //        }
        //        else
        //        {
        //            string? location = path + "\\" + vendorName + "_PaymentNote.docx";
        //            // Ensure the file does not get overridden by generating a unique file name
        //            string uniqueLocation = GetUniqueFileName(location);

        //            try
        //            {
        //                // Create and save the Word document
        //                using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(uniqueLocation, WordprocessingDocumentType.Document))
        //                {
        //                    // Add a main document part
        //                    MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
        //                    mainPart.Document = new Document();
        //                    Body body = new();

        //                    #region Title

        //                    Paragraph headingParagraph = new Paragraph();
        //                    Run headingRun = new Run();
        //                    RunProperties runPropertiesheading = new();
        //                    FontSize paragraphFontSize1 = new() { Val = "40" }; // 14pt font
        //                    Bold paragraphBold1 = new(); // Bold text

        //                    runPropertiesheading.Append(paragraphBold1);
        //                    runPropertiesheading.Append(paragraphFontSize1);

        //                    ParagraphProperties paragraphProperties = new ParagraphProperties();
        //                    Justification justification = new Justification() { Val = JustificationValues.Center };
        //                    paragraphProperties.Append(justification);

        //                    // Apply the properties to the paragraph.
        //                    headingParagraph.Append(paragraphProperties);

        //                    headingRun.Append(new Text("Thane Bharat Sahakari Bank Ltd"));
        //                    headingParagraph.Append(headingRun);
        //                    body.Append(headingParagraph);
        //                    headingRun.PrependChild(runPropertiesheading);

        //                    // Add the subheading.
        //                    Paragraph subheadingParagraph = new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Right }));
        //                    Run subheadingRun = new Run(new RunProperties(new FontSize() { Val = "18" }));
        //                    subheadingRun.Append(new Text("(Scheduled Bank)"));
        //                    subheadingParagraph.Append(subheadingRun);
        //                    body.Append(subheadingParagraph);

        //                    #endregion

        //                    #region Header Table

        //                    // Add a table
        //                    Table tableHeader = new Table();

        //                    SetTableProperties(tableHeader);

        //                    // Add a couple of rows
        //                    TableRow rowHeader = new TableRow();
        //                    TableCellProperties cellProperties = new TableCellProperties(
        //                        new TableCellWidth() { Width = "3000", Type = TableWidthUnitValues.Dxa });
        //                    rowHeader.Append(cellProperties);
        //                    rowHeader.Append
        //                        (
        //                        new TableCell(new Paragraph(new Run(new Text("From:" + from)))),
        //                        new TableCell(new Paragraph(new Run(new Text("To: " + to))))
        //                        );
        //                    tableHeader.Append(rowHeader);
        //                    //subject 
        //                    TableRow rowHeader2 = new TableRow();

        //                    TableCell mergedCell1 = new TableCell(new TableCellProperties(new GridSpan() { Val = 2 }));
        //                    mergedCell1.Append(new Paragraph(new Run(new Text("Sub : Payment To be made to  M/S " + result?.FirstOrDefault()?.VendorName))));
        //                    rowHeader2.Append(mergedCell1);

        //                    tableHeader.Append(rowHeader2);


        //                    TableRow rowHeader3 = new TableRow();
        //                    TableCell mergedCell2 = new TableCell(new TableCellProperties(new GridSpan() { Val = 2 }));
        //                    mergedCell2.Append(new Paragraph(new Run(new Text("Ref :" + result?.FirstOrDefault()?.ServiceSantionedBy + " dated " + result?.FirstOrDefault()?.SantionedDate.ToString()))));
        //                    rowHeader3.Append(mergedCell2);
        //                    tableHeader.Append(rowHeader3);

        //                    TableRow rowHeader4 = new TableRow();
        //                    rowHeader4.Append(
        //                        new TableCell(new Paragraph(new Run(new Text("Date :" + result?.FirstOrDefault()?.VendorPaymentDate.ToString())))),//Row 2, Cell 1
        //                        new TableCell(new Paragraph(new Run(new Text("Payment Note No.: " + result?.FirstOrDefault()?.PaymentNoteNo))))

        //                    );
        //                    tableHeader.Append(rowHeader4);

        //                    body.AppendChild(tableHeader);

        //                    #endregion

        //                    #region Paregraph

        //                    // Add a paragraph with custom text
        //                    Paragraph paragraphBodyBefore = new Paragraph();

        //                    // Create RunProperties and set the font size, bold, and other properties
        //                    RunProperties paragraphRunProperties = new RunProperties();


        //                    // Create a Run and apply the RunProperties
        //                    Run paragraphRun = new Run(new Text(bodyTextBefore));
        //                    paragraphRun.PrependChild(paragraphRunProperties);

        //                    // Add the Run to the Paragraph
        //                    paragraphBodyBefore.Append(paragraphRun);

        //                    // Add the Paragraph to the Body
        //                    body.Append(paragraphBodyBefore);

        //                    #endregion

        //                    #region Invoice

        //                    Table tableInvocie = new Table();

        //                    SetTableProperties(tableInvocie);

        //                    // Add a couple of rows
        //                    TableRow rowInvoice = new();
        //                    rowInvoice.Append
        //                    (
        //                      new TableCell(new Paragraph(new Run(new Text("SrNo ")))),
        //                      new TableCell(new Paragraph(new Run(new Text("Invoice No")))),
        //                      new TableCell(new Paragraph(new Run(new Text("Date ")))),
        //                      new TableCell(new Paragraph(new Run(new Text("Particular ")))),
        //                      new TableCell(new Paragraph(new Run(new Text("Sub Total")))),
        //                      new TableCell(new Paragraph(new Run(new Text("CGST ")))),
        //                      new TableCell(new Paragraph(new Run(new Text("SGST")))),
        //                      new TableCell(new Paragraph(new Run(new Text("IGST")))),
        //                      new TableCell(new Paragraph(new Run(new Text("Amount "))))
        //                    );


        //                    TableRow rowHeaderMege = new TableRow();
        //                    TableCell mergedCellInvoice = new TableCell(new TableCellProperties(new GridSpan() { Val = 9 }));
        //                    //mergedCellInvoice.Append(new Paragraph(new Run(new Text(result.FirstOrDefault().InvoiceParticulars.ToString()))));
        //                    //rowHeaderMege.Append(mergedCellInvoice);

        //                    tableInvocie.Append(rowInvoice);
        //                    //tableInvocie.Append(rowHeaderMege);
        //                    int srNo = 1;
        //                    decimal totalAmountPaid = 0;
        //                    if (result != null)
        //                    {

        //                        foreach (var invoice in result)
        //                        {
        //                            TableRow rowInvoice1 = new();
        //                            rowInvoice1.Append
        //                            (
        //                              new TableCell(new Paragraph(new Run(new Text(srNo.ToString())))),
        //                              new TableCell(new Paragraph(new Run(new Text(invoice?.InvoiceNumber ?? "")))),
        //                              new TableCell(new Paragraph(new Run(new Text(invoice?.InvoiceDate != null ? invoice?.InvoiceDate.Value.ToShortDateString().ToString() ?? "" : "")))),
        //                              new TableCell(new Paragraph(new Run(new Text(invoice?.InvoiceParticulars?.ToString() ?? "")))),
        //                              new TableCell(new Paragraph(new Run(new Text(string.Format("{0:F2}", invoice?.VendorPaymentAmount).ToString() ?? "")))),
        //                              new TableCell(new Paragraph(new Run(new Text(string.Format("{0:F2}", invoice?.VendorPaymentCgst).ToString() ?? "")))),
        //                              new TableCell(new Paragraph(new Run(new Text(string.Format("{0:F2}", invoice?.VendorPaymentSgst).ToString() ?? "")))),
        //                              new TableCell(new Paragraph(new Run(new Text(string.Format("{0:F2}", invoice?.VendorPaymentIgst).ToString() ?? "")))),
        //                              new TableCell(new Paragraph(new Run(new Text(string.Format("{0:F2}", invoice?.TotalAmountPaid).ToString() ?? ""))))
        //                            );
        //                            srNo++;
        //                            tableInvocie.Append(rowInvoice1);
        //                            totalAmountPaid = Convert.ToDecimal(totalAmountPaid + invoice?.TotalAmountPaid);
        //                        }
        //                        TableRow rowInvoice2 = new();
        //                        rowInvoice2.Append
        //                             (
        //                               new TableCell(new Paragraph(new Run(new Text()))),
        //                               new TableCell(new Paragraph(new Run(new Text()))),
        //                               new TableCell(new Paragraph(new Run(new Text()))),
        //                               new TableCell(new Paragraph(new Run(new Text()))),
        //                               new TableCell(new Paragraph(new Run(new Text()))),
        //                               new TableCell(new Paragraph(new Run(new Text()))),
        //                               new TableCell(new Paragraph(new Run(new Text()))),
        //                               new TableCell(new Paragraph(new Run(new Text("Total")))),
        //                               new TableCell(new Paragraph(new Run(new Text(string.Format("{0:F2}", totalAmountPaid).ToString()))))
        //                             );
        //                        srNo++;
        //                        tableInvocie.Append(rowInvoice2);
        //                    }
        //                    body.Append(tableInvocie);
        //                    #endregion

        //                    #region Paregraph

        //                    // Add a paragraph with custom text
        //                    Paragraph paragraphBodyAfter = new Paragraph();

        //                    // Create RunProperties and set the font size, bold, and other properties
        //                    RunProperties paragraphRunPropertiesAfter = new();

        //                    // Create a Run and apply the RunProperties
        //                    Run paragraphRunAfter = new Run(new Text(bodyTextAfter));
        //                    paragraphRun.PrependChild(paragraphRunPropertiesAfter);

        //                    // Add the Run to the Paragraph
        //                    paragraphBodyAfter.Append(paragraphRunAfter);

        //                    // Add the Paragraph to the Body
        //                    body.Append(paragraphBodyAfter);

        //                    #endregion

        //                    #region To

        //                    // Add a paragraph with custom text
        //                    Paragraph paragraphBodyAfterTo = new Paragraph();

        //                    // Create RunProperties and set the font size, bold, and other properties
        //                    RunProperties paragraphRunPropertiesAfterTo = new();

        //                    // Create a Run and apply the RunProperties
        //                    Run paragraphRunAfterTo = new Run(new Text(from));
        //                    paragraphRun.PrependChild(paragraphRunPropertiesAfterTo);

        //                    // Add the Run to the Paragraph
        //                    paragraphBodyAfterTo.Append(paragraphRunAfterTo);

        //                    // Add the Paragraph to the Body
        //                    body.Append(paragraphBodyAfterTo);

        //                    #endregion

        //                    #region Footer Table
        //                    // Add a table
        //                    Table tableFooter = new();

        //                    SetTableProperties(tableFooter);

        //                    // Add a couple of rows
        //                    TableRow rowFooter1 = new TableRow();
        //                    rowFooter1.Append
        //                        (
        //                        new TableCell(new Paragraph(new Run(new Text("UTR No: ")))),
        //                        new TableCell(new Paragraph(new Run(new Text(result.Count() != 0 ? result?.FirstOrDefault().VendorPaymentUtrnumber : null)))),
        //                        new TableCell(new Paragraph(new Run(new Text("Amount: ")))),
        //                        new TableCell(new Paragraph(new Run(new Text(string.Format("{0:F2}", totalAmountPaid).ToString()))))
        //                        );
        //                    tableFooter.Append(rowFooter1);
        //                    //subject 
        //                    TableRow rowFooter2 = new TableRow();
        //                    rowFooter2.Append
        //                        (
        //                        new TableCell(new Paragraph(new Run(new Text("Date")))),
        //                        new TableCell(new Paragraph(new Run(new Text(result?.FindAll(x=>x.VendorPaymentRtgsDate!=null)?.FirstOrDefault()?.VendorPaymentRtgsDate?.ToString())))),
        //                        new TableCell(new Paragraph(new Run(new Text("TDS")))),
        //                        new TableCell(new Paragraph(new Run(new Text(string.Format("{0:F2}", result?.FirstOrDefault()?.VendorPaymentTdsamount)))))
        //                        );
        //                    tableFooter.Append(rowFooter2);

        //                    decimal amtPaid = Convert.ToDecimal(totalAmountPaid) - Convert.ToDecimal(result?.FirstOrDefault()?.VendorPaymentTdsamount);
        //                    TableRow row2 = new TableRow();
        //                    row2.Append
        //                        (
        //                        new TableCell(new Paragraph(new Run(new Text("      ")))),
        //                        new TableCell(new Paragraph(new Run(new Text("      ")))),
        //                        new TableCell(new Paragraph(new Run(new Text("Total Amount Paid")))),
        //                        new TableCell(new Paragraph(new Run(new Text(string.Format("{0:F2}", amtPaid).ToString()))))
        //                        );
        //                    tableFooter.Append(row2);

        //                    body.AppendChild(tableFooter);

        //                    #endregion

        //                    mainPart.Document.Append(body);
        //                    mainPart.Document.Save();
        //                }
        //            }
        //            catch (Exception ex)
        //            {

        //                //MessageBox.Show(MessagesContants.FIlePresent + string.Join(",", serviceName));
        //            }

        //            // Open the Word file
        //            OpenWordFile(uniqueLocation);
        //        }
        //    }
        //}
        //private TableCell CreateTableCell(string text, string width)
        //{
        //    TableCell cell = new TableCell();
        //    TableCellProperties cellProperties = new TableCellProperties(
        //        new TableCellWidth() { Width = width, Type = TableWidthUnitValues.Dxa }
        //    );
        //    cell.Append(cellProperties);
        //    cell.Append(new Paragraph(new Run(new Text(text))));
        //    return cell;
        //}
        //private void SetTableProperties(Table table)
        //{
        //    TableProperties tableProperties = new TableProperties(
        //        new TableWidth() { Width = "10000", Type = TableWidthUnitValues.Dxa },
        //        new TableBorders(
        //            new TopBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
        //            new BottomBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
        //            new LeftBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
        //            new RightBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
        //            new InsideHorizontalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
        //            new InsideVerticalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 }
        //        )
        //    );
        //    table.AppendChild(tableProperties);
        //}
        public async Task CreateAndOpenWordFile(
                          List<string> serviceName,
                          string? from,
                          string? to,
                          string? bodyTextBefore,
                          string? bodyTextAfter,
                          string? financialYear,
                          string? path,
                          string vendorName,
                          string paymentNoteNo,
                          DateOnly noteGenerationDate)
        {
            if (path == null) return;

            var result = await _venderPaymentNotesRepository.GetAllServicePayments(serviceName, financialYear, vendorName, paymentNoteNo).ConfigureAwait(false);

            if (result == null || !result.Any())
            {
                MessageBox.Show(MessagesContants.NoPaymentFound + string.Join(",", serviceName));
                return;
            }

            string uniqueLocation = GetUniqueFileName($"{path}\\{vendorName}_PaymentNote.docx");

            try
            {
                using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(uniqueLocation, WordprocessingDocumentType.Document))
                {
                    MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                    mainPart.Document = new Document();
                    Body body = new Body();

                    AppendTitle(body);
                    AppendHeaderTable(body, result?.FirstOrDefault(), from, to);
                    AppendParagraph(body, bodyTextBefore);
                    AppendInvoiceTable(body, result);
                    AppendParagraph(body, bodyTextAfter);
                    AppendFooterTable(body, result);

                    mainPart.Document.Append(body);
                    mainPart.Document.Save();
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
            }

            // Open the Word file
            OpenWordFile(uniqueLocation);
        }

        private void AppendTitle(Body body)
        {
            Paragraph headingParagraph = new Paragraph();
            Run headingRun = new Run(new Text("Thane Bharat Sahakari Bank Ltd"));
            headingRun.PrependChild(new RunProperties(new FontSize() { Val = "40" }, new Bold()));

            headingParagraph.Append(new ParagraphProperties(new Justification() { Val = JustificationValues.Center }));
            headingParagraph.Append(headingRun);
            body.Append(headingParagraph);

            Paragraph subheadingParagraph = new Paragraph(new Run(new Text("(Scheduled Bank)")));
            subheadingParagraph.Append(new RunProperties(new FontSize() { Val = "18" }));
            body.Append(subheadingParagraph);
        }

        private void AppendHeaderTable(Body body, CreateWordDocumentPaymentNote? result, string? from, string? to)
        {
            Table tableHeader = new Table();
            SetTableProperties(tableHeader);

            // Header Rows
            tableHeader.Append(CreateHeaderRow($"From: {from}", $"To: {to}"));
            tableHeader.Append(CreateHeaderRow($"Sub: Payment To be made to M/S {result?.VendorName}", gridSpan: 2)); // Merging cells
            tableHeader.Append(CreateHeaderRow($"Ref: {result?.ServiceSantionedBy} dated {result?.SantionedDate}", gridSpan: 2)); // Merging cells
            tableHeader.Append(CreateHeaderRow($"Date: {result?.VendorPaymentDate}", $"Payment Note No.: {result?.PaymentNoteNo}"));

            body.AppendChild(tableHeader);
        }

        private TableRow CreateHeaderRow(string leftText, string? rightText = null, int gridSpan = 1)
        {
            TableRow row = new TableRow();

            if (gridSpan > 1)
            {
                TableCell mergedCell = new TableCell(new TableCellProperties(new GridSpan { Val = (int)gridSpan }));
                mergedCell.Append(new Paragraph(new Run(new Text(leftText)))); // Use leftText for merged cell
                row.Append(mergedCell);
            }
            else
            {
                row.Append(CreateTableCell(leftText));
            }

            if (rightText != null)
            {
                row.Append(CreateTableCell(rightText));
            }

            return row;
        }
        private TableRow CreateHeaderRow(string leftText, string? rightText = null)
        {
            TableRow row = new TableRow();
            row.Append(CreateTableCell(leftText, "3000"));

            if (rightText != null)
            {
                row.Append(CreateTableCell(rightText, "3000"));
            }

            return row;
        }

        private void AppendParagraph(Body body, string? text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                Paragraph paragraph = new Paragraph(new Run(new Text(text)));
                body.Append(paragraph);
            }
        }

        private void AppendInvoiceTable(Body body, List<CreateWordDocumentPaymentNote>? result)
        {
            Table tableInvoice = new Table();
            SetTableProperties(tableInvoice);

            // Header Row
            tableInvoice.Append(CreateInvoiceHeaderRow());

            int srNo = 1;
            decimal totalAmountPaid = 0;

            foreach (var invoice in result)
            {
                tableInvoice.Append(CreateInvoiceRow(srNo++, invoice));
                totalAmountPaid += invoice?.TotalAmountPaid ?? 0;
            }

            // Total Row
            tableInvoice.Append(CreateTotalRow(totalAmountPaid));
            body.Append(tableInvoice);
        }

        private TableRow CreateInvoiceHeaderRow()
        {
            return new TableRow(
                CreateTableCell("SrNo"),
                CreateTableCell("Invoice No"),
                CreateTableCell("Date"),
                CreateTableCell("Particular"),
                CreateTableCell("Sub Total"),
                CreateTableCell("CGST"),
                CreateTableCell("SGST"),
                CreateTableCell("IGST"),
                CreateTableCell("Amount")
            );
        }

        private TableRow CreateInvoiceRow(int srNo, CreateWordDocumentPaymentNote invoice)
        {
            return new TableRow(
                CreateTableCell(srNo.ToString()),
                CreateTableCell(invoice?.InvoiceNumber ?? ""),
                CreateTableCell(invoice?.InvoiceDate?.ToShortDateString() ?? ""),
                CreateTableCell(invoice?.InvoiceParticulars ?? ""),
                CreateTableCell(string.Format("{0:F2}", invoice?.VendorPaymentAmount)),
                CreateTableCell(string.Format("{0:F2}", invoice?.VendorPaymentCgst)),
                CreateTableCell(string.Format("{0:F2}", invoice?.VendorPaymentSgst)),
                CreateTableCell(string.Format("{0:F2}", invoice?.VendorPaymentIgst)),
                CreateTableCell(string.Format("{0:F2}", invoice?.TotalAmountPaid))
            );
        }

        private TableRow CreateTotalRow(decimal totalAmountPaid)
        {
            return new TableRow(
                CreateTableCell("Total", null, 8), // Merged cell
                CreateTableCell(string.Format("{0:F2}", totalAmountPaid))
            );
        }

        private void AppendFooterTable(Body body, List<CreateWordDocumentPaymentNote>? result)
        {
            Table tableFooter = new Table();
            SetTableProperties(tableFooter);

            // Footer Rows
            var first = result?.FirstOrDefault();
            decimal? totalAmountPaid = result?.Sum(x => x.TotalAmountPaid);

            tableFooter.Append(CreateFooterRow("UTR No:", first?.VendorPaymentUtrnumber, "Amount:", string.Format("{0:F2}", totalAmountPaid)));
            tableFooter.Append(CreateFooterRow("Date", first?.VendorPaymentRtgsDate?.ToString(), "TDS", string.Format("{0:F2}", first?.VendorPaymentTdsamount)));
            decimal? amtPaid = totalAmountPaid - (first?.VendorPaymentTdsamount ?? 0);
            tableFooter.Append(CreateFooterRow("", "", "Total Amount Paid", string.Format("{0:F2}", amtPaid)));

            body.AppendChild(tableFooter);
        }

        private TableRow CreateFooterRow(string? left1, string? left2, string right1, string right2)
        {
            return new TableRow(
                CreateTableCell(left1),
                CreateTableCell(left2),
                CreateTableCell(right1),
                CreateTableCell(right2)
            );
        }

        private TableCell CreateTableCell(string? text, string? width = null, int gridSpan = 1)
        {
            TableCell cell = new TableCell();
            TableCellProperties cellProperties = new TableCellProperties(new TableCellWidth { Width = width ?? "3000", Type = TableWidthUnitValues.Dxa });

            if (gridSpan > 1)
            {
                cellProperties.Append(new GridSpan { Val = gridSpan });
            }

            cell.Append(cellProperties);
            cell.Append(new Paragraph(new Run(new Text(text))));
            return cell;
        }

        private void SetTableProperties(Table table)
        {
            TableProperties tableProperties = new TableProperties(
                new TableWidth() { Width = "10000", Type = TableWidthUnitValues.Dxa },
                new TableBorders(
                    new TopBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                    new BottomBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                    new LeftBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                    new RightBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                    new InsideHorizontalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                    new InsideVerticalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 }
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
