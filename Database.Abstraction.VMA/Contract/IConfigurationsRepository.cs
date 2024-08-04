using Database.VMA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Abstraction.VMA.Contract
{
    public interface IConfigurationsRepository
    {
        public Task AddConfiguration(Configuration ConfigurationEntity);
        public Task UpdateDeleteConfigurations(Configuration ConfigurationEntity);
        public Task<IEnumerable<Configuration>> GetAllConfigurations();
        public Task<Configuration?> GetConfigurationByKey(string CFGKey);
    }
}
