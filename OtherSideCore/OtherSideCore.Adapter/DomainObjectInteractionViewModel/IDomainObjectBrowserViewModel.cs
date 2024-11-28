using System.Collections.ObjectModel;
using System.ComponentModel;

namespace OtherSideCore.Adapter.DomainObjectInteraction
{
   public interface IDomainObjectBrowserViewModel : IDisposable, INotifyPropertyChanged
   {
      bool HasUnsavedChanges { get; }
      public ObservableCollection<IDomainObjectEditorViewModel> DomainObjectEditorViewModels { get; }
      Task SaveChangesAsync();
      Task CancelChangesAsync();
      Task LoadEditorViewModelsAsync(IEnumerable<DomainObjectViewModel> domainObjectViewModels);
      void UnloadEditorViewModels(IEnumerable<DomainObjectViewModel> domainObjectViewModels);
      Task SelectSearchResultViewModelAsync(DomainObjectViewModel domainObjectViewModel);
      Task InitializeAsync();
   }
}
