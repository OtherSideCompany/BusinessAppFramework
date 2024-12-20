using CommunityToolkit.Mvvm.ComponentModel;
using OtherSideCore.Application.Workflows;

namespace OtherSideCore.Adapter.Workflows
{
   public class ProcessWorkflowStepViewModel : ObservableObject
   {
      #region Fields

      private ProcessWorkflowStep _processWorkflowStep;
      private bool _isSelected;
      private bool _isEnabled;
      private bool _isExecutable;
      private bool _isCompleted;
      private bool _isPreviousStepCompleted;
      private bool _isNextStepCompleted;

      #endregion

      #region Properties

      public bool IsSelected
      {
         get => _isSelected;
         set => SetProperty(ref _isSelected, value);
      }

      public bool IsEnabled
      {
         get => _isEnabled;
         set => SetProperty(ref _isEnabled, value);
      }

      public bool IsExecutable
      {
         get => _isExecutable;
         set => SetProperty(ref _isExecutable, value);
      }

      public bool IsCompleted
      {
         get => _isCompleted;
         set => SetProperty(ref _isCompleted, value);
      }

      public bool IsPreviousStepCompleted
      {
         get => _isPreviousStepCompleted;
         set => SetProperty(ref _isPreviousStepCompleted, value);
      }

      public ProcessWorkflowStep ProcessWorkflowStep
      {
         get => _processWorkflowStep;
         set => SetProperty(ref _processWorkflowStep, value);
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public ProcessWorkflowStepViewModel(ProcessWorkflowStep processWorkflowStep)
      {
         _processWorkflowStep = processWorkflowStep;
      }

      #endregion

      #region Public Methods
      
      public void RefreshState(ProcessWorkflowStepViewModel previousProcessWorkflowStepViewModel)
      {
         IsExecutable = _processWorkflowStep.IsExecutable();
         IsCompleted = _processWorkflowStep.IsCompleted();

         IsPreviousStepCompleted = previousProcessWorkflowStepViewModel == null ? false : previousProcessWorkflowStepViewModel.ProcessWorkflowStep.IsCompleted();
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
