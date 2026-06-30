using BusinessAppFramework.Application.Relations;
using BusinessAppFramework.Contracts.ApiRoutes;
using BusinessAppFramework.Domain.DomainObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusinessAppFramework.Adapter.Controllers
{
    [ApiController]
    [Authorize]
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

        [HttpGet($"{RelationshipSegments.GetHydratedReference}/{{{ApiRouteParams.ParentDomainObjectId}:int}}/{{{ApiRouteParams.DomainObjectId}:int}}/{{{ApiRouteParams.Key}}}")]
        public virtual async Task<ActionResult<DomainObjectReference?>> GetHydratedReference(
            [FromRoute(Name = ApiRouteParams.ParentDomainObjectId)] int parentDomainObjectId,
            [FromRoute(Name = ApiRouteParams.DomainObjectId)] int domainObjectId,
            [FromRoute(Name = ApiRouteParams.Key)] string key)
        {
            var reference = await _relationService.GetHydratedReferenceAsync(parentDomainObjectId, domainObjectId, key);

            if (reference == null)
                return NoContent();

            return Ok(reference);
        }

        [HttpGet($"{RelationshipSegments.GetHydratedReference}/{{{ApiRouteParams.DomainObjectId}:int}}/{{{ApiRouteParams.Key}}}")]
        public virtual async Task<ActionResult<DomainObjectReference>> GetHydratedDomainObjectReferenceAsync(
            [FromRoute(Name = ApiRouteParams.DomainObjectId)] int domainObjectId,
            [FromRoute(Name = ApiRouteParams.Key)] string relationKey)
        {
            var domainObjectReference = await _relationService.GetHydratedDomainObjectReferenceAsync(domainObjectId, relationKey);

            return Ok(domainObjectReference);
        }

        [HttpGet($"{RelationshipSegments.GetHydratedReferenceListItem}/{{{ApiRouteParams.DomainObjectId}:int}}/{{{ApiRouteParams.Key}}}")]
        public virtual async Task<ActionResult<DomainObjectReferenceListItem>> GetHydratedDomainObjectReferenceListItemAsync(
            [FromRoute(Name = ApiRouteParams.DomainObjectId)] int domainObjectId,
            [FromRoute(Name = ApiRouteParams.Key)] string key)
        {
            var domainObjectReferenceListItem = await _relationService.GetHydratedDomainObjectReferenceListItemAsync(domainObjectId, key);

            return Ok(domainObjectReferenceListItem);
        }

        [HttpGet($"{RelationshipSegments.GetChildrenIds}/{{{ApiRouteParams.ParentDomainObjectId}:int}}/{{{ApiRouteParams.Key}}}")]
        public virtual async Task<ActionResult<List<int>>> GetChildrenIds(
            [FromRoute(Name = ApiRouteParams.ParentDomainObjectId)] int parentDomainObjectId,
            [FromRoute(Name = ApiRouteParams.Key)] string key)
        {
            var reference = await _relationService.GetChildrenIdsAsync(parentDomainObjectId, key);

            return Ok(reference);
        }

        #endregion

        #region Private Methods



        #endregion
    }
}
