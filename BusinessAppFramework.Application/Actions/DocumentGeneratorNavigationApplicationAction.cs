using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Contracts.ApiRoutes;
using BusinessAppFramework.Domain;

namespace BusinessAppFramework.Application.Actions
{
    public class DocumentGeneratorNavigationApplicationAction : IDocumentNavigationApplicationAction
    {
        #region Fields



        #endregion

        #region Properties
        public StringKey ActionKey { get; init; } = StringKey.Empty;
        public StringKey DocumentKey { get; set; } = StringKey.Empty;
        public string ExecuteRoute { get; init; } = string.Empty;
        public int DomainObjectId { get; set; }
        public bool RequireDomainObjectId { get; init; } = true;

        #endregion

        #region Events



        #endregion

        #region Constructor

        public DocumentGeneratorNavigationApplicationAction()
        {

        }

        #endregion

        #region Public Methods

        public virtual string BuildRoute()
        {
            return $"/{ApiRouteSegments.DocumentGenerator}/{DocumentKey}/{DomainObjectId}";
        }


        #endregion

        #region Private Methods



        #endregion
    }
}
