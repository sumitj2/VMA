using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.Abstraction.VMA.Models;
using Database.Abstraction.VMA.Contract;
using Database.VMA.Entities;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
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

        public async Task<ConfigurationModel> GetConfigurationByKey(string cfgKey)
        {
            var configurationResult = await _configurationsRepository.GetConfigurationByKey(cfgKey);
            if (configurationResult != null)
            {
                ConfigurationModel configuration = new ConfigurationModel()
                {
                    Id = configurationResult.Id,
                    Cfgkey = configurationResult.Cfgkey,
                    CfgValue = configurationResult.Cfgvalue
                };
                return configuration;
            }
            return null;

        }

        public async Task<IEnumerable<ConfigurationModel>> GetConfigurations()
        {
            var configurationResult = await _configurationsRepository.GetAllConfigurations();

            List<ConfigurationModel> configurationlist = new List<ConfigurationModel>();

            foreach (var configuration in configurationResult)
            {
                configurationlist.Add(new ConfigurationModel()
                {
                    Id = configuration.Id,
                    Cfgkey = configuration.Cfgkey,
                    CfgValue = configuration.Cfgvalue
                });

            }

            return configurationlist;
        }

        public async Task AddConfiguration(ConfigurationModel configurationmodel)
        {
            Configuration configuration = new Configuration()
            {
                Cfgkey = configurationmodel.Cfgkey,
                Cfgvalue = configurationmodel.CfgValue
            };

            await _configurationsRepository.AddConfiguration(configuration);
        }

        public async Task UpdateOrDeleteConfiguration(ConfigurationModel configurationmodel)
        {
            Configuration configuration = new Configuration()
            {
                Id = configurationmodel.Id,
                Cfgkey = configurationmodel.Cfgkey,
                Cfgvalue = configurationmodel.CfgValue
            };

            await _configurationsRepository.UpdateDeleteConfigurations(configuration).ConfigureAwait(true);
        }
    }
}
