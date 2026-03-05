using BusinessAppFramework.Application.Search;
using BusinessAppFramework.Domain.DomainObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessAppFramework.Application.Interfaces
{
    public interface IDomainObjectRouteKeyRegistry
    {
        void RegisterRouteKey<TDomainObject>(string routeKey) where TDomainObject : DomainObject;
        string GetRouteKey<TDomainObject>() where TDomainObject : DomainObject;
    }
}
