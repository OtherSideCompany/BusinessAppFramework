using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Application.Relations;
using BusinessAppFramework.Domain;
using BusinessAppFramework.Domain.DomainObjects;

namespace BusinessAppFramework.Application.Services
{
   public class ReferenceHydrator : IReferenceHydrator
   {
      #region Fields

      private readonly IReferenceResolver _relationResolver;
      private readonly IRelationService _relationService;

      #endregion

      #region Properties



      #endregion

      #region Events



      #endregion

      #region Constructor

      public ReferenceHydrator(
          IReferenceResolver relationResolver,
          IRelationService relationService)
      {
         _relationResolver = relationResolver;
         _relationService = relationService;
      }

      #endregion

      #region Public Methods

      public async Task HydrateAsync(DomainObject domainObject)
      {
         foreach (var domainObjectReference in domainObject.GetReferences())
         {
            if (domainObjectReference != null && domainObjectReference.DomainObjectId != null && domainObjectReference.DomainObjectId > 0)
            {
               if (_relationResolver.TryGetReferenceRelationEntry(domainObjectReference.RelationKey, out var relationEntry))
               {
                  await _relationService.HydrateDomainObjectReferenceAsync(domainObjectReference);
               }
            }
         }

         foreach (var domainObjectReferenceList in domainObject.GetReferenceLists())
         {
            if (_relationResolver.TryGetReferenceListRelationEntry(domainObjectReferenceList.RelationKey, out var relationListEntry))
            {
               await _relationService.HydrateDomainObjectReferenceListAsync(domainObjectReferenceList);

            }
         }
      }
   }

   #endregion


   #region Private Methods



   #endregion
}

