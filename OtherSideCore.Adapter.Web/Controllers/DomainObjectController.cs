using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OtherSideCore.Application.Factories;
using OtherSideCore.Application.Search;
using OtherSideCore.Contracts;
using OtherSideCore.Contracts.ActionResult;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Adapter.Web.Controllers
{
    [ApiController]
    [Authorize]
    public class DomainObjectController<TDomainObject, TSearchResult> : ControllerBase
        where TDomainObject : DomainObject, new()
        where TSearchResult : DomainObjectSearchResult, new()
    {
        #region Fields

        protected IDomainObjectServiceFactory _domainObjectServiceFactory;

        #endregion

        #region Properties



        #endregion

        #region Events



        #endregion

        #region Constructor

        public DomainObjectController(
            IDomainObjectServiceFactory domainObjectServiceFactory)
        {
            _domainObjectServiceFactory = domainObjectServiceFactory;
        }

        #endregion

        #region Public Methods

        [HttpPost(Routes.CreateTemplate)]
        public virtual async Task<ActionResult<DomainObjectApplicationActionResultPayload>> CreateAsync()
        {
            var service = _domainObjectServiceFactory.CreateDomainObjectService<TDomainObject>();
            var domainObject = await service.CreateAsync();

            var applicationActionResultPayload = new DomainObjectApplicationActionResultPayload();
            applicationActionResultPayload.Changes.Add(new DomainObjectChange
            {
                DomainObjectId = domainObject.Id,
                ChangeType = ChangeType.Added
            });

            return Ok(applicationActionResultPayload);
        }

        [HttpGet(Routes.GetTemplate)]
        public virtual async Task<ActionResult<TDomainObject>> GetAsync(int domainObjectId)
        {
            var service = _domainObjectServiceFactory.CreateDomainObjectService<TDomainObject>();
            var domainObject = await service.GetAsync(domainObjectId);

            return Ok(domainObject);
        }

        [HttpGet(Routes.GetHydratedTemplate)]
        public virtual async Task<ActionResult<TDomainObject>> GetHydratedAsync(int domainObjectId)
        {
            var service = _domainObjectServiceFactory.CreateDomainObjectService<TDomainObject>();
            var domainObject = await service.GetHydratedAsync(domainObjectId);

            return Ok(domainObject);
        }

        [HttpGet(Routes.GetHydratedDomainObjectReferenceTemplate)]
        public virtual async Task<ActionResult<DomainObjectReference>> GetHydratedDomainObjectReferenceAsync(int domainObjectId, string key)
        {
            var service = _domainObjectServiceFactory.CreateDomainObjectService<TDomainObject>();
            var domainObjectReference = await service.GetHydratedDomainObjectReference(domainObjectId, key);

            return Ok(domainObjectReference);
        }

        [HttpGet(Routes.GetHydratedDomainObjectReferenceListItemTemplate)]
        public virtual async Task<ActionResult<DomainObjectReferenceListItem>> GetHydratedDomainObjectReferenceListItemAsync(int domainObjectId, string key)
        {
            var service = _domainObjectServiceFactory.CreateDomainObjectService<TDomainObject>();
            var domainObjectReferenceListItem = await service.GetHydratedDomainObjectReferenceListItem(domainObjectId, key);

            return Ok(domainObjectReferenceListItem);
        }       


        [HttpPut(Routes.SaveTemplate)]
        public virtual async Task<ActionResult<DomainObjectApplicationActionResultPayload>> SaveAsync(TDomainObject domainObject)
        {
            var service = _domainObjectServiceFactory.CreateDomainObjectService<TDomainObject>();
            await service.SaveAsync(domainObject);

            var applicationActionResultPayload = new DomainObjectApplicationActionResultPayload();
            applicationActionResultPayload.Changes.Add(new DomainObjectChange
            {
                DomainObjectId = domainObject.Id,
                ChangeType = ChangeType.Modified
            });

            return Ok(applicationActionResultPayload);
        }

        [HttpDelete(Routes.DeleteTemplate)]
        public virtual async Task<ActionResult<DomainObjectApplicationActionResultPayload>> DeleteAsync(int domainObjectId)
        {
            var service = _domainObjectServiceFactory.CreateDomainObjectService<TDomainObject>();
            await service.DeleteAsync(domainObjectId);

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



        #endregion
    }
}
