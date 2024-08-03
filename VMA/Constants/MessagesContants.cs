using DocumentFormat.OpenXml.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMA.Constants
{
    public static class MessagesContants
    {
        public const string RequireVendorName = "*Vendor Name is Required";
        public const string InvalidEmail = "Invalid email address";
        public const string SuccessVendorUpdated = "Vendor Data Updated Successfully";
        public const string SuccessVendorAdded = "Vendor Data Added Successfully";

        public const string VendorDetailMsg = "Vendor Details already added for ";

        public const string ErrorMsgConfiguration = "Failed to load All configurations, Please contact to Administrator";

        public const string SuccessVendorDetailsUpdated = "Vendors Details Updated Successfully";
        public const string SuccessVendorDetailsAdded = "Vendors Details Added Successfully";

        public const string ErrorMessageVendorDetailsSave = "Failed to Save vendor service details, Please try again or contact to administrator";

        public const string PaymentNoteAlreadyGeneratedMsg = "Payment Note alreday genrated for ";
        public const string PaymentNoteDataUpdated = "Payment Note Data Updated Successfully";
        public const string PaymentNoteDataAdded = "Payment Note Data Added Successfully";
        public const string PaymentDataUpdate = "Payment Data Updated Successfully";
        public const string PaymentDataAdded="Payment Data Added Successfully";

        public const string PaymentSubmitErroMsg = "Failed to save Payment Details, Please contact to Administrator";
        public const string PaymentMsgSantionAmtHigh = "Total Amount cannot be greater than santioned amount";

        public const string ProductServiceUpdated = "Product Services Updated Successfully";
        public const string ProductServiceAdded = "Product Services Added Successfully";



    }
}
