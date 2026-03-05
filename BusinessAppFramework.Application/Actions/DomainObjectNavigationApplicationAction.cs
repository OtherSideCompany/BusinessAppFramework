using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Contracts.ApiRoutes;
using BusinessAppFramework.Domain.DomainObjects;

namespace BusinessAppFramework.Application.Actions
{
   public class DomainObjectNavigationApplicationAction<TDomainObject> : DomainObjectApplicationAction<TDomainObject>, IDomainObjectNavigationApplicationAction where TDomainObject : DomainObject, new()
   {
      #region Fields

      private IDomainObjectPageWorkspaceKeyRegistry _domainObjectPageWorkspaceKeyResolver;

      #endregion

      #region Properties



      #endregion

      #region Events



      #endregion

      #region Constructor

      public DomainObjectNavigationApplicationAction(IDomainObjectPageWorkspaceKeyRegistry domainObjectPageWorkspaceKeyResolver)
      {
         _domainObjectPageWorkspaceKeyResolver = domainObjectPageWorkspaceKeyResolver;
      }

      #endregion

      #region Public Methods

      public override string BuildRoute()
      {
         return $"/{ApiRouteSegments.Workspace}/{_domainObjectPageWorkspaceKeyResolver.GetPageWorkspaceKey<TDomainObject>()}?id={DomainObjectId}";
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
