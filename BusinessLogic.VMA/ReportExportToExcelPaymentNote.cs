using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.Abstraction.VMA.Models;
using ClosedXML.Excel;
using Database.Abstraction.VMA.Contract;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BusinessLogic.VMA
{
    public class ReportExportToExcelPaymentNote : IReportExportToExcelPaymentNote
    {
        private readonly IVenderPaymentNotesRepository _venderPaymentNotesRepository;
        public ReportExportToExcelPaymentNote(IVenderPaymentNotesRepository venderPaymentNotesRepository)
        {
            _venderPaymentNotesRepository = venderPaymentNotesRepository;
        }

        public async Task ExportPaymentNotes()
        {
            List<ExportPaymentNoteData> exportData = new List<ExportPaymentNoteData>();
            var res = await _venderPaymentNotesRepository.GetAllPaymentDetailsWithServiceDetailsToExport().ConfigureAwait(true);
            foreach (var item in res)
            {
                exportData.Add(new ExportPaymentNoteData
                {
                    InvoiceDate = item.InvoiceDate,
                    InvoiceNumber = item.InvoiceNumber,
                    InvoiceParticulars = item.InvoiceParticulars,
                    PaymentNoteDate = item.PaymentNoteDate,
                    PaymentNoteNo = item.PaymentNoteNo,
                    ServiceSantionAmount = item.ServiceSantionAmount,
                    VendorDetailCategory = item.VendorDetailCategory,
                    VendorServiceName = item.VendorServiceName,
                    VendorPaymentYearRange = item.VendorPaymentYearRange,
                    VendorName = item.VendorName,
                    ServiceType = item.ServiceType,
                    VendorPaymentUtrnumber = item.VendorPaymentUtrnumber,
                    ServiceSantionedBy = item.ServiceSantionedBy,
                    SrNo = item.SrNo,
                    VendorPaymentAmount = item.VendorPaymentAmount,
                    VendorPaymentRtgsAmount = item.VendorPaymentRtgsAmount,
                    VendorPaymentRtgsDate = item.VendorPaymentRtgsDate,
                    VendorPaymentTdsamount = item.VendorPaymentTdsamount
                });
            }
            var fileContent = ExportToExcel(exportData);

            // Save the file to disk
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Excel Files|*.xlsx",
                Title = "PaymentNotes",
                FileName = "output.xlsx"
            };


            File.WriteAllBytes(saveFileDialog.FileName, fileContent);
            MessageBox.Show($"File successfully saved to {saveFileDialog.FileName}");

        }

        private byte[] ExportToExcel(List<ExportPaymentNoteData> data)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Sheet1");
                //		  								AMC	"Type of 
                //Expenditure"	Sanctioned	Sanctioned_by	Sanctioned_Date		RTGS_Amount		

                // Add headers
                worksheet.Cell(1, 1).Value = "Year";
                worksheet.Cell(1, 2).Value = "Sr_No";
                worksheet.Cell(1, 3).Value = "Payment_Note_number";
                worksheet.Cell(1, 4).Value = "Payment_Note_Date";
                worksheet.Cell(1, 5).Value = "Vendor_Name";
                worksheet.Cell(1, 6).Value = "Service_Name";
                worksheet.Cell(1, 7).Value = "Invoice_Number";
                worksheet.Cell(1, 8).Value = "Invoice_Date";
                worksheet.Cell(1, 9).Value = "Invoice_Particular";
                worksheet.Cell(1, 10).Value = "Total_Amount";
                worksheet.Cell(1, 11).Value = "Department";
                worksheet.Cell(1, 12).Value = "AMC";
                worksheet.Cell(1, 13).Value = "Type_Of_Expenditure";
                worksheet.Cell(1, 14).Value = "Sanctioned_by";
                worksheet.Cell(1, 15).Value = "TDS_Amount";
                worksheet.Cell(1, 16).Value = "RTGS_Amount";
                worksheet.Cell(1, 17).Value = "UTR_Number";
                worksheet.Cell(1, 18).Value = "RTGS_Date";

                // Add data
                for (int i = 0; i < data.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = data[i].VendorPaymentYearRange;
                    worksheet.Cell(i + 2, 2).Value = i;
                    worksheet.Cell(i + 2, 3).Value = data[i].PaymentNoteNo;
                    worksheet.Cell(i + 2, 4).Value = data[i].PaymentNoteDate;
                    worksheet.Cell(i + 2, 5).Value = data[i].VendorName;
                    worksheet.Cell(i + 2, 6).Value = data[i].VendorServiceName;
                    worksheet.Cell(i + 2, 7).Value = data[i].InvoiceNumber;
                    worksheet.Cell(i + 2, 8).Value = data[i].InvoiceDate;
                                  
                    worksheet.Cell(i + 2, 9).Value = data[i].InvoiceParticulars;
                    worksheet.Cell(i + 2, 10).Value = data[i].VendorPaymentAmount;
                    worksheet.Cell(i + 2, 11).Value = data[i].VendorDetailCategory;
                    worksheet.Cell(i + 2, 12).Value = "";//AMC
                                  
                                  
                    worksheet.Cell(i + 2, 13).Value = data[i].ServiceType;
                    worksheet.Cell(i + 2, 14).Value = data[i].ServiceSantionedBy;
                    worksheet.Cell(i + 2, 15).Value = data[i].VendorPaymentTdsamount;
                    worksheet.Cell(i + 2, 16).Value = data[i].VendorPaymentRtgsAmount;
                    worksheet.Cell(i + 2, 17).Value = data[i].VendorPaymentUtrnumber;
                    worksheet.Cell(i + 2, 18).Value = data[i].VendorPaymentRtgsDate.ToString();


                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }
    }
}
