using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Contracts.ApiRoutes;
using BusinessAppFramework.Domain.DomainObjects;

namespace BusinessAppFramework.Application.Actions
{
   public abstract class DomainObjectApplicationAction<TDomainObject> : ApplicationAction, IDomainObjectApplicationAction where TDomainObject : DomainObject, new()
   {
      #region Fields



      #endregion

      #region Properties

      public int DomainObjectId { get; set; }

      #endregion

      #region Events



      #endregion

      #region Constructor

      public DomainObjectApplicationAction()
      {

      }

      #endregion

      #region Public Methods

      public override string BuildRoute()
      {
            return ExecuteRoute.Replace(ApiRouteParams.DomainObjectId, DomainObjectId.ToString());
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
