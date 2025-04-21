using OtherSideCore.Adapter.DomainObjectInteractionViewModel;
using System.Collections.ObjectModel;

namespace OtherSideCore.Adapter.DomainObjectInteraction
{
   public interface IDomainObjectSearchViewModel : IDisposable
   {
      ObservableCollection<DomainObjectSearchResultViewModel> SearchResultViewModels { get; }
      MultiTextFilterViewModel MultiTextFilterViewModel { get; }
      Task SearchAsync(SearchParameters parameters);
      Task PaginatedSearchAsync(PaginatedSearchParameters parameters);
      void CancelSearch();
      void LoadSearchResultViewModels();
      Task ReloadSearchResultAsync(int domainObjectId);
      void UnloadSearchResultViewModels();
      void AddSearchResultViewModel(DomainObjectSearchResultViewModel domainObjectSearchResultViewModel);
      Task<DomainObjectSearchResultViewModel> AddSearchResultViewModelAsync(int domainObjectId);
      Task<DomainObjectSearchResultViewModel> InsertSearchResultViewModelAsync(int domainObjectId, int index);
      void RemoveSearchResultViewModel(DomainObjectSearchResultViewModel domainObjectSearchResultViewModel);
      void RemoveSearchResultViewModel(int domainObjectId);
   }
}
