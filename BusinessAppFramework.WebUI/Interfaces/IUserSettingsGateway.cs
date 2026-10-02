using BusinessAppFramework.Application.Settings;

namespace BusinessAppFramework.WebUI.Interfaces
{
    public interface IUserSettingsGateway
    {
        Task<TSettings> GetAsync<TSettings>() where TSettings : UserSettings, new();
        Task<bool> SaveAsync<TSettings>(TSettings settings) where TSettings : UserSettings;
    }
}
