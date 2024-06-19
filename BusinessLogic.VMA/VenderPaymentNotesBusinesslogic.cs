using BusinessLogic.Abstraction.VMA.Contract;
using Database.Abstraction.VMA.Contract;
using Database.VMA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.VMA.Repositories
{
    public class VenderPaymentNotesBusinesslogic : IVenderPaymentNotesBusinessLogic
    {
        private IVenderPaymentNotesRepository _venderPaymentNotesRepository;
        public VenderPaymentNotesBusinesslogic(IVenderPaymentNotesRepository venderPaymentNotesRepository)
        {
            _venderPaymentNotesRepository = venderPaymentNotesRepository;
        }
    }
}
