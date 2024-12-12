using OtherSideCore.Adapter.DomainObjectInteractionViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace OtherSideCore.Adapter.DomainObjectInteraction
{
   public interface IDomainObjectBrowserViewModel : IDisposable, INotifyPropertyChanged
   {
      bool HasUnsavedChanges { get; }
      Task SaveChangesAsync();
      Task CancelChangesAsync();
      Task SelectSearchResultViewModelAsync(DomainObjectSearchResultViewModel domainObjectSearchResultViewModel);
      Task InitializeAsync();
   }
}
