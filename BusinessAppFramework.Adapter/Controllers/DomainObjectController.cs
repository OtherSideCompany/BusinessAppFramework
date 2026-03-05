using BusinessAppFramework.Application.Actions;
using BusinessAppFramework.Application.Factories;
using BusinessAppFramework.Application.Search;
using BusinessAppFramework.Contracts.ApiRoutes;
using BusinessAppFramework.Domain.DomainObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusinessAppFramework.Adapter.Controllers
{
   [ApiController]
   [Authorize]
   public abstract class DomainObjectController<TDomainObject, TSearchResult> : ControllerBase
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

      [HttpPost(DomainObjectRouteSegments.Create)]
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

      [HttpGet($"{DomainObjectRouteSegments.Get}/{{{ApiRouteParams.DomainObjectId}:int}}")]
      public virtual async Task<ActionResult<TDomainObject>> GetAsync(
          [FromRoute(Name = ApiRouteParams.DomainObjectId)] int domainObjectId)
      {
         var service = _domainObjectServiceFactory.CreateDomainObjectService<TDomainObject>();
         var domainObject = await service.GetAsync(domainObjectId);

         return Ok(domainObject);
      }

      [HttpGet($"{DomainObjectRouteSegments.GetHydrated}/{{{ApiRouteParams.DomainObjectId}:int}}")]
      public virtual async Task<ActionResult<TDomainObject>> GetHydratedAsync(
          [FromRoute(Name = ApiRouteParams.DomainObjectId)] int domainObjectId)
      {
         var service = _domainObjectServiceFactory.CreateDomainObjectService<TDomainObject>();
         var domainObject = await service.GetHydratedAsync(domainObjectId);

         return Ok(domainObject);
      }

      [HttpGet($"{DomainObjectRouteSegments.GetHydratedReference}/{{{ApiRouteParams.DomainObjectId}:int}}/{{{ApiRouteParams.Key}}}")]
      public virtual async Task<ActionResult<DomainObjectReference>> GetHydratedDomainObjectReferenceAsync(
          [FromRoute(Name = ApiRouteParams.DomainObjectId)] int domainObjectId,
          [FromRoute(Name = ApiRouteParams.Key)] string relationKey)
      {
         var service = _domainObjectServiceFactory.CreateDomainObjectService<TDomainObject>();
         var domainObjectReference = await service.GetHydratedDomainObjectReference(domainObjectId, relationKey);

         return Ok(domainObjectReference);
      }

      [HttpGet($"{DomainObjectRouteSegments.GetHydratedReferenceList}/{{{ApiRouteParams.DomainObjectId}:int}}/{{{ApiRouteParams.Key}}}")]
      public virtual async Task<ActionResult<DomainObjectReferenceListItem>> GetHydratedDomainObjectReferenceListItemAsync(
          [FromRoute(Name = ApiRouteParams.DomainObjectId)] int domainObjectId,
          [FromRoute(Name = ApiRouteParams.Key)] string key)
      {
         var service = _domainObjectServiceFactory.CreateDomainObjectService<TDomainObject>();
         var domainObjectReferenceListItem = await service.GetHydratedDomainObjectReferenceListItem(domainObjectId, key);

         return Ok(domainObjectReferenceListItem);
      }


      [HttpPut(DomainObjectRouteSegments.Save)]
      public virtual async Task<ActionResult<DomainObjectApplicationActionResultPayload>> SaveAsync(
          [FromBody] TDomainObject domainObject)
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

      [HttpDelete($"{DomainObjectRouteSegments.Delete}/{{{ApiRouteParams.DomainObjectId}:int}}")]
      public virtual async Task<ActionResult<DomainObjectApplicationActionResultPayload>> DeleteAsync(
          [FromRoute(Name = ApiRouteParams.DomainObjectId)] int domainObjectId)
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
