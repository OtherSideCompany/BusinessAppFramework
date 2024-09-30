using CommunityToolkit.Mvvm.Input;
using OtherSideCore.Domain.ModelObjects;
using OtherSideCore.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OtherSideCore.ViewModel
{
   public interface IRepositoryEditorViewModel : IDisposable
   {
      AsyncRelayCommand<bool> SearchCommandAsync { get; }
      RelayCommand CancelSearchCommand { get; }
      AsyncRelayCommand<ModelObjectViewModel> SelectSearchResultCommandAsync { get; }
      RelayCommand CancelSelectSearchResultCommand { get; }
      AsyncRelayCommand CreateAsyncCommand { get; }
      AsyncRelayCommand SaveSelectedSearchResultChangesAsyncCommand { get; }
      AsyncRelayCommand SaveDirtySearchResultChangesAsyncCommand { get; }
      AsyncRelayCommand CancelSelectedSearchResultChangesAsyncCommand { get; }
      AsyncRelayCommand CancelDirtySearchResultChangesAsyncCommand { get; }
      AsyncRelayCommand DeleteSelectedSearchResultAsyncCommand { get; }
      AsyncRelayCommand<ModelObject> DeleteAsyncCommand { get; }
      public Task CreateModelObjectAsync(ModelObject modelObject);
      void SetConstraints(List<Constraint> constraints);
      void ActivateConstraint(Constraint constraint);
   }
}
