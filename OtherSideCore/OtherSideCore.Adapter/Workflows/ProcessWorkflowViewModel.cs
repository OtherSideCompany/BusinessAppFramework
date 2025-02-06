using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OtherSideCore.Application.Workflows;
using System.Collections.ObjectModel;

namespace OtherSideCore.Adapter.Workflows
{
   public class ProcessWorkflowViewModel : ObservableObject, IDisposable
   {
      #region Fields

      private ProcessWorkflow _processWorkflow;
      private ObservableCollection<ProcessWorkflowStepViewModel> _processWorkflowStepViewModels;

      #endregion

      #region Properties

      public ObservableCollection<ProcessWorkflowStepViewModel> ProcessWorkflowStepViewModels
      {
         get => _processWorkflowStepViewModels;
         set => SetProperty(ref _processWorkflowStepViewModels, value);
      }

      #endregion

      #region Commands

      public RelayCommand GoNextCommand { get; private set; }
      public RelayCommand GoBackCommand { get; private set; }
      public RelayCommand<ProcessWorkflowStepViewModel> GoToStepCommand { get; private set; }

      #endregion

      #region Constructor

      public ProcessWorkflowViewModel(ProcessWorkflow processWorkflow)
      {
         _processWorkflow = processWorkflow;

         ProcessWorkflowStepViewModels = new ObservableCollection<ProcessWorkflowStepViewModel>();

         foreach (var step in processWorkflow.ProcessWorkflowSteps)
         {
            ProcessWorkflowStepViewModels.Add(new ProcessWorkflowStepViewModel(step));
         }

         GoNextCommand = new RelayCommand(GoNext, CanGoNext);
         GoBackCommand = new RelayCommand(GoBack, CanGoBack);
         GoToStepCommand = new RelayCommand<ProcessWorkflowStepViewModel>(GoToStep, CanGoToStep);
      }


      #endregion

      #region Public Methods

      public void Initialize()
      {
         RefreshWorkflowState();

         var initialStep = ProcessWorkflowStepViewModels.LastOrDefault(vm => vm.ProcessWorkflowStep.IsExecutable() && vm.IsPreviousStepCompleted);

         GoToStep(initialStep != null ? initialStep : ProcessWorkflowStepViewModels.First());
      }

      public void RefreshWorkflowState()
      {
         foreach (var processWorkflowStepViewModel in ProcessWorkflowStepViewModels)
         {
            var previousStepViewModel = ProcessWorkflowStepViewModels.ElementAtOrDefault(ProcessWorkflowStepViewModels.IndexOf(processWorkflowStepViewModel) - 1);

            processWorkflowStepViewModel.RefreshState(previousStepViewModel);
         }

         NotifyCommandCanExecuteChanged();
      }

      public virtual void Dispose()
      {

      }

      #endregion

      #region Private Methods

      private void NotifyCommandCanExecuteChanged()
      {
         GoNextCommand.NotifyCanExecuteChanged();
         GoBackCommand.NotifyCanExecuteChanged();
         GoToStepCommand.NotifyCanExecuteChanged();
      }

      private bool CanGoBack()
      {
         return true;
      }

      private void GoBack()
      {

      }

      private bool CanGoNext()
      {
         return true;
      }

      private void GoNext()
      {

      }

      private bool CanGoToStep(ProcessWorkflowStepViewModel? processWorkflowStepViewModel)
      {
         return processWorkflowStepViewModel != null && processWorkflowStepViewModel.IsExecutable;
      }

      private void GoToStep(ProcessWorkflowStepViewModel? processWorkflowStepViewModel)
      {
         ProcessWorkflowStepViewModels.ToList().ForEach(vm => vm.IsSelected = false);
         processWorkflowStepViewModel.IsSelected = true;
      }

      #endregion
   }
}
