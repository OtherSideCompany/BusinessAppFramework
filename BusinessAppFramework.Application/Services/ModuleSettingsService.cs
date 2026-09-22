using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Application.Repository;
using BusinessAppFramework.Application.Settings;
using BusinessAppFramework.Contracts;

namespace BusinessAppFramework.Application.Services
{
    public class ModuleSettingsService : IModuleSettingsService
    {
        private readonly IModuleSettingsRegistry _registry;
        private readonly IModuleSettingsRepository _repository;

        public ModuleSettingsService(
            IModuleSettingsRegistry registry,
            IModuleSettingsRepository repository)
        {
            _registry = registry;
            _repository = repository;
        }

        public async Task<TSettings> GetAsync<TSettings>() where TSettings : ModuleSettings
        {
            return (TSettings)await GetAsync(_registry.Resolve<TSettings>());
        }

        public Task<ModuleSettings> GetAsync(string key)
        {
            return GetAsync(ResolveType(key));
        }

        public Task SaveAsync(string key, ModuleSettings settings)
        {
            var settingsType = ResolveType(key);

            if (!settingsType.IsInstanceOfType(settings))
                throw new ArgumentException($"Settings '{key}' expect an instance of {settingsType.Name}.", nameof(settings));

            return _repository.SaveAsync(key, settings);
        }

        private async Task<ModuleSettings> GetAsync(Type settingsType)
        {
            return await _repository.GetAsync(SettingsKeys.For(settingsType), settingsType)
                ?? (ModuleSettings)Activator.CreateInstance(settingsType)!;
        }

        private Type ResolveType(string key)
        {
            return _registry.GetAll().FirstOrDefault(type => SettingsKeys.For(type) == key)
                ?? throw new KeyNotFoundException($"No settings registered for key '{key}'.");
        }
    }
}
