using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Application.Settings;

namespace BusinessAppFramework.Application.Registry
{
    public class ModuleSettingsRegistry : IModuleSettingsRegistry
    {
        private readonly List<Type> _settingsTypes = new();

        public void Register<TSettings>() where TSettings : ModuleSettings, new()
        {
            if (!_settingsTypes.Contains(typeof(TSettings)))
                _settingsTypes.Add(typeof(TSettings));
        }

        public Type Resolve<TSettings>() where TSettings : ModuleSettings
        {
            return _settingsTypes.FirstOrDefault(type => typeof(TSettings).IsAssignableFrom(type))
                ?? throw new KeyNotFoundException($"No settings registered for type '{typeof(TSettings).Name}'.");
        }

        public IReadOnlyList<Type> GetAll() => _settingsTypes.AsReadOnly();
    }
}
