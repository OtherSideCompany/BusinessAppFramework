using BusinessAppFramework.Application.Settings;

namespace BusinessAppFramework.Application.Interfaces
{
    public interface IUserSettingsRegistry
    {
        void Register<TSettings>() where TSettings : UserSettings, new();
        Type Resolve<TSettings>() where TSettings : UserSettings;
        Type? Resolve(string key);
        IReadOnlyList<Type> GetAll();
    }
}
