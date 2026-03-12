using BusinessAppFramework.Application.Services;
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
        protected readonly IModuleBootstrapperPurchaseService _modulePurchaseService;
        protected readonly IUserPermissionResolverService _userPermissionResolverService;

        public NavigationController(
           ICurrentUserService currentUserService,
           IModuleBootstrapperPurchaseService modulePurchaseService,
           IUserPermissionResolverService userPermissionResolverService)
        {
            _currentUserService = currentUserService;
            _modulePurchaseService = modulePurchaseService;
            _userPermissionResolverService = userPermissionResolverService;
        }

        [HttpGet(NavigationRouteSegments.Modules)]
        public async Task<ActionResult<List<string>>> GetAvailableModules()
        {
            var workspaceKeys = new List<string>();

            if (_currentUserService.UserId.HasValue)
            {
                foreach (var module in _modulePurchaseService.GetModules())
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
                var module = _modulePurchaseService.GetModuleByKey(StringKey.From(moduleKey));

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
