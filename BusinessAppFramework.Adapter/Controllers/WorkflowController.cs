using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Application.Workflows;
using BusinessAppFramework.Contracts.ApiRoutes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusinessAppFramework.Adapter.Controllers
{
    [ApiController]
    [Authorize]
    [Route($"{ApiRouteSegments.Root}/{ApiRouteSegments.Workflow}")]
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

        [HttpGet($"{WorkflowRouteSegments.GetWorkflow}/{{{ApiRouteParams.Key}}}/{{{ApiRouteParams.DomainObjectId}:int}}")]
        public virtual async Task<ActionResult<ProcessWorkflow>> GetAsync(
            [FromRoute(Name = ApiRouteParams.Key)] string key,
            [FromRoute(Name = ApiRouteParams.DomainObjectId)] int domainObjectId)
        {
            var workflow = await _workflowService.GetWorkflowAsync(key, domainObjectId);
            return Ok(workflow);
        }

        #endregion

        #region Private Methods



        #endregion
    }
}
