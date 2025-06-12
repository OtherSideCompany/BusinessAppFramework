using OtherSideCore.Application.Services;
using OtherSideCore.Domain.DomainObjects;
using System.Reflection;

namespace OtherSideCore.Application.Factories
{
   public class DomainObjectServiceFactory : TypeBasedFactory, IDomainObjectServiceFactory
   {
      #region Fields

      protected IRepositoryFactory _repositoryFactory;
      protected DomainObjectServiceDependencies _domainObjectServiceDependencies;

      #endregion

      #region Properties



      #endregion

      #region Constructor

      public DomainObjectServiceFactory(
         IRepositoryFactory repositoryFactory,
         DomainObjectServiceDependencies domainObjectServiceDependencies)
      {
         _repositoryFactory = repositoryFactory;
         _domainObjectServiceDependencies = domainObjectServiceDependencies;

         domainObjectServiceDependencies.DomainObjectServiceFactory = this;

         SetFallbackFactory(type => CreateDefaultDomainObjectService(type));
      }

      #endregion

      #region Public Methods

      public IDomainObjectService<T> CreateDomainObjectService<T>() where T : DomainObject, new()
      {
         return (IDomainObjectService<T>)CreateFromType<T>();
      }

      public object CreateDomainObjectService(Type type)
      {
         return CreateFromType(type);
      }

      public void Register<T>(Func<IDomainObjectService<T>> factory) where T : DomainObject, new()
      {
         base.Register<T>(() => factory());
      }

      #endregion

      #region Private Methods

      private object CreateDefaultDomainObjectService(Type type)
      {
         var method = GetType()
             .GetMethod(nameof(CreateDefaultDomainObjectServiceGeneric), BindingFlags.NonPublic | BindingFlags.Instance)!
             .MakeGenericMethod(type);

         return method.Invoke(this, null)!;
      }

      protected object CreateDefaultDomainObjectServiceGeneric<T>() where T : DomainObject, new()
      {
         var repo = _repositoryFactory.CreateRepository<T>();
         return new DomainObjectService<T>(repo, _domainObjectServiceDependencies);
      }

      #endregion
   }
}
