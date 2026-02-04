using OtherSideCore.Adapter.DomainObjectInteractionViewModel;
using OtherSideCore.Domain.DomainObjects;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace OtherSideCore.Adapter.DomainObjectInteraction
{
   public interface IDomainObjectBrowserViewModel : IDisposable, INotifyPropertyChanged
   {
      bool HasUnsavedChanges { get; }
      IDomainObjectSearchViewModel DomainObjectSearchViewModel { get; }
      DomainObjectViewModel ContextViewModel { get; set; }
      bool CanSaveChanges();
      Task SaveChangesAsync();
      bool CanCancelChanges();
      Task CancelChangesAsync();
      Task SearchAsync(SearchParameters parameters);
      Task PaginatedSearchAsync(PaginatedSearchParameters parameters);
      void CancelSearch();
      Task SelectSearchResultViewModelAsync(DomainObjectSearchResultViewModel domainObjectSearchResultViewModel);
      bool CanShowDomainObjectDetailsEditor(DomainObjectSearchResultViewModel? obj);
      Task ShowDomainObjectDetailsEditorAsync(DomainObjectSearchResultViewModel domainObjectSearchResultViewModel);
      Task InitializeAsync(int? domainObjectId = null);
   }
}
