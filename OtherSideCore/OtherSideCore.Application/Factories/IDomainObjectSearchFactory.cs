using OtherSideCore.Application.Search;

namespace OtherSideCore.Application.Factories
{
   public interface IDomainObjectSearchFactory
   {
      IDomainObjectSearch<T> CreateDomainObjectSearch<T>(IDomainObjectQueryServiceFactory domainObjectQueryServiceFactory) where T : DomainObjectSearchResult, new();
   }
}
