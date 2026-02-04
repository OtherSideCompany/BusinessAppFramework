using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OtherSideCore.Application.Interfaces;
using OtherSideCore.Application.Workflows;
using OtherSideCore.Contracts;

namespace OtherSideCore.Adapter.Web.Controllers
{
    [ApiController]
    [Authorize]
    public class WorkflowController : ControllerBase
    {
        #region Fields

        private IWorkflowService _workflowService;

        #endregion

        #region Properties



        #endregion

        #region Events



        #endregion

        #region Constructor

        public WorkflowController(IWorkflowService workflowService)
        {
            _workflowService = workflowService;
        }

        #endregion

        #region Public Methods

        [HttpGet(Routes.GetWorkflowTemplate)]
        public virtual async Task<ActionResult<ProcessWorkflow>> GetAsync(string key, int domainObjectId)
        {
            var workflow = await _workflowService.GetWorkflowAsync(key, domainObjectId);
            return Ok(workflow);
        }

        #endregion

        #region Private Methods



        #endregion
    }
}
