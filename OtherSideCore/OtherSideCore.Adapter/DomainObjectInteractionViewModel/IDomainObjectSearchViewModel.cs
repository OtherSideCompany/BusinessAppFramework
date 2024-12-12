using OtherSideCore.Adapter.DomainObjectInteractionViewModel;

namespace OtherSideCore.Adapter.DomainObjectInteraction
{
   public interface IDomainObjectSearchViewModel : IDisposable
   {
      Task SearchAsync(SearchParameters parameters);
      Task PaginatedSearchAsync(PaginatedSearchParameters parameters);
      void LoadSearchResultViewModels();
      void UnloadSearchResultViewModels();
      void AddSearchResultViewModel(DomainObjectSearchResultViewModel domainObjectSearchResultViewModel);
      DomainObjectSearchResultViewModel AddSearchResultViewModel(int domainObjectId);
      void RemoveSearchResultViewModel(DomainObjectSearchResultViewModel domainObjectSearchResultViewModel);
      void RemoveSearchResultViewModel(int domainObjectId);
   }
}
