using OtherSideCore.Adapter.DomainObjectInteractionViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace OtherSideCore.Adapter.DomainObjectInteraction
{
   public interface IDomainObjectBrowserViewModel : IDisposable, INotifyPropertyChanged
   {
      bool HasUnsavedChanges { get; }
      IDomainObjectSearchViewModel DomainObjectSearchViewModel { get; }
      DomainObjectViewModel ContextViewModel { get; set; }
      Task SaveChangesAsync();
      Task CancelChangesAsync();
      Task SearchAsync(SearchParameters parameters);
      Task PaginatedSearchAsync(PaginatedSearchParameters parameters);
      void CancelSearch();
      Task SelectSearchResultViewModelAsync(DomainObjectSearchResultViewModel domainObjectSearchResultViewModel);
      bool CanShowDomainObjectDetailsEditor(DomainObjectSearchResultViewModel? obj);
      Task ShowDomainObjectDetailsEditorAsync(DomainObjectSearchResultViewModel domainObjectSearchResultViewModel);
      Task InitializeAsync();
   }
}
