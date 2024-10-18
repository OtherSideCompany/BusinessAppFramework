using System.Collections.ObjectModel;
using System.ComponentModel;

namespace OtherSideCore.Adapter.DomainObjectBrowser
{
   public interface IDomainObjectBrowserViewModel : IDisposable, INotifyPropertyChanged
   {
      bool HasUnsavedChanges { get; }
      public ObservableCollection<IDomainObjectEditorViewModel> DomainObjectEditorViewModels { get; }
      ObservableCollection<IDomainObjectBrowserViewModel> NestedDomainObjectBrowserViewModels { get; }
      IEnumerable<IDomainObjectBrowserViewModel> InlineNestedDomainObjectBrowserViewModels { get; }
      Task SaveChangesAsync();
      Task CancelChangesAsync();
      Task LoadEditorViewModelsAsync(IEnumerable<DomainObjectViewModel> domainObjectViewModels);
      void UnloadEditorViewModels(IEnumerable<DomainObjectViewModel> domainObjectViewModels);
      Task LoadNestedBrowsersAsync();
      Task SelectSearchResultViewModelAsync(DomainObjectViewModel domainObjectViewModel);
   }
}
