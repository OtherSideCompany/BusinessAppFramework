using OtherSideCore.Application.Factories;
using OtherSideCore.Application.Repository;
using OtherSideCore.Domain.DomainObjects;
using System;
namespace OtherSideCore.Infrastructure.Factories
{
   public class RepositoryFactory : TypeBasedFactory, IRepositoryFactory
   {
      #region Fields

      private RepositoryDependencies _repositoryDependencies;

      #endregion

      #region Properties

      public RepositoryDependencies RepositoryDependencies => _repositoryDependencies;

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public RepositoryFactory(RepositoryDependencies repositoryDependencies) : base()
      {
         _repositoryDependencies = repositoryDependencies;
      }


      #endregion

      #region Public Methods

      public IRepository<T> CreateRepository<T>() where T : DomainObject
      {
         return (IRepository<T>)CreateFromType<T>();
      }

      public object CreateRepository(Type type)
      {
         return CreateFromType(type);
      }

      public void Register<T>(Func<IRepository<T>> factory) where T : DomainObject
      {
         base.Register<T>(() => factory());
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
