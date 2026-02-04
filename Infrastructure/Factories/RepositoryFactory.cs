using Application.Factories;
using Application.Repository;
using Domain.DomainObjects;
using Microsoft.Extensions.DependencyInjection;
using System;
namespace Infrastructure.Factories
{
   public class RepositoryFactory : IRepositoryFactory
   {
      #region Fields

      private readonly IServiceProvider _serviceProvider;

      #endregion

      #region Properties


      #endregion

      #region Commands



      #endregion

      #region Constructor

      public RepositoryFactory(IServiceProvider serviceProvider)
      {
         _serviceProvider = serviceProvider;
      }


      #endregion

      #region Public Methods

      public IRepository<T> CreateRepository<T>() where T : DomainObject
      {
         return _serviceProvider.GetRequiredService<IRepository<T>>();
      }

      public object CreateRepository(Type type)
      {
         var serviceType = typeof(IRepository<>).MakeGenericType(type);
         return _serviceProvider.GetRequiredService(serviceType);
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
