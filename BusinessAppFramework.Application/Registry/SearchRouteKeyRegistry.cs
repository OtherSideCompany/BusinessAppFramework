using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Application.Search;

namespace BusinessAppFramework.Application.Registry
{
    public class SearchRouteKeyRegistry : Registry<Type, string>, ISearchRouteKeyRegistry
    {
        public string GetRouteKey<TSearchResult>() where TSearchResult : DomainObjectSearchResult
        {
            return Resolve(typeof(TSearchResult));
        }

        public void RegisterRouteKey<TSearchResult>(string routeKey) where TSearchResult : DomainObjectSearchResult
        {
            Register(typeof(TSearchResult), routeKey);
        }
    }
}
