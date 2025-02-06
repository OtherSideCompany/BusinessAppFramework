using OtherSideCore.Adapter.DomainObjectInteractionViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace OtherSideCore.Adapter.DomainObjectInteraction
{
   public interface IDomainObjectBrowserViewModel : IDisposable, INotifyPropertyChanged
   {
      bool HasUnsavedChanges { get; }
      IDomainObjectSearchViewModel DomainObjectSearchViewModel { get; }
      Task SaveChangesAsync();
      Task CancelChangesAsync();
      Task SelectSearchResultViewModelAsync(DomainObjectSearchResultViewModel domainObjectSearchResultViewModel);
      bool CanShowDomainObjectDetailsEditor(DomainObjectSearchResultViewModel? obj);
      Task ShowDomainObjectDetailsEditorAsync(DomainObjectSearchResultViewModel domainObjectSearchResultViewModel);
      Task InitializeAsync();
   }
}
