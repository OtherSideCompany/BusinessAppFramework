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

        public const string Modules = "api/navigation/modules";
        public const string Workspaces = $"api/navigation/modules/{{{ModuleKeyParam}}}/workspaces";
        public static string WorkspacesFor(string moduleKey) => Workspaces.Replace($"{{{ModuleKeyParam}}}", moduleKey);

        public const string Search = "api/search/[controller]";
        public const string PaginatedSearch = "api/search/[controller]/paginated";
        public const string SpecificSearch = $"api/search/[controller]/{{{DomainObjectIdParam}}}";
        public static string SearchFor(Type searchResultType) => Search.Replace("[controller]", searchResultType.Name.ToLowerInvariant());
        public static string PaginatedSearchFor(Type searchResultType) => PaginatedSearch.Replace("[controller]", searchResultType.Name.ToLowerInvariant());
        public static string SpecificSearchFor(Type searchResultType, int domainObjectId) => SpecificSearch.Replace("[controller]", searchResultType.Name.ToLowerInvariant())
                                                                                                           .Replace($"{{{DomainObjectIdParam}}}", domainObjectId.ToString());

        public const string Create = "api/create/[controller]";
        public static string CreateFor(Type domainObjectType) => Create.Replace("[controller]", domainObjectType.Name.ToLowerInvariant());
    }
}
