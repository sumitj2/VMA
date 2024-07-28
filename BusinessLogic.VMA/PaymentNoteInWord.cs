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

                if (result != null)
                {                   
                    // Ensure the directory exists
                    Directory.CreateDirectory(Path.GetDirectoryName(path+"_"+serviceName+ "_PaymentNote.docx"));

                    // Create and save the Word document
                    using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(path, WordprocessingDocumentType.Document))
                    {
                        // Add a main document part
                        MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                        mainPart.Document = new Document();
                        Body body = new();

                        #region Title

                        // Add a paragraph with custom text
                        Paragraph paragraphBody1 = new();

                        // Create RunProperties and set the font size, bold, and other properties
                        RunProperties paragraphRunProperties1 = new();
                        FontSize paragraphFontSize1 = new() { Val = "28" }; // 14pt font
                        Bold paragraphBold1 = new(); // Bold text
                        Justification justify = new() { Val = JustificationValues.Left };
                        //Color paragraphColor1 = new() { Val = "0000FF" }; // Blue color
                        paragraphRunProperties1.Append(paragraphFontSize1);
                        paragraphRunProperties1.Append(paragraphBold1);
                        paragraphRunProperties1.Append(justify);
                        //paragraphRunProperties1.Append(paragraphColor1);

                        // Create a Run and apply the RunProperties
                        Run titleRun1 = new Run(new Text("Thane Bharat Sahakari Bank Ltd"));
                        titleRun1.PrependChild(paragraphRunProperties1);

                        // Add the Run to the Paragraph
                        paragraphBody1.Append(titleRun1);

                        // Add the Paragraph to the Body
                        body.Append(paragraphBody1);
                        // Add a title
                        Paragraph titleParagraph = new(new Run(new Text("(Scheduled Bank)")));
                        titleParagraph.ParagraphProperties = new ParagraphProperties(new Justification() { Val = JustificationValues.Right });
                        titleParagraph.ParagraphProperties = new ParagraphProperties(new FontSize() { Val = "20" });
                        titleParagraph.ParagraphProperties = new ParagraphProperties(new Bold());

                        body.AppendChild(titleParagraph);

                        #endregion

                        #region Header Table

                        // Add a table
                        Table tableHeader = new Table();

                        // Set the table properties
                        TableProperties tableheaderProperties = new TableProperties(
                            new TableBorders(
                                new TopBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                                new BottomBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                                new LeftBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                                new RightBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                                new InsideHorizontalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                                new InsideVerticalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 }
                            )
                        );
                        tableHeader.AppendChild(tableheaderProperties);

                        // Add a couple of rows
                        TableRow rowHeader = new TableRow();
                        rowHeader.Append
                            (
                            new TableCell(new Paragraph(new Run(new Text("From: " + from)))),
                            new TableCell(new Paragraph(new Run(new Text("To: " + to))))
                            );
                        tableHeader.Append(rowHeader);
                        //subject 
                        TableRow rowHeader2 = new TableRow();
                        rowHeader2.Append
                            (
                            new TableCell(new Paragraph(new Run(new Text("Sub : Payment to be made to M/S. " + result?.FirstOrDefault()?.VendorName + " " + result?.FirstOrDefault()?.VendorServiceName)))),
                            new TableCell(new Paragraph(new Run(new Text(" "))))
                            );
                        tableHeader.Append(rowHeader2);

                        TableRow rowHeader3 = new TableRow();
                        rowHeader3.Append
                            (
                            new TableCell(new Paragraph(new Run(new Text("Ref :" + result?.FirstOrDefault()?.ServiceSantionedBy + " " + result?.FirstOrDefault()?.SantionedDate)))),
                            new TableCell(new Paragraph(new Run(new Text(" "))))
                            );
                        tableHeader.Append(rowHeader3);

                        TableRow rowHeader4 = new TableRow();
                        rowHeader4.Append(
                            new TableCell(new Paragraph(new Run(new Text("Date :03rd June 2024 ")))),//Row 2, Cell 1
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
                        //FontSize paragraphFontSize = new() { Val = "28" }; // 14pt font
                        //Bold paragraphBold = new(); // Bold text
                        //Color paragraphColor = new() { Val = "0000FF" }; // Blue color
                        //paragraphRunProperties.Append(paragraphFontSize);
                        //paragraphRunProperties.Append(paragraphBold);
                        //paragraphRunProperties.Append(paragraphColor);

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

                        // Set the table properties
                        TableProperties tableInvocieProperties = new TableProperties(
                            new TableBorders(
                                new TopBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                                new BottomBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                                new LeftBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                                new RightBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                                new InsideHorizontalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                                new InsideVerticalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 }
                            )
                        );
                        tableInvocie.AppendChild(tableInvocieProperties);

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
                        //FontSize paragraphFontSizeAfter = new() { Val = "28" }; // 14pt font
                        //Bold paragraphBoldAfter= new(); // Bold text
                        //Color paragraphColorAfter = new() { Val = "0000FF" }; // Blue color
                        //paragraphRunPropertiesAfter.Append(paragraphFontSizeAfter);
                        //paragraphRunPropertiesAfter.Append(paragraphBoldAfter);
                        //paragraphRunPropertiesAfter.Append(paragraphColorAfter);

                        // Create a Run and apply the RunProperties
                        Run paragraphRunAfter = new Run(new Text("This is a custom paragraph with bold, 14pt blue text." + bodyTextAfter));
                        paragraphRun.PrependChild(paragraphRunPropertiesAfter);

                        // Add the Run to the Paragraph
                        paragraphBodyAfter.Append(paragraphRunAfter);

                        // Add the Paragraph to the Body
                        body.Append(paragraphBodyAfter);

                        #endregion

                        #region Footer Table
                        // Add a table
                        Table tableFooter = new Table();

                        // Set the table properties
                        TableProperties tableFooterProperties = new TableProperties(
                            new TableBorders(
                                new TopBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                                new BottomBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                                new LeftBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                                new RightBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                                new InsideHorizontalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                                new InsideVerticalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 }
                            )
                        );
                        tableFooter.AppendChild(tableFooterProperties);

                        // Add a couple of rows
                        TableRow rowFooter1 = new TableRow();
                        rowFooter1.Append
                            (
                            new TableCell(new Paragraph(new Run(new Text("UTR No: ")))),
                            new TableCell(new Paragraph(new Run(new Text(result?.FirstOrDefault().VendorPaymentUtrnumber)))),
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
                    OpenWordFile(path);
                }
            }
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
                Debug.WriteLine($"Error opening the file: {ex.Message}");
            }
        }
    }

}
