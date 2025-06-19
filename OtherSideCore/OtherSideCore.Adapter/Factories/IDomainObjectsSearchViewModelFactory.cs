using OtherSideCore.Adapter.DomainObjectInteraction;
using OtherSideCore.Application.Factories;
using OtherSideCore.Application.Search;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Adapter.Factories
{
   public interface IDomainObjectsSearchViewModelFactory
   {
      void RegisterDomainObjectSearchViewModel<TSearchResult>(Func<IDomainObjectSearch<TSearchResult>, IDomainObjectSearchViewModel> factory) where TSearchResult : DomainObjectSearchResult, new();
      IDomainObjectSearchViewModel CreateDomainObjectSearchViewModel<TSearchResult>(IDomainObjectSearch<TSearchResult> domainObjectSearch) where TSearchResult : DomainObjectSearchResult, new();
   }
}
