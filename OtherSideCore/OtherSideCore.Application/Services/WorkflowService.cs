using OtherSideCore.Application.Interfaces;
using OtherSideCore.Application.Workflows;
using OtherSideCore.Domain;

namespace OtherSideCore.Application.Services
{
    public class WorkflowService : IWorkflowService
    {
        #region Fields

        private readonly IWorkflowFactory _workflowFactory;

        #endregion

        #region Properties



        #endregion

        #region Events



        #endregion

        #region Constructor

        public WorkflowService(IWorkflowFactory workflowFactory)
        {
            _workflowFactory = workflowFactory;
        }

        #endregion

        #region Public Methods

        public async Task<ProcessWorkflow> GetWorkflowAsync(string workflowKey, int domainObjectId)
        {
            var workflow = _workflowFactory.CreateWorkflow(StringKey.From(workflowKey));

            foreach (var step in workflow.ProcessWorkflowSteps)
            {
                foreach (var condition in step.ProcessWorkflowStepConditions)
                {
                    condition.IsCompleted = await _conditionService.EvaluateAsync(condition.Key, domainObjectId);
                }
            }

            return workflow;
        }

        #endregion

        #region Private Methods



        #endregion
    }
}
