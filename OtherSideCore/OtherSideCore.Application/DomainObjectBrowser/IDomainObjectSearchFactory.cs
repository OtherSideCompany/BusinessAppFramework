using OtherSideCore.Application.Services;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Application.DomainObjectBrowser
{
   public interface IDomainObjectSearchFactory
   {
      IDomainObjectSearch<T> CreateDomainObjectSearch<T>(IDomainObjectQueryServiceFactory domainObjectQueryServiceFactory) where T : DomainObject, new();
      IDomainObjectTreeSearch CreateDomainObjectTreeSearch(DomainObject domainObject, IDomainObjectQueryServiceFactory domainObjectQueryServiceFactory);
   }
}
