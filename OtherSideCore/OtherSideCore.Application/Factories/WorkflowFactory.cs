using OtherSideCore.Application.Interfaces;
using OtherSideCore.Application.Trees;
using OtherSideCore.Application.Workflows;
using OtherSideCore.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace OtherSideCore.Application.Factories
{
    public class WorkflowFactory : StringKeyBasedFactory, IWorkflowFactory
    {
        #region Fields



        #endregion

        #region Properties



        #endregion

        #region Events



        #endregion

        #region Constructor

        public WorkflowFactory()
        {

        }

        #endregion

        #region Public Methods

        public ProcessWorkflow CreateWorkflow(StringKey key)
        {
            return (ProcessWorkflow)Create(key);
        }

        public void RegisterWorkflow(StringKey key, Func<ProcessWorkflow> tree)
        {
            Register(key, tree);
        }

        #endregion

        #region Private Methods



        #endregion
    }
}
