using OtherSideCore.Application.Interfaces;
using OtherSideCore.Domain;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Application.ActionResult
{
    public abstract class DomainObjectApplicationAction<TDomainObject> : IDomainObjectApplicationAction where TDomainObject : DomainObject, new()
    {
        #region Fields



        #endregion

        #region Properties

        public StringKey ActionKey { get; init; } = StringKey.Empty;
        public string ExecuteRouteTemplate { get; init; } = string.Empty;
        public int? DomainObjectId { get; set; }

        #endregion

        #region Events



        #endregion

        #region Constructor

        public DomainObjectApplicationAction()
        {

        }

        #endregion

        #region Public Methods

        public virtual string BuildRoute()
        {
            return Routes.For(ExecuteRouteTemplate, typeof(TDomainObject), DomainObjectId);
        }

        #endregion

        #region Private Methods



        #endregion
    }
}
