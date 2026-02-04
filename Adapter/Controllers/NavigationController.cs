using Application.Services;
using Contracts;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Adapter.Controllers
{
    [ApiController]
    [Authorize]
    public class NavigationController : ControllerBase
    {
        protected readonly ICurrentUserService _currentUserService;
        protected readonly IModuleBootstrapperProviderService _moduleProviderService;
        protected readonly IUserPermissionResolverService _userPermissionResolverService;

        public NavigationController(
           ICurrentUserService currentUserService,
           IModuleBootstrapperProviderService moduleProviderService,
           IUserPermissionResolverService userPermissionResolverService)
        {
            _currentUserService = currentUserService;
            _moduleProviderService = moduleProviderService;
            _userPermissionResolverService = userPermissionResolverService;
        }

        [HttpGet(Routes.ModulesTemplate)]
        public async Task<IActionResult> GetAvailableModules()
        {
            var workspaceKeys = new List<string>();

            if (_currentUserService.UserId.HasValue)
            {
                foreach (var module in _moduleProviderService.GetModules())
                {
                    var key = module.GetModuleWorkspaceKey();

                    if (key != null)
                    {
                        if (await _userPermissionResolverService.CanAccessAsync(key.Value.Key, _currentUserService.UserId.Value))
                        {
                            workspaceKeys.Add(key.Value.Key);
                        }
                    }
                }
            }

            return Ok(workspaceKeys);
        }

        [HttpGet(Routes.WorkspacesTemplate)]
        public async Task<IActionResult> GetAvailableWorkspaces([FromRoute(Name = Routes.ModuleKeyParam)] string moduleKey)
        {
            var workspaceKeys = new List<string>();

            if (_currentUserService.UserId.HasValue)
            {
                var module = _moduleProviderService.GetModuleByKey(StringKey.From(moduleKey));

                if (module != null)
                {
                    foreach (var key in module.GetWorkspacesKeys())
                    {
                        if (await _userPermissionResolverService.CanAccessAsync(key.Key, _currentUserService.UserId.Value))
                        {
                            workspaceKeys.Add(key.Key);
                        }
                    }
                }
            }

            return Ok(workspaceKeys);
        }
    }
}
