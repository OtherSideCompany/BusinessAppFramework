using BusinessAppFramework.Application.Settings;

namespace BusinessAppFramework.Application.Repository
{
    public interface IUserSettingsRepository
    {
        Task<UserSettings?> GetAsync(int userId, string key, Type settingsType);
        Task SaveAsync(int userId, string key, UserSettings settings);
    }
}
