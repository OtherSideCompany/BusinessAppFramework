using BusinessAppFramework.Application.Settings;

namespace BusinessAppFramework.Application.Interfaces
{
    public interface IModuleSettingsRegistry
    {
        void Register<TSettings>() where TSettings : ModuleSettings, new();
        Type Resolve<TSettings>() where TSettings : ModuleSettings;
        IReadOnlyList<Type> GetAll();
    }
}
