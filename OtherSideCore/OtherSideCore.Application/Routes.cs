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

        public const string ModulesTemplate = "api/navigation/modules";
        public const string WorkspacesTemplate = $"api/navigation/modules/{{{ModuleKeyParam}}}/workspaces";       

        public const string CountTemplate = "api/count/[controller]";
        public const string SearchTemplate = "api/search/[controller]";
        public const string PaginatedSearchTemplate = "api/search/[controller]/paginated";
        public const string SpecificSearchTemplate = $"api/search/[controller]/{{{DomainObjectIdParam}}}";

        public const string CreateTemplate = "api/create/[controller]";
        public const string GetTemplate = $"api/get/[controller]/{{{DomainObjectIdParam}}}";
        public const string SaveTemplate = $"api/save/[controller]/{{{DomainObjectIdParam}}}";
        public const string DeleteTemplate = $"api/delete/[controller]/{{{DomainObjectIdParam}}}";

        public static string WorkspacesFor(string moduleKey) => WorkspacesTemplate.Replace($"{{{ModuleKeyParam}}}", moduleKey);
        public static string For(string template, Type domainObjectType, int? id)
        {
            var route = template.Replace("[controller]", domainObjectType.Name.ToLowerInvariant());

            if (id != null)
            {
                route = route.Replace($"{{{DomainObjectIdParam}}}", id.ToString());
            }

            return route;
        }
    }
}
