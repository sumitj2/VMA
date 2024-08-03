using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.Abstraction.VMA.Models;
using ClosedXML.Excel;
using Database.Abstraction.VMA.Contract;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VMA.Constants;

namespace BusinessLogic.VMA
{

    public class ReportExportToExcelPaymentNote : IReportExportToExcelPaymentNote
    {

        private readonly IVenderPaymentNotesRepository _venderPaymentNotesRepository;
        public ReportExportToExcelPaymentNote(IVenderPaymentNotesRepository venderPaymentNotesRepository)
        {
            _venderPaymentNotesRepository = venderPaymentNotesRepository;
        }

        public async Task ExportPaymentNotes(string? financialYear,string? path)
        {
            List<ExportPaymentNoteData> exportData = [];
            var payments = await _venderPaymentNotesRepository.GetAllPaymentDetailsWithServiceDetailsToExport(financialYear).ConfigureAwait(true);
            if(payments?.Count == 0) 
            {
                MessageBox.Show(MessagesContants.ReportExcelNoPaymentFoud + financialYear);
            }
            else
            {
                var orderByPayments = payments?.OrderBy(x => x.PaymentNoteNo).ThenBy(x => x.VendorPaymentDate);
                foreach (var item in orderByPayments)
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
                        VendorPaymentTdsamount = item.VendorPaymentTdsamount,
                        VendorPaymentDate = item.VendorPaymentDate,
                        IsAmc = item.IsAmc
                    });
                }
                var fileContent = ExportToExcel(exportData);

                // Save the file to disk
                var saveFileDialog = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "Excel Files|*.xlsx",
                    Title = "PaymentNotes",
                    FileName = "AMC_Payment_Details_" + DateTime.UtcNow.ToString("dd_t") + ".xlsx"
                };

                string location = path + "\\" + saveFileDialog.FileName;
                
                File.WriteAllBytes(location, fileContent);

                MessageBox.Show($"{MessagesContants.FileSavedToSucesfully} {saveFileDialog.FileName}");

                OpenExcelFile(location);
            }

        }

        private byte[] ExportToExcel(List<ExportPaymentNoteData> data)
        {
            var groupServiceNameList = data.GroupBy(x => x.VendorServiceName);

            using var workbook = new XLWorkbook();
            int counter = 0;
            int srNo = 1;

            var worksheet = workbook.Worksheets.Add("AMC Chart");
            worksheet.Cell(1, 1).Value = "Sr_No";
            worksheet.Cell(1, 2).Value = "Financial Year";
            worksheet.Cell(1, 3).Value = "Payment_Note_number";
            worksheet.Cell(1, 4).Value = "Payment_Note_Date";
            worksheet.Cell(1, 5).Value = "Vendor_Name";
            worksheet.Cell(1, 6).Value = "Service_Name";
            worksheet.Cell(1, 7).Value = "Santioned Amount";
            worksheet.Cell(1, 8).Value = "Amount Due";
            worksheet.Cell(1, 9).Value = "Period";
            worksheet.Cell(1, 10).Value = "Period Amount Paid";
            worksheet.Cell(1, 11).Value = "Payment Date";
            worksheet.Cell(1, 12).Value = "Total Amount Paid Till Now";
            worksheet.Cell(1, 13).Value = "Invoice_Number";
            worksheet.Cell(1, 14).Value = "Invoice_Date";
            worksheet.Cell(1, 15).Value = "Invoice_Particular";
            worksheet.Cell(1, 16).Value = "Department";
            worksheet.Cell(1, 17).Value = "AMC";
            worksheet.Cell(1, 18).Value = "Type_Of_Expenditure";
            worksheet.Cell(1, 19).Value = "Sanctioned_by";
            worksheet.Cell(1, 20).Value = "TDS_Amount";
            worksheet.Cell(1, 21).Value = "RTGS_Amount";
            worksheet.Cell(1, 22).Value = "UTR_Number";
            worksheet.Cell(1, 23).Value = "RTGS_Date";

            foreach (var service in groupServiceNameList.ToList())
            {
                // Add Venor Name + Service Name
                worksheet.Cell(counter + 2, 1).Value = service?.FirstOrDefault()?.VendorName + " " + service?.Key;
                counter++;

                for (int i = 0; i < service?.ToList().Count; i++)
                {
                    worksheet.Cell(counter + 2, 1).Value = srNo;
                    worksheet.Cell(counter + 2, 2).Value = service.ToList()[i].VendorPaymentYearRange;
                    worksheet.Cell(counter + 2, 3).Value = service.ToList()[i].PaymentNoteNo;
                    worksheet.Cell(counter + 2, 4).Value = service.ToList()[i].PaymentNoteDate;
                    worksheet.Cell(counter + 2, 5).Value = service.ToList()[i].VendorName;
                    worksheet.Cell(counter + 2, 6).Value = service.ToList()[i].VendorServiceName;
                    worksheet.Cell(counter + 2, 7).Value = service.ToList()[i].ServiceSantionAmount;
                    if (i == 0)
                    {
                        worksheet.Cell(counter + 2, 8).Value = service.ToList()[i].ServiceSantionAmount - service.ToList()[i].VendorPaymentAmount;//amt due
                    }
                    else
                    {
                        worksheet.Cell(counter + 2, 8).Value = (decimal)worksheet.Cell((counter + 2) - 1, 8).Value.GetNumber() - service.ToList()[i].VendorPaymentAmount;
                    }

                    worksheet.Cell(counter + 2, 9).Value = (i + 1) + "Period Amout";//Period
                    worksheet.Cell(counter + 2, 10).Value = service.ToList()[i].VendorPaymentAmount;//"Period Amount Paid"
                    worksheet.Cell(counter + 2, 11).Value = service.ToList()[i].VendorPaymentDate.ToString();

                    if (i == 0)
                    {
                        worksheet.Cell(counter + 2, 12).Value = service.ToList()[i].VendorPaymentAmount; ;//totla amt paid tilll no                           
                    }
                    else
                    {
                        worksheet.Cell(counter + 2, 12).Value = (decimal)worksheet.Cell((counter + 2) - 1, 12).Value.GetNumber() + service.ToList()[i].VendorPaymentAmount;//totla amt paid tilll now
                    }
                    worksheet.Cell(counter + 2, 13).Value = service.ToList()[i].InvoiceNumber;
                    worksheet.Cell(counter + 2, 14).Value = service.ToList()[i].InvoiceDate;
                    worksheet.Cell(counter + 2, 15).Value = service.ToList()[i].InvoiceParticulars;
                    worksheet.Cell(counter + 2, 16).Value = service.ToList()[i].VendorDetailCategory;
                    if (service?.ToList()[i].IsAmc == true)
                    {
                        worksheet.Cell(counter + 2, 17).Value = "Yes";
                    }
                    else
                    {
                        worksheet.Cell(counter + 2, 17).Value = "No";
                    }

                    worksheet.Cell(counter + 2, 18).Value = service.ToList()[i].ServiceType;
                    worksheet.Cell(counter + 2, 19).Value = service.ToList()[i].ServiceSantionedBy;
                    worksheet.Cell(counter + 2, 20).Value = service.ToList()[i].VendorPaymentTdsamount;
                    worksheet.Cell(counter + 2, 21).Value = service.ToList()[i].VendorPaymentRtgsAmount;
                    worksheet.Cell(counter + 2, 22).Value = service.ToList()[i].VendorPaymentUtrnumber;
                    worksheet.Cell(counter + 2, 23).Value = service.ToList()[i].VendorPaymentRtgsDate.ToString();
                    counter++;
                    srNo++;
                }
            }
            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                //OpenExcelFile()
                return stream.ToArray();
            }
        }
        public void OpenExcelFile(string filePath)
        {
            try
            {
                Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Debug.WriteLine($"{MessagesContants.ErrorOpeningFile} {ex.Message}");
            }
        }
    }
}
