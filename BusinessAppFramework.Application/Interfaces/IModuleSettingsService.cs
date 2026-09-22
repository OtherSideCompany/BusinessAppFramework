using BusinessAppFramework.Application.Settings;

namespace BusinessAppFramework.Application.Interfaces
{
    public interface IModuleSettingsService
    {
        Task<TSettings> GetAsync<TSettings>() where TSettings : ModuleSettings;
        Task<ModuleSettings> GetAsync(string key);
        Task SaveAsync(string key, ModuleSettings settings);
    }
}
