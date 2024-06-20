using BusinessLogic.Abstraction.VMA.Contract;
using Database.Abstraction.VMA.Contract;

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
