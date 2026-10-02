using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Application.Settings;
using BusinessAppFramework.Contracts;

namespace BusinessAppFramework.Application.Registry
{
    public class UserSettingsRegistry : IUserSettingsRegistry
    {
        private readonly List<Type> _settingsTypes = new();

        public void Register<TSettings>() where TSettings : UserSettings, new()
        {
            if (!_settingsTypes.Contains(typeof(TSettings)))
                _settingsTypes.Add(typeof(TSettings));
        }

        public Type Resolve<TSettings>() where TSettings : UserSettings
        {
            return _settingsTypes.FirstOrDefault(type => typeof(TSettings).IsAssignableFrom(type))
                ?? throw new KeyNotFoundException($"No user settings registered for type '{typeof(TSettings).Name}'.");
        }

        public Type? Resolve(string key)
        {
            return _settingsTypes.FirstOrDefault(type => SettingsKeys.For(type) == key);
        }

        public IReadOnlyList<Type> GetAll() => _settingsTypes.AsReadOnly();
    }
}
