using OtherSideCore.Domain.DomainObjects;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace OtherSideCore.Adapter.DomainObjectInteraction
{
   public interface IDomainObjectEditorViewModel : IDisposable, INotifyPropertyChanged
   {
      DomainObjectViewModel DomainObjectViewModel { get; }

      ObservableCollection<DomainObjectTreeViewModel> NestedDomainObjectTreeViewModels { get; }

      event EventHandler<int> DomainObjectSavedEvent;

      event EventHandler<int> DomainObjectDeletedEvent;

      event EventHandler DomainObjectReferencesModified;

      bool HasUnsavedChanges { get; }
      bool IsEnabled { get; }
      bool CanSaveChanges();
      Task SaveChangesAsync();
      bool CanCancelChanges();
      Task CancelChangesAsync();
      Task LoadNestedStructuresAsync();
      Task LoadDomainObjetReferencesAsync();
      Task<DomainObject> DupplicateAsync(DomainObject? parent);
   }
}
