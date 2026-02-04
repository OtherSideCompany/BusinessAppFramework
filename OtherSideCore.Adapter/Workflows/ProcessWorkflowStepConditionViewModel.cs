using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Identity.Client;
using OtherSideCore.Application.Workflows;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Adapter.Workflows
{
   public class ProcessWorkflowStepConditionViewModel : ObservableObject
   {
      #region Fields

      ProcessWorkflowStepCondition _processWorkflowStepCondition;

      private bool _isConditionVerified;

      #endregion

      #region Properties

      public ProcessWorkflowStepCondition ProcessWorkflowStepCondition
      {
         get => _processWorkflowStepCondition;
         set => SetProperty(ref _processWorkflowStepCondition, value);
      }

      public bool IsConditionVerified
      {
         get => _isConditionVerified;
         set => SetProperty(ref _isConditionVerified, value);
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public ProcessWorkflowStepConditionViewModel(ProcessWorkflowStepCondition processWorkflowStepCondition)
      {
         _processWorkflowStepCondition = processWorkflowStepCondition;
      }

      #endregion

      #region Public Methods

      public void RefreshConditionVerified()
      {
         IsConditionVerified = ProcessWorkflowStepCondition.Evaluate();
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
