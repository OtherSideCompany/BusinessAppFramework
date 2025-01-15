using OtherSideCore.Adapter.DomainObjectInteractionViewModel;

namespace OtherSideCore.Adapter.DomainObjectInteraction
{
   public interface IDomainObjectSearchViewModel : IDisposable
   {
      Task SearchAsync(SearchParameters parameters);
      Task PaginatedSearchAsync(PaginatedSearchParameters parameters);
      void LoadSearchResultViewModels();
      Task ReloadSearchResultAsync(int domainObjectId);
      void UnloadSearchResultViewModels();
      void AddSearchResultViewModel(DomainObjectSearchResultViewModel domainObjectSearchResultViewModel);
      Task<DomainObjectSearchResultViewModel> AddSearchResultViewModelAsync(int domainObjectId);
      void RemoveSearchResultViewModel(DomainObjectSearchResultViewModel domainObjectSearchResultViewModel);
      void RemoveSearchResultViewModel(int domainObjectId);
   }
}
