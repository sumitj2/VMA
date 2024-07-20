using BusinessLogic.Abstraction.VMA.Contract;
using Database.Abstraction.VMA.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.VMA
{
    public class ConfigurationBusinessLogic : IConfigurationBusinessLogic
    {
        public IConfigurationsRepository _configurationsRepository;
        public ConfigurationBusinessLogic(IConfigurationsRepository configurationsRepository)
        {
            _configurationsRepository = configurationsRepository;
        }
    }
}
