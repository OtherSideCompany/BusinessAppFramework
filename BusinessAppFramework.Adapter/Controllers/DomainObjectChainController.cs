using BusinessAppFramework.Application.DomainObjectChains;
using BusinessAppFramework.Contracts.ApiRoutes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusinessAppFramework.Adapter.Controllers
{
    [ApiController]
    [Authorize]
    public class DomainObjectChainController : ControllerBase
    {
        #region Fields

        private readonly IDomainObjectChainRegistry _registry;
        private readonly IDomainObjectChainService _domainObjectChainService;

        #endregion

        #region Constructor

        public DomainObjectChainController(IDomainObjectChainRegistry registry, IDomainObjectChainService domainObjectChainService)
        {
            _registry = registry;
            _domainObjectChainService = domainObjectChainService;
        }

        #endregion

        #region Public Methods

        [HttpGet($"{ApiRouteSegments.GetChain}/{{{ApiRouteParams.DomainObjectId}:int}}")]
        public async Task<ActionResult<DomainObjectChain>> GetChainAsync(
            [FromRoute(Name = ApiRouteParams.DomainObjectId)] int domainObjectId,
            [FromQuery] string typeKey,
            [FromQuery] DomainObjectChainDirection direction,
            CancellationToken ct)
        {
            var type = _registry.ResolveType(typeKey);

            if (type == null)
                return Ok(new DomainObjectChain());

            return Ok(await _domainObjectChainService.GetChainAsync(type, domainObjectId, direction, ct));
        }

        #endregion
    }
}
