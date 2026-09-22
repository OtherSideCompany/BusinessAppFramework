using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Application.Settings;
using BusinessAppFramework.Contracts;
using BusinessAppFramework.Contracts.ApiRoutes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BusinessAppFramework.Adapter.Controllers
{
    [ApiController]
    [Authorize]
    public class ModuleSettingsController : ControllerBase
    {
        private static readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web);

        private readonly IModuleSettingsService _moduleSettingsService;
        private readonly IModuleSettingsRegistry _moduleSettingsRegistry;

        public ModuleSettingsController(
            IModuleSettingsService moduleSettingsService,
            IModuleSettingsRegistry moduleSettingsRegistry)
        {
            _moduleSettingsService = moduleSettingsService;
            _moduleSettingsRegistry = moduleSettingsRegistry;
        }

        [HttpGet($"{{{ApiRouteParams.Key}}}")]
        public async Task<ActionResult> GetAsync(
            [FromRoute(Name = ApiRouteParams.Key)] string key)
        {
            return Ok(await _moduleSettingsService.GetAsync(key));
        }

        [HttpPut($"{{{ApiRouteParams.Key}}}")]
        public async Task<ActionResult> SaveAsync(
            [FromRoute(Name = ApiRouteParams.Key)] string key,
            [FromBody] JsonElement settingsElement)
        {
            var settingsType = _moduleSettingsRegistry.GetAll().FirstOrDefault(type => SettingsKeys.For(type) == key);

            if (settingsType == null)
                return NotFound();

            var settings = (ModuleSettings?)settingsElement.Deserialize(settingsType, _jsonOptions);

            if (settings == null)
                return BadRequest();

            await _moduleSettingsService.SaveAsync(key, settings);

            return Ok();
        }
    }
}
