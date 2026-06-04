using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessAppFramework.Contracts.ApiRoutes
{
    public static class ApiRoute
    {
        public static string DomainObjectControllerRoute(string routeKey) =>
            $"{ApiRouteSegments.Root}/{ApiRouteSegments.DomainObjects}/{routeKey}";

        public static string DomainObjectDocumentControllerRoute(string routeKey) =>
            $"{ApiRouteSegments.Root}/{ApiRouteSegments.Documents}/{routeKey}";

        public static string SearchControllerRoute(string routeKey) =>
            $"{ApiRouteSegments.Root}/{ApiRouteSegments.Search}/{routeKey}";
    }
}
