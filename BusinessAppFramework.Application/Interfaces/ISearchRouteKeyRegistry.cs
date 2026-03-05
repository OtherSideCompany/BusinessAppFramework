using BusinessAppFramework.Application.Search;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessAppFramework.Application.Interfaces
{
    public interface ISearchRouteKeyRegistry
    {
        void RegisterRouteKey<TSearchResult>(string routeKey) where TSearchResult : DomainObjectSearchResult;
        string GetRouteKey<TSearchResult>() where TSearchResult : DomainObjectSearchResult;
    }
}
