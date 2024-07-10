using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.VMA.Entities.CustomEntities
{
    public class ExportPaymentNoteData
    {
        public int SrNo { get; set; }
        public string? VendorName { get; set; }//Vendor_Name

        public string? VendorServiceName { get; set; }
        public string? VendorPaymentYearRange { get; set; }//Year
        public string? PaymentNoteNo { get; set; }//Payment_Note_number

        public DateTime? PaymentNoteDate { get; set; }//Payment_Note_Date

        public string? InvoiceNumber { get; set; }//Invoice_Number
        public DateTime? InvoiceDate { get; set; }//Invoice_Date

        public string? InvoiceParticulars { get; set; }//Invoice_Particular

        public string? VendorPaymentAmount { get; set; }//Total_Amount

        public string? VendorDetailCategory { get; set; }//Department

        public string? ServiceType { get; set; }//Type of expenditure

        public string? ServiceSantionAmount { get; set; }
        public string? ServiceSantionedBy { get; set; }//Sanctioned_by
        public decimal? VendorPaymentTdsamount { get; set; }//TDS_Amount
        public int? VendorPaymentUtrnumber { get; set; }//UTR_Number

        public decimal? VendorPaymentRtgsAmount { get; set; }//RTGS_Amount

        public DateOnly? VendorPaymentRtgsDate { get; set; }//RTGS_Date
    }
}
