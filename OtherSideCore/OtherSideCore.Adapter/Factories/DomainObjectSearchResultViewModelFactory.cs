using OtherSideCore.Adapter.DomainObjectInteractionViewModel;
using OtherSideCore.Application.Search;

namespace OtherSideCore.Adapter.Factories
{
   public class DomainObjectSearchResultViewModelFactory : IDomainObjectSearchResultViewModelFactory
   {
      public DomainObjectSearchResultViewModel CreateViewModel(DomainObjectSearchResult domainObjectSearchResult)
      {
         return new DomainObjectSearchResultViewModel(domainObjectSearchResult);
      }
   }
}
