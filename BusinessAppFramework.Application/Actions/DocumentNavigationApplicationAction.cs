using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Domain;

namespace BusinessAppFramework.Application.Actions
{
    public class DocumentNavigationApplicationAction : IDocumentNavigationApplicationAction
    {
        #region Fields



        #endregion

        #region Properties
        public StringKey ActionKey { get; init; } = StringKey.Empty;
        public StringKey DocumentKey { get; set; } = StringKey.Empty;
        public string ExecuteRouteTemplate { get; init; } = string.Empty;
        public int DomainObjectId { get; set; }
        public bool RequireDomainObjectId { get; init; } = true;

        #endregion

        #region Events



        #endregion


        #region Constructor

        public DocumentNavigationApplicationAction()
        {

        }

        #endregion

        #region Public Methods

        public virtual string BuildRoute()
        {
            return $"/document/{DocumentKey}/{DomainObjectId}";
        }


        #endregion

        #region Private Methods



        #endregion
    }
}
