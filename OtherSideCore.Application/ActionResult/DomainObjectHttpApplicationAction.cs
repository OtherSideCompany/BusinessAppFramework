using OtherSideCore.Application.Interfaces;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Application.ActionResult
{
    public class DomainObjectHttpApplicationAction<TDomainObject> : DomainObjectApplicationAction<TDomainObject>, IHttpDomainObjectApplicationAction where TDomainObject : DomainObject, new()
    {
        #region Fields



        #endregion

        #region Properties

        public HttpMethod HttpMethod { get; set; } = HttpMethod.Post;        

        #endregion

        #region Events



        #endregion

        #region Constructor

        public DomainObjectHttpApplicationAction()
        {

        }

        #endregion

        #region Public Methods

        

        #endregion

        #region Private Methods



        #endregion
    }
}
