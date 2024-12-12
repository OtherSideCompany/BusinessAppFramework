using OtherSideCore.Application.Services;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Application.Factories
{
    public abstract class DomainObjectQueryServiceFactory : IDomainObjectQueryServiceFactory
    {
        protected IRepositoryFactory _repositoryFactory;

        public DomainObjectQueryServiceFactory(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        public abstract IDomainObjectQueryService<T> CreateDomainObjectQueryService<T>() where T : DomainObject, new();
    }
}
