using OtherSideCore.Application.Search;
using System;
using System.Collections.Generic;
using System.Text;

namespace OtherSideCore.Application
{
    public static class Routes
    {
        public const string ModuleKeyParam = "moduleKey";
        public const string DomainObjectIdParam = "domainObjectId";
        public const string ParentDomainObjectIdParam = "parentDomainObjectId";
        public const string KeyParam = "key";

        public const string ModulesTemplate = "api/navigation/modules";
        public const string WorkspacesTemplate = $"api/navigation/modules/{{{ModuleKeyParam}}}/workspaces";
        public static string WorkspacesFor(string moduleKey) => WorkspacesTemplate.Replace($"{{{ModuleKeyParam}}}", moduleKey);

        public const string CountTemplate = "api/count/[controller]";
        public const string SearchTemplate = "api/search/[controller]";
        public const string PaginatedSearchTemplate = "api/search/[controller]/paginated";
        public const string SpecificSearchTemplate = $"api/search/[controller]/{{{DomainObjectIdParam}}}";

        public const string CreateTemplate = "api/create/[controller]";
        public const string GetTemplate = $"api/get/[controller]/{{{DomainObjectIdParam}}}";
        public const string GetChildrenTemplate = $"api/getchildren/[controller]/{{{DomainObjectIdParam}}}/{{{KeyParam}}}";
        public const string GetHydratedTemplate = $"api/get-hydrated/[controller]/{{{DomainObjectIdParam}}}";
        public const string GetHydratedDomainObjectReferenceTemplate = $"api/get-hydrated-domainobject-reference/[controller]/{{{DomainObjectIdParam}}}/{{{KeyParam}}}";
        public const string GetHydratedDomainObjectReferenceListItemTemplate = $"api/get-hydrated-domainobject-reference-list-item/[controller]/{{{DomainObjectIdParam}}}/{{{KeyParam}}}";
        public const string GetSummaryTemplate = $"api/[controller]/get-summary/{{{DomainObjectIdParam}}}";
        public const string SaveTemplate = $"api/save/[controller]/{{{DomainObjectIdParam}}}";
        public const string DeleteTemplate = $"api/delete/[controller]/{{{DomainObjectIdParam}}}";
        public const string SetParentTemplate = $"api/setparent/{{{ParentDomainObjectIdParam}}}/{{{DomainObjectIdParam}}}/{{{KeyParam}}}";

        public const string CreateTreeNodeTemplate = $"api/create-treenode/{{{ParentDomainObjectIdParam}}}/{{{KeyParam}}}";
        public const string DeleteTreeNodeTemplate = $"api/delete-treenode/{{{ParentDomainObjectIdParam}}}/{{{DomainObjectIdParam}}}/{{{KeyParam}}}";
        public const string GetTreeTemplate = $"api/get-tree/{{{DomainObjectIdParam}}}/{{{KeyParam}}}";

        public static string BuildRoute(string template, int domainObjectId, string key)
        {
            return template.Replace($"{{{DomainObjectIdParam}}}", domainObjectId.ToString())
                           .Replace($"{{{KeyParam}}}", key);
        }

        public static string BuildRouteFromParent(string template, int parentDomainObjectId, string key)
        {
            return template.Replace($"{{{ParentDomainObjectIdParam}}}", parentDomainObjectId.ToString())
                           .Replace($"{{{KeyParam}}}", key);
        }

        public static string BuildRoute(string template, int parentId, int childId, string key)
        {
            return template.Replace($"{{{ParentDomainObjectIdParam}}}", parentId.ToString())
                           .Replace($"{{{DomainObjectIdParam}}}", childId.ToString())
                           .Replace($"{{{KeyParam}}}", key.ToString());
        }

        public static string BuildRoute(string template, Type domainObjectType, int? id = null, string? key = null)
        {
            var route = template.Replace("[controller]", domainObjectType.Name.ToLowerInvariant());

            if (id != null)
            {
                route = route.Replace($"{{{DomainObjectIdParam}}}", id.ToString());
            }

            if (key != null)
            {
                route = route.Replace($"{{{KeyParam}}}", key.ToString());
            }

            return route;
        }
    }
}
