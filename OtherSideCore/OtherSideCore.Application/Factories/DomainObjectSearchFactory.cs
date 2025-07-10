using OtherSideCore.Application.Search;
using OtherSideCore.Domain.DomainObjects;
using System.Reflection;

namespace OtherSideCore.Application.Factories
{
   public class DomainObjectSearchFactory : IDomainObjectSearchFactory
   {
      #region Fields

      private IDomainObjectQueryServiceFactory _domainObjectQueryServiceFactory;
      private IDomainObjectServiceFactory _domainObjectServiceFactory;

      private TypeBasedFactory _domainObjectSearchFactory = new();
      

      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectSearchFactory(
         IDomainObjectQueryServiceFactory domainObjectQueryServiceFactory)
      {
         _domainObjectQueryServiceFactory = domainObjectQueryServiceFactory;

         _domainObjectSearchFactory.SetFallbackFactory(type => CreateDefaultDomainObjectSearch(type));
      }

      #endregion

      #region Public Methods

      public void RegisterDomainObjectSearch<T>(Func<IDomainObjectSearch<T>> factory) where T : DomainObjectSearchResult, new()
      {
         _domainObjectSearchFactory.Register<T>(() => factory());
      }

      public IDomainObjectSearch<T> CreateDomainObjectSearch<T>(IDomainObjectQueryServiceFactory domainObjectQueryServiceFactory) where T : DomainObjectSearchResult, new()
      {
         return (IDomainObjectSearch<T>)_domainObjectSearchFactory.CreateFromType<T>();
      }

      #endregion

      #region Private Methods

      private object CreateDefaultDomainObjectSearch(Type type)
      {
         var method = GetType()
             .GetMethod(nameof(CreateDefaultDomainObjectSearchGeneric), BindingFlags.NonPublic | BindingFlags.Instance)!
             .MakeGenericMethod(type);

         return method.Invoke(this, null)!;
      }

      private object CreateDefaultDomainObjectSearchGeneric<T>() where T : DomainObjectSearchResult, new()
      {
         return new DomainObjectSearch<T>(_domainObjectQueryServiceFactory.CreateDomainObjectQueryService<T>());
      }

      #endregion
   }
}
