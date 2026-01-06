using OtherSideCore.Application.Factories;
using OtherSideCore.Application.Interfaces;
using OtherSideCore.Application.Relations;
using OtherSideCore.Application.Repository;
using OtherSideCore.Domain;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Application.Services
{
    public class ReferenceHydrator : IReferenceHydrator
    {
        #region Fields

        private readonly IRelationResolver _relationResolver;
        private readonly IRepositoryFactory _repositoryFactory;

        #endregion

        #region Properties



        #endregion

        #region Events



        #endregion

        #region Constructor

        public ReferenceHydrator(
            IRelationResolver relationResolver,
            IRepositoryFactory repositoryFactory)
        {
            _relationResolver = relationResolver;
            _repositoryFactory = repositoryFactory;
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
                        IRelationRepository repository = (IRelationRepository)_repositoryFactory.CreateRepository(relationEntry.TargetDomainObjectType);
                        await repository.HydrateDomainObjectReferenceAsync(domainObjectReference);
                    }
                }
            }
        }

        #endregion


        #region Private Methods



        #endregion
    }
}
