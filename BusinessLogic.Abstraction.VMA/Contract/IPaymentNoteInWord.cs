using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Abstraction.VMA.Contract
{
    public interface IPaymentNoteInWord
    {
        Task CreateAndOpenWordFileForNone(List<string> serviceName, string? from, string? to, string? bodyTextBefore, string? bodyTextAfter, string? financilaYear, string? path, string vendorName);
        Task CreateAndOpenWordFile(List<string> serviceName, string? from, string? to, string? bodyTextBefore, string? bodyTextAfter, string? financilaYear, string? path, string vendorName);
    }
}
