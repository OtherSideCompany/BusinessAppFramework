using System.Collections.ObjectModel;
using System.ComponentModel;

namespace OtherSideCore.Adapter.DomainObjectInteraction
{
   public interface IDomainObjectEditorViewModel : IDisposable, INotifyPropertyChanged
   {
      DomainObjectViewModel DomainObjectViewModel { get; }

      event EventHandler<int> DomainObjectDeletedEvent;
      bool HasUnsavedChanges { get; }
      bool IsEnabled { get; }
      bool CanSaveChanges();
      Task SaveChangesAsync();
      bool CanCancelChanges();
      Task CancelChangesAsync();
      Task LoadNestedStructuresAsync();

      Task LoadDomainObjetReferencesAsync();
   }
}
