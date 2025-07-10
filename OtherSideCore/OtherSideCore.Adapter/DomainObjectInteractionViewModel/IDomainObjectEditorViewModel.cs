using OtherSideCore.Adapter.DomainObjectInteractionViewModel;
using OtherSideCore.Domain;
using OtherSideCore.Domain.DomainObjects;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace OtherSideCore.Adapter.DomainObjectInteraction
{
   public interface IDomainObjectEditorViewModel : IDisposable, INotifyPropertyChanged
   {
      StringKey DomainObjectEditorKey { get; }
      DomainObjectViewModel DomainObjectViewModel { get; }
      DomainObjectEditorViewModelDependencies DomainObjectEditorViewModelDependencies { get; }
      IDomainObjectReferencesEditorViewModel DomainObjectReferencesEditorViewModel { get; }
      //ObservableCollection<DomainObjectReferenceViewModel> DomainObjectReferenceViewModels { get; }

      ObservableCollection<DomainObjectTreeViewModel> NestedDomainObjectTreeViewModels { get; }

      event EventHandler<int> DomainObjectSavedEvent;

      event EventHandler<int> DomainObjectDeletedEvent;

      bool HasUnsavedChanges { get; }
      bool IsEnabled { get; }
      bool IsReadOnly { get; set; }
      bool CanSaveChanges();
      Task SaveChangesAsync();
      bool CanCancelChanges();
      Task CancelChangesAsync();
      Task InitializeAsync();
      Task LoadNestedStructuresAsync();
      Task LoadDomainObjetReferencesAsync();
      Task<DomainObject?> DupplicateAsync(DomainObject? parent);
   }
}
