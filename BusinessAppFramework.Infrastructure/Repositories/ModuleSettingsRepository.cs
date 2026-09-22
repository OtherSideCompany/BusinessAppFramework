using BusinessAppFramework.Application.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ModuleSettings = BusinessAppFramework.Application.Settings.ModuleSettings;

namespace BusinessAppFramework.Infrastructure.Repositories
{
    public class ModuleSettingsRepository : IModuleSettingsRepository
    {
        #region Fields

        private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

        private readonly IDbContextFactory<DbContext> _dbContextFactory;

        #endregion

        #region Constructor

        public ModuleSettingsRepository(IDbContextFactory<DbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        #endregion

        #region Public Methods

        public async Task<ModuleSettings?> GetAsync(string key, Type settingsType)
        {
            using var context = _dbContextFactory.CreateDbContext();

            var entity = await context.Set<Entities.ModuleSettings>()
                                      .AsNoTracking()
                                      .FirstOrDefaultAsync(e => e.Key == key);

            // Missing properties keep the defaults of the settings class, unknown ones are ignored.
            return entity == null
                ? null
                : (ModuleSettings?)JsonSerializer.Deserialize(entity.Values, settingsType, _jsonOptions);
        }

        public async Task SaveAsync(string key, ModuleSettings settings)
        {
            using var context = _dbContextFactory.CreateDbContext();

            var entity = await context.Set<Entities.ModuleSettings>().FirstOrDefaultAsync(e => e.Key == key);

            if (entity == null)
            {
                entity = new Entities.ModuleSettings { Key = key };
                context.Add(entity);
            }

            entity.Values = JsonSerializer.Serialize(settings, settings.GetType(), _jsonOptions);

            await context.SaveChangesAsync();
        }

        #endregion
    }
}
