using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.ViewModel
{
   public interface IRepositoryEditorViewModel : IDisposable
   {
      AsyncRelayCommand SearchCommandAsync { get; }
      RelayCommand CancelSearchCommand { get; }
      AsyncRelayCommand<ModelObjectViewModel> SelectSearchResultCommandAsync { get; }
      RelayCommand CancelSelectSearchResultCommand { get; }
      AsyncRelayCommand CreateAsyncCommand { get; }
      AsyncRelayCommand SaveSelectedSearchResultChangesAsyncCommand { get; }
      AsyncRelayCommand CancelSelectedSearchResultChangesAsyncCommand { get; }
      AsyncRelayCommand DeleteSelectedSearchResultAsyncCommand { get; }
   }
}
