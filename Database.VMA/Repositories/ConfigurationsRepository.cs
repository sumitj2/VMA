using Database.Abstraction.VMA.Contract;
using Database.VMA.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.VMA.Repositories
{
    public class ConfigurationsRepository : IConfigurationsRepository
    {
        private readonly VendorManagementDbContext _context;
        public ConfigurationsRepository(VendorManagementDbContext context)
        {
            _context = context;
        }
        public async Task AddConfiguration(Configuration ConfigurationEntity)
        {
            await _context.AddAsync(ConfigurationEntity).ConfigureAwait(true);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateDeleteConfigurations(Configuration ConfigurationEntity)
        {
            var existingEntity = _context.Configurations.Find(ConfigurationEntity.Id);
            if (existingEntity == null)
            {
                _context.Attach(ConfigurationEntity);
            }
            else
            {
                _context.Entry(existingEntity).CurrentValues.SetValues(ConfigurationEntity);
            }
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Configuration>> GetAllConfigurations()
        {
            return await _context.Configurations.ToListAsync();
        }
        public async Task<Configuration?> GetConfigurationByKey(string CFGKey)
        {
            return await _context.Configurations.Where(x => x.Cfgkey == CFGKey).FirstOrDefaultAsync();
        }
    }
}
