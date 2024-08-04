using BusinessLogic.Abstraction.VMA.Models;

namespace BusinessLogic.Abstraction.VMA.Contract
{
    public interface IConfigurationBusinessLogic
    {
        public Task AddConfiguration(ConfigurationModel configuration);

        public Task<ConfigurationModel> GetConfigurationByKey(string cfgKey);

        public Task UpdateOrDeleteConfiguration(ConfigurationModel configuration);

        public Task<IEnumerable<ConfigurationModel>> GetConfigurations();

    }
}
