using OtherSideCore.Application.Services;
using OtherSideCore.Domain.DomainObjects;
using OtherSideCore.Domain.Services;

namespace OtherSideCore.Application.Factories
{
    public abstract class DomainObjectServiceFactory : IDomainObjectServiceFactory
    {
        protected IRepositoryFactory _repositoryFactory;
        protected IUserContext _userContext;

        public DomainObjectServiceFactory(IRepositoryFactory repositoryFactory, IUserContext userContext)
        {
            _repositoryFactory = repositoryFactory;
            _userContext = userContext;
        }

        public abstract IDomainObjectService<T> CreateDomainObjectService<T>() where T : DomainObject, new();
    }
}
