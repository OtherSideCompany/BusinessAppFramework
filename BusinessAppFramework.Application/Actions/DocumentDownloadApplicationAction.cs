using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Contracts;

namespace BusinessAppFramework.Application.Actions
{
    public class DocumentDownloadApplicationAction : DocumentNavigationApplicationAction, IDocumentDownloadApplicationAction
    {        
        public override string BuildRoute()
        {
            var route = Routes.BuildRoute(Routes.DownloadPdfDocumentTemplate, DomainObjectId, DocumentKey.Key);

            return route;
        }
    }
}
