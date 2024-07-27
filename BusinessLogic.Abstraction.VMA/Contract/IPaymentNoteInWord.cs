using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Abstraction.VMA.Contract
{
    public interface IPaymentNoteInWord
    {
        Task CreateAndOpenWordFile(string serviceName, string from, string to, string bodyTextBefore, string bodyTextAfter);
    }
}
