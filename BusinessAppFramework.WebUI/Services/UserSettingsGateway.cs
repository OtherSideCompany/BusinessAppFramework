using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Application.Settings;
using BusinessAppFramework.Contracts;
using BusinessAppFramework.Contracts.ApiRoutes;
using BusinessAppFramework.WebUI.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace BusinessAppFramework.WebUI.Services
{
    public class UserSettingsGateway : HttpService, IUserSettingsGateway
    {
        #region Fields

        private const string _baseUrl = $"{ApiRouteSegments.Root}/{ApiRouteSegments.UserSettings}";

        private static readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web);

        #endregion

        #region Constructor

        public UserSettingsGateway(
            IHttpClientFactory clientFactory,
            IOptions<ApiClientOptions> apiClientOptions,
            ILogger<UserSettingsGateway> logger,
            ILocalizedStringService localizedStringService,
            IUserDialogService userDialogService)
            : base(clientFactory, apiClientOptions, logger, localizedStringService, userDialogService)
        {
        }

        #endregion

        #region Public Methods

        public async Task<TSettings> GetAsync<TSettings>() where TSettings : UserSettings, new()
        {
            var result = await GetAsync<JsonElement>($"{_baseUrl}/{SettingsKeys.For(typeof(TSettings))}");

            if (!result.Success || result.Data.ValueKind != JsonValueKind.Object)
                return new TSettings();

            return result.Data.Deserialize<TSettings>(_jsonOptions) ?? new TSettings();
        }

        public async Task<bool> SaveAsync<TSettings>(TSettings settings) where TSettings : UserSettings
        {
            return (await PutAsync($"{_baseUrl}/{SettingsKeys.For(settings.GetType())}", settings)).Success;
        }

        #endregion
    }
}
