using System.Collections.ObjectModel;
using System.ComponentModel;

namespace OtherSideCore.Adapter.DomainObjectInteraction
{
   public interface IDomainObjectEditorViewModel : IDisposable, INotifyPropertyChanged
   {
      DomainObjectViewModel DomainObjectViewModel { get; }

      event EventHandler<DomainObjectViewModel> DomainObjectDeletedEvent;
      ObservableCollection<IDomainObjectBrowserViewModel> NestedDomainObjectBrowserViewModels { get; }
      bool HasUnsavedChanges { get; }
      bool IsEnabled { get; }
      bool CanSaveChanges();
      Task SaveChangesAsync();
      bool CanCancelChanges();
      Task CancelChangesAsync();
      Task LoadNestedBrowsersAsync();
   }
}
