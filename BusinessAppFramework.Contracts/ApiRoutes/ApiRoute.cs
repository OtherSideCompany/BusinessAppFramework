using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessAppFramework.Contracts.ApiRoutes
{
    public static class ApiRoute
    {
        public static string BaseRoute(string route) =>
            $"{ApiRouteSegments.Root}/{route}";

        public static string DomainObjectControllerRoute<T>() =>
            $"{ApiRouteSegments.Root}/{ApiRouteSegments.DomainObjects}/{DomainObjectAggregateKeys<T>.Type}";

        public static string DomainObjectControllerRoute(Type domainObjectType) =>
            $"{ApiRouteSegments.Root}/{ApiRouteSegments.DomainObjects}/{DomainObjectAggregateKeys.Type(domainObjectType)}";

        public static string SearchControllerRoute<T>() =>
            $"{ApiRouteSegments.Root}/{ApiRouteSegments.Search}/{DomainObjectAggregateKeys<T>.Type}";

        public static string SearchControllerRoute(string routeKey) =>
            $"{ApiRouteSegments.Root}/{ApiRouteSegments.Search}/{routeKey}";
    }
}
