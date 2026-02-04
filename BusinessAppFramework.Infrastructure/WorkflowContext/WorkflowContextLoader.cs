using BusinessAppFramework.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace BusinessAppFramework.Infrastructure.WorkflowContext
{
   public abstract class WorkflowContextLoader : IWorkflowContextLoader
   {
      #region Fields

      protected readonly IDbContextFactory<DbContext> _dbContextFactory;

      #endregion

      #region Properties



      #endregion

      #region Events



      #endregion

      #region Constructor

      public WorkflowContextLoader(IDbContextFactory<DbContext> dbContextFactory)
      {
         _dbContextFactory = dbContextFactory;
      }

      #endregion

      #region Public Methods

      public abstract System.Threading.Tasks.Task<Application.Workflows.WorkflowContext> LoadAsync(int domainObjectId, CancellationToken cancellationToken = default);

      #endregion

      #region Private Methods



      #endregion
   }
}
