using BusinessAppFramework.Application.Relations;
using BusinessAppFramework.Contracts;
using BusinessAppFramework.Contracts.ApiRoutes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusinessAppFramework.Adapter.Controllers
{
    [ApiController]
    [Authorize]
    [Route($"{ApiRouteSegments.Root}/{ApiRouteSegments.RelationShips}")]
    public class RelationServiceController : ControllerBase
    {
        #region Fields

        private IRelationService _relationService;

        #endregion

        #region Properties



        #endregion

        #region Events



        #endregion

        #region Constructor

        public RelationServiceController(IRelationService relationService)
        {
            _relationService = relationService;
        }

        #endregion

        #region Public Methods

        [HttpPut($"{RelationshipSegments.SetParent}/{{{ApiRouteParams.ParentDomainObjectId}:int}}/{{{ApiRouteParams.DomainObjectId}:int}}/{{{ApiRouteParams.Key}}}")]
        public virtual async Task<ActionResult> SetParent(
            [FromRoute(Name = ApiRouteParams.ParentDomainObjectId)] int parentDomainObjectId,
            [FromRoute(Name = ApiRouteParams.DomainObjectId)] int domainObjectId,
            [FromRoute(Name = ApiRouteParams.Key)] string key)
        {
            await _relationService.SetParentAsync(parentDomainObjectId, domainObjectId, key);

            return Ok();
        }

        #endregion

        #region Private Methods



        #endregion
    }
}
