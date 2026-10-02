using BusinessAppFramework.Application.Settings;

namespace BusinessAppFramework.Application.Interfaces
{
    public interface IUserSettingsService
    {
        Task<TSettings> GetAsync<TSettings>() where TSettings : UserSettings;
        Task<UserSettings> GetAsync(string key);
        Task SaveAsync(string key, UserSettings settings);
    }
}
