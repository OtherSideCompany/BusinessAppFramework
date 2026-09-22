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
    public class ModuleSettingsGateway : HttpService, IModuleSettingsGateway
    {
        #region Fields

        private const string _baseUrl = $"{ApiRouteSegments.Root}/{ApiRouteSegments.Settings}";

        private static readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web);

        private readonly IModuleSettingsRegistry _moduleSettingsRegistry;

        #endregion

        #region Constructor

        public ModuleSettingsGateway(
            IHttpClientFactory clientFactory,
            IOptions<ApiClientOptions> apiClientOptions,
            ILogger<ModuleSettingsGateway> logger,
            ILocalizedStringService localizedStringService,
            IUserDialogService userDialogService,
            IModuleSettingsRegistry moduleSettingsRegistry)
            : base(clientFactory, apiClientOptions, logger, localizedStringService, userDialogService)
        {
            _moduleSettingsRegistry = moduleSettingsRegistry;
        }

        #endregion

        #region Public Methods

        public async Task<List<(string Key, ModuleSettings Settings)>> GetAllAsync()
        {
            var result = new List<(string, ModuleSettings)>();

            foreach (var settingsType in _moduleSettingsRegistry.GetAll())
            {
                var key = SettingsKeys.For(settingsType);
                var element = (await GetAsync<JsonElement>($"{_baseUrl}/{key}")).Data;
                var settings = (ModuleSettings?)element.Deserialize(settingsType, _jsonOptions);

                if (settings != null)
                    result.Add((key, settings));
            }

            return result;
        }

        public async Task<bool> SaveAsync(string key, ModuleSettings settings)
        {
            return (await PutAsync($"{_baseUrl}/{key}", settings)).Success;
        }

        #endregion
    }
}
