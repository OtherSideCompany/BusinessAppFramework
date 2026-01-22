using OtherSideCore.Application.Workflows;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OtherSideCore.Application.Interfaces
{
    public interface IWorkflowContextLoader
    {
        Task<WorkflowContext> LoadAsync(int domainObjectId, CancellationToken cancellationToken = default);
    }
}
