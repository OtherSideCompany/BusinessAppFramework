using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessAppFramework.Contracts.ApiRoutes
{
    public static class RelationshipSegments
    {
        public const string SetParent = "set-parent";
        public const string GetHydratedReference = "get-hydrated-reference";
        public const string GetHydratedReferenceListItem = $"get-hydrated-reference-list-item";
        public const string GetChildrenIds = "get-children-ids";
        public const string GetChildren = "get-children";
    }
}
