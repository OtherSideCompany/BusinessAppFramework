using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Contracts.ApiRoutes;
using BusinessAppFramework.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusinessAppFramework.Adapter.Controllers
{
    [ApiController]
    [Authorize]
    [Route($"{ApiRouteSegments.Root}/{ApiRouteSegments.Navigation}")]
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

        [HttpGet(NavigationRouteSegments.Modules)]
        public async Task<ActionResult<List<string>>> GetAvailableModules()
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

        [HttpGet($"{{{ApiRouteParams.ModuleKey}}}/{NavigationRouteSegments.Workspaces}")]
        public async Task<ActionResult<List<string>>> GetAvailableWorkspaces(
            [FromRoute(Name = ApiRouteParams.ModuleKey)] string moduleKey)
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
