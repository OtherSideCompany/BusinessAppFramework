
using OtherSideCore.Application.Interfaces;
using OtherSideCore.Domain;

namespace OtherSideCore.Application.Factories
{
    public class WorkflowContextLoaderFactory : StringKeyBasedFactory, IWorkflowContextLoaderFactory
    {
        #region Fields



        #endregion

        #region Properties



        #endregion

        #region Events



        #endregion

        #region Constructor

        public WorkflowContextLoaderFactory()
        {

        }

        #endregion

        #region Public Methods

        public IWorkflowContextLoader Get(StringKey workflowKey)
        {
            return (IWorkflowContextLoader)Create(workflowKey);
        }

        public void RegisterWorkflowContextLoader(StringKey workflowKey, Func<IWorkflowContextLoader> loader)
        {
            Register(workflowKey, loader);
        }

        #endregion

        #region Private Methods



        #endregion
    }
}
