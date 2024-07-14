using BusinessLogic.Abstraction.VMA.Contract;
using Database.VMA.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMA.MVVM.ViewModels.Add
{
    public class GSTViewModel: ViewModelBase
    {
        private readonly IGstcalculationMasterBusinessLogic _gstcalculationMasterBusinessLogic;
        public GSTViewModel(IGstcalculationMasterBusinessLogic gstcalculationMasterBusinessLogic)
        {
            _gstcalculationMasterBusinessLogic = gstcalculationMasterBusinessLogic;
        }
    }
}
