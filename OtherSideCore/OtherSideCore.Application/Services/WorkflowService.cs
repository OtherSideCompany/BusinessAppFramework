using OtherSideCore.Application.Interfaces;
using OtherSideCore.Application.Workflows;
using OtherSideCore.Domain;

namespace OtherSideCore.Application.Services
{
    public class WorkflowService : IWorkflowService
    {
        #region Fields

        private readonly IWorkflowFactory _workflowFactory;
        private readonly IWorkflowContextLoaderFactory _workflowContextLoaderFactory;

        #endregion

        #region Properties



        #endregion

        #region Events



        #endregion

        #region Constructor

        public WorkflowService(IWorkflowFactory workflowFactory, IWorkflowContextLoaderFactory workflowContextLoaderFactory)
        {
            _workflowFactory = workflowFactory;
            _workflowContextLoaderFactory = workflowContextLoaderFactory;
        }

        #endregion

        #region Public Methods

        public async Task<ProcessWorkflow> GetWorkflowAsync(string workflowKey, int domainObjectId)
        {
            var workflow = _workflowFactory.CreateWorkflow(StringKey.From(workflowKey));
            var workflowContextLoader = _workflowContextLoaderFactory.Get(StringKey.From(workflowKey));
            var context = await workflowContextLoader.LoadAsync(domainObjectId);
            workflow.EvaluateWorkflowSate(context);
            return workflow;
        }

        #endregion

        #region Private Methods



        #endregion
    }
}
