using BusinessAppFramework.Application.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using UserSettings = BusinessAppFramework.Application.Settings.UserSettings;

namespace BusinessAppFramework.Infrastructure.Repositories
{
    public class UserSettingsRepository : IUserSettingsRepository
    {
        #region Fields

        private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

        private readonly IDbContextFactory<DbContext> _dbContextFactory;

        #endregion

        #region Constructor

        public UserSettingsRepository(IDbContextFactory<DbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        #endregion

        #region Public Methods

        public async Task<UserSettings?> GetAsync(int userId, string key, Type settingsType)
        {
            using var context = _dbContextFactory.CreateDbContext();

            var entity = await context.Set<Entities.UserSettings>()
                                      .AsNoTracking()
                                      .FirstOrDefaultAsync(e => e.UserId == userId && e.Key == key);

            return entity == null ? null : (UserSettings?)JsonSerializer.Deserialize(entity.Values, settingsType, _jsonOptions);
        }

        public async Task SaveAsync(int userId, string key, UserSettings settings)
        {
            using var context = _dbContextFactory.CreateDbContext();

            var entity = await context.Set<Entities.UserSettings>().FirstOrDefaultAsync(e => e.UserId == userId && e.Key == key);

            if (entity == null)
            {
                entity = new Entities.UserSettings { UserId = userId, Key = key };
                context.Add(entity);
            }

            entity.Values = JsonSerializer.Serialize(settings, settings.GetType(), _jsonOptions);

            await context.SaveChangesAsync();
        }

        #endregion
    }
}
