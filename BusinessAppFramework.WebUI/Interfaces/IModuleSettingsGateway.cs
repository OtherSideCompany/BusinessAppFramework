using BusinessAppFramework.Application.Settings;

namespace BusinessAppFramework.WebUI.Interfaces
{
    public interface IModuleSettingsGateway
    {
        Task<List<(string Key, ModuleSettings Settings)>> GetAllAsync();
        Task<bool> SaveAsync(string key, ModuleSettings settings);
    }
}
