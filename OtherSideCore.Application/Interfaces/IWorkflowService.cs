using OtherSideCore.Application.Workflows;
using System;
using System.Collections.Generic;
using System.Text;

namespace OtherSideCore.Application.Interfaces
{
    public interface IWorkflowService
    {
        Task<ProcessWorkflow> GetWorkflowAsync(string workflowKey, int domainObjectId);
    }
}
