using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.ViewModel
{
   public interface IRepositoryManagerViewModel : IDisposable
   {
      AsyncRelayCommand SearchCommandAsync { get; }
      RelayCommand CancelSearchCommand { get; }
      AsyncRelayCommand<ModelObjectViewModel> SelectModelObjectCommandAsync { get; }
      RelayCommand CancelSelectModelObjectCommand { get; }
      AsyncRelayCommand CreateAsyncCommand { get; }
      AsyncRelayCommand SaveSelectedObjectChangesAsyncCommand { get; }
      AsyncRelayCommand CancelSelectedObjectChangesAsyncCommand { get; }
      AsyncRelayCommand DeleteSelectedObjectAsyncCommand { get; }
   }
}
