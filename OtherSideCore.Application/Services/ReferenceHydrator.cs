using OtherSideCore.Application.Factories;
using OtherSideCore.Application.Interfaces;
using OtherSideCore.Application.Relations;
using OtherSideCore.Domain;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Application.Services
{
    public class ReferenceHydrator : IReferenceHydrator
    {
        #region Fields

        private readonly IRelationResolver _relationResolver;
        private readonly IRelationService _relationService;

        #endregion

        #region Properties



        #endregion

        #region Events



        #endregion

        #region Constructor

        public ReferenceHydrator(
            IRelationResolver relationResolver,
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
                    if (_relationResolver.TryGetReferenceRelationEntry(StringKey.From(domainObjectReference.RelationKey), out var relationEntry))
                    {
                        await _relationService.HydrateDomainObjectReferenceAsync(domainObjectReference);
                    }
                }
            }

            foreach (var domainObjectReferenceList in domainObject.GetReferenceLists())
            {
                if (_relationResolver.TryGetReferenceListRelationEntry(StringKey.From(domainObjectReferenceList.RelationKey), out var relationListEntry))
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

