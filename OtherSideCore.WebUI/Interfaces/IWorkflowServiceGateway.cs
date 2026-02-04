using OtherSideCore.Application.Workflows;
using System;
using System.Collections.Generic;
using System.Text;

namespace OtherSideCore.WebUI.Interfaces
{
    public interface IWorkflowServiceGateway
    {
        Task<ProcessWorkflow?> GetAsync(string workflowKey, int domainObjectId, CancellationToken cancellationToken = default);
    }
}
