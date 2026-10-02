using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Application.Settings;
using BusinessAppFramework.Contracts.ApiRoutes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BusinessAppFramework.Adapter.Controllers
{
    [ApiController]
    [Authorize]
    public class UserSettingsController : ControllerBase
    {
        private static readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web);

        private readonly IUserSettingsService _userSettingsService;
        private readonly IUserSettingsRegistry _userSettingsRegistry;

        public UserSettingsController(
            IUserSettingsService userSettingsService,
            IUserSettingsRegistry userSettingsRegistry)
        {
            _userSettingsService = userSettingsService;
            _userSettingsRegistry = userSettingsRegistry;
        }

        [HttpGet($"{{{ApiRouteParams.Key}}}")]
        public async Task<ActionResult> GetAsync(
            [FromRoute(Name = ApiRouteParams.Key)] string key)
        {
            if (_userSettingsRegistry.Resolve(key) == null)
                return NotFound();

            return Ok(await _userSettingsService.GetAsync(key));
        }

        [HttpPut($"{{{ApiRouteParams.Key}}}")]
        public async Task<ActionResult> SaveAsync(
            [FromRoute(Name = ApiRouteParams.Key)] string key,
            [FromBody] JsonElement settingsElement)
        {
            var settingsType = _userSettingsRegistry.Resolve(key);

            if (settingsType == null)
                return NotFound();

            var settings = (UserSettings?)settingsElement.Deserialize(settingsType, _jsonOptions);

            if (settings == null)
                return BadRequest();

            await _userSettingsService.SaveAsync(key, settings);

            return Ok();
        }
    }
}
