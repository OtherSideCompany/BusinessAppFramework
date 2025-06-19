using OtherSideCore.Application.Search;

namespace OtherSideCore.Application.Factories
{
   public interface IDomainObjectSearchFactory
   {
      void RegisterDomainObjectSearch<T>(Func<IDomainObjectSearch<T>> factory) where T : DomainObjectSearchResult, new();
      IDomainObjectSearch<T> CreateDomainObjectSearch<T>(IDomainObjectQueryServiceFactory domainObjectQueryServiceFactory) where T : DomainObjectSearchResult, new();
   }
}
