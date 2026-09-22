using BusinessAppFramework.Application.Settings;

namespace BusinessAppFramework.Application.Repository
{
    public interface IModuleSettingsRepository
    {
        Task<ModuleSettings?> GetAsync(string key, Type settingsType);
        Task SaveAsync(string key, ModuleSettings settings);
    }
}
