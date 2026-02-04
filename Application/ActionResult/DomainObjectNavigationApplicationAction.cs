using Application.Interfaces;
using Domain.DomainObjects;

namespace Application.ActionResult
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
         return $"/workspace/{_domainObjectPageWorkspaceKeyResolver.GetPageWorkspaceKey<TDomainObject>()}?id={DomainObjectId}";
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
