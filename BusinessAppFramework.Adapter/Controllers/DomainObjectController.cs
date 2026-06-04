using BusinessAppFramework.Application.Actions;
using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Application.Search;
using BusinessAppFramework.Contracts.ApiRoutes;
using BusinessAppFramework.Domain.DomainObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusinessAppFramework.Adapter.Controllers
{
    [ApiController]
    [Authorize]
    public class DomainObjectController<TDomainObject, TSearchResult> : ControllerBase
        where TDomainObject : DomainObject, new()
        where TSearchResult : DomainObjectSearchResult, new()
    {
        #region Fields

        protected IDomainObjectService<TDomainObject> _service;

        #endregion

        #region Properties



        #endregion

        #region Events



        #endregion

        #region Constructor

        public DomainObjectController(
            IDomainObjectService<TDomainObject> service)
        {
            _service = service;
        }

        #endregion

        #region Public Methods

        [HttpPost(DomainObjectRouteSegments.Create)]
        public virtual async Task<ActionResult<DomainObjectApplicationActionResultPayload>> CreateAsync()
        {
            var domainObject = await _service.CreateAsync();

            var applicationActionResultPayload = new DomainObjectApplicationActionResultPayload();
            applicationActionResultPayload.Changes.Add(new DomainObjectChange
            {
                DomainObjectId = domainObject.Id,
                ChangeType = ChangeType.Added
            });

            return Ok(applicationActionResultPayload);
        }

        [HttpGet($"{DomainObjectRouteSegments.Get}/{{{ApiRouteParams.DomainObjectId}:int}}")]
        public virtual async Task<ActionResult<TDomainObject>> GetAsync(
            [FromRoute(Name = ApiRouteParams.DomainObjectId)] int domainObjectId)
        {
            var domainObject = await _service.GetAsync(domainObjectId);

            return Ok(domainObject);
        }

        [HttpGet($"{DomainObjectRouteSegments.GetHydrated}/{{{ApiRouteParams.DomainObjectId}:int}}")]
        public virtual async Task<ActionResult<TDomainObject>> GetHydratedAsync(
            [FromRoute(Name = ApiRouteParams.DomainObjectId)] int domainObjectId)
        {
            var domainObject = await _service.GetHydratedAsync(domainObjectId);

            return Ok(domainObject);
        }

        [HttpGet($"{DomainObjectRouteSegments.GetHydratedReference}/{{{ApiRouteParams.DomainObjectId}:int}}/{{{ApiRouteParams.Key}}}")]
        public virtual async Task<ActionResult<DomainObjectReference>> GetHydratedDomainObjectReferenceAsync(
            [FromRoute(Name = ApiRouteParams.DomainObjectId)] int domainObjectId,
            [FromRoute(Name = ApiRouteParams.Key)] string relationKey)
        {
            var domainObjectReference = await _service.GetHydratedDomainObjectReference(domainObjectId, relationKey);

            return Ok(domainObjectReference);
        }

        [HttpGet($"{DomainObjectRouteSegments.GetHydratedReferenceList}/{{{ApiRouteParams.DomainObjectId}:int}}/{{{ApiRouteParams.Key}}}")]
        public virtual async Task<ActionResult<DomainObjectReferenceListItem>> GetHydratedDomainObjectReferenceListItemAsync(
            [FromRoute(Name = ApiRouteParams.DomainObjectId)] int domainObjectId,
            [FromRoute(Name = ApiRouteParams.Key)] string key)
        {
            var domainObjectReferenceListItem = await _service.GetHydratedDomainObjectReferenceListItem(domainObjectId, key);

            return Ok(domainObjectReferenceListItem);
        }


        [HttpPut(DomainObjectRouteSegments.Save)]
        public virtual async Task<ActionResult<DomainObjectApplicationActionResultPayload>> SaveAsync(
            [FromBody] TDomainObject domainObject)
        {
            var (isValid, validationError) = await _service.ValidateSaveAsync(domainObject);

            if (!isValid)
            {
                var payload = new DomainObjectApplicationActionResultPayload()
                {
                    ErrorMessageKey = validationError
                };
                return Ok(payload);
            }

            await _service.SaveAsync(domainObject);
            return Ok(CreateModifiedPayload(domainObject.Id));
        }

        [HttpDelete($"{DomainObjectRouteSegments.Delete}/{{{ApiRouteParams.DomainObjectId}:int}}")]
        public virtual async Task<ActionResult<DomainObjectApplicationActionResultPayload>> DeleteAsync(
            [FromRoute(Name = ApiRouteParams.DomainObjectId)] int domainObjectId)
        {
            await _service.DeleteAsync(domainObjectId);

            var applicationActionResultPayload = new DomainObjectApplicationActionResultPayload();
            applicationActionResultPayload.Changes.Add(new DomainObjectChange
            {
                DomainObjectId = domainObjectId,
                ChangeType = ChangeType.Deleted
            });

            return Ok(applicationActionResultPayload);
        }

        #endregion

        #region Private Methods

        protected DomainObjectApplicationActionResultPayload CreateModifiedPayload(int domainObjectId)
        {
            var applicationActionResultPayload = new DomainObjectApplicationActionResultPayload();

            applicationActionResultPayload.Changes.Add(new DomainObjectChange
            {
                DomainObjectId = domainObjectId,
                ChangeType = ChangeType.Modified
            });

            return applicationActionResultPayload;
        }

        protected DomainObjectApplicationActionResultPayload CreateErrorPayload(string errorMessageKey)
        {
            var applicationActionResultPayload = new DomainObjectApplicationActionResultPayload()
            {
                ErrorMessageKey = errorMessageKey
            };

            return applicationActionResultPayload;
        }

        #endregion
    }
}
