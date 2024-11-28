using OtherSideCore.Application.DomainObjectBrowser;
using OtherSideCore.Appplication.Services;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Adapter.DomainObjectInteraction
{
   public interface IDomainObjectsSearchViewModelFactory
   {
      IDomainObjectSearchViewModel CreateDomainObjectSearchViewModel<T>(
         DomainObjectSearch<T> domainObjectSearch, 
         IDomainObjectViewModelFactory domainObjectViewModelFactory) where T : DomainObject, new();
   }
}
