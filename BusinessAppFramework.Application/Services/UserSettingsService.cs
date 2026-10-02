using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Application.Repository;
using BusinessAppFramework.Application.Settings;
using BusinessAppFramework.Contracts;

namespace BusinessAppFramework.Application.Services
{
    public class UserSettingsService : IUserSettingsService
    {
        private readonly IUserSettingsRegistry _registry;
        private readonly IUserSettingsRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public UserSettingsService(
            IUserSettingsRegistry registry,
            IUserSettingsRepository repository,
            ICurrentUserService currentUserService)
        {
            _registry = registry;
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<TSettings> GetAsync<TSettings>() where TSettings : UserSettings
        {
            return (TSettings)await GetAsync(_registry.Resolve<TSettings>());
        }

        public Task<UserSettings> GetAsync(string key)
        {
            return GetAsync(ResolveType(key));
        }

        public Task SaveAsync(string key, UserSettings settings)
        {
            var settingsType = ResolveType(key);

            if (!settingsType.IsInstanceOfType(settings))
                throw new ArgumentException($"User settings '{key}' expect an instance of {settingsType.Name}.", nameof(settings));

            return _repository.SaveAsync(GetCurrentUserId(), key, settings);
        }

        private async Task<UserSettings> GetAsync(Type settingsType)
        {
            return await _repository.GetAsync(GetCurrentUserId(), SettingsKeys.For(settingsType), settingsType)
                ?? (UserSettings)Activator.CreateInstance(settingsType)!;
        }

        private Type ResolveType(string key)
        {
            return _registry.Resolve(key)
                ?? throw new KeyNotFoundException($"No user settings registered for key '{key}'.");
        }

        private int GetCurrentUserId()
        {
            return _currentUserService.UserId
                ?? throw new UnauthorizedAccessException("User settings require an authenticated user.");
        }
    }
}
