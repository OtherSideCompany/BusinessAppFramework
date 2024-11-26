using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OtherSideCore.Adapter.DomainObjectBrowser
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
