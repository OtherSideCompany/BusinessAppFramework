using OtherSideCore.Application.Trees;
using OtherSideCore.Application.Workflows;
using OtherSideCore.Domain;

namespace OtherSideCore.Application.Interfaces
{
    public interface IWorkflowFactory
    {
        void RegisterWorkflow(StringKey key, Func<ProcessWorkflow> tree);
        ProcessWorkflow CreateWorkflow(StringKey key);
    }
}
