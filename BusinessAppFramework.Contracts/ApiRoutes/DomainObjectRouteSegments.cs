using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessAppFramework.Contracts.ApiRoutes
{
    public static class DomainObjectRouteSegments
    {
        public const string Create = "create";
        public const string Get = $"get";
        public const string GetHydrated = $"get-hydrated";
        public const string GetHydratedReference = $"get-hydrated-reference";
        public const string GetHydratedReferenceList = $"get-hydrated-reference-list";
        public const string Save = $"save";
        public const string Delete = $"delete";
        public const string GetChildren = $"get-children";
    }
}
