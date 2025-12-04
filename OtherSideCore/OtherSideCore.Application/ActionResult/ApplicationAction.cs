using OtherSideCore.Application.Interfaces;
using OtherSideCore.Domain;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Application.ActionResult
{
    public class ApplicationAction<TDomainObject> : IApplicationAction where TDomainObject : DomainObject, new()
    {
        #region Fields



        #endregion

        #region Properties

        public StringKey ActionKey { get; init; } = StringKey.Empty;
        public string ExecuteRouteTemplate { get; init; } = string.Empty;        
        public HttpMethod HttpMethod { get; set; } = HttpMethod.Post;
        public int? DomainObjectId { get; set; }

        #endregion

        #region Events



        #endregion

        #region Constructor

        public ApplicationAction()
        {

        }

        #endregion

        #region Public Methods

        public string BuildRoute()
        {
            return Routes.For(ExecuteRouteTemplate, typeof(TDomainObject), DomainObjectId);
        }

        #endregion

        #region Private Methods



        #endregion
    }
}
