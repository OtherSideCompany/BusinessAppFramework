using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Contracts.ApiRoutes;

namespace BusinessAppFramework.Application.Actions
{
    public class DocumentDownloadApplicationAction : DocumentGeneratorNavigationApplicationAction, IDocumentDownloadApplicationAction
    {        
        public override string BuildRoute()
        {
            return $"{ApiRouteSegments.Root}/{ApiRouteSegments.Documents}/{DocumentRouteSegments.DownloadPdf}/{DocumentKey.Key}/{DomainObjectId}";
        }
    }
}
