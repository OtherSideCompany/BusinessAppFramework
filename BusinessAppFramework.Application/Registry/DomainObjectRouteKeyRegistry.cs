using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Application.Search;
using BusinessAppFramework.Domain.DomainObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessAppFramework.Application.Registry
{
    public class DomainObjectRouteKeyRegistry : Registry<Type, string>, IDomainObjectRouteKeyRegistry
    {
        public string GetRouteKey<TDomainObject>() where TDomainObject : DomainObject
        {
            return Resolve(typeof(TDomainObject));
        }

        public void RegisterRouteKey<TDomainObject>(string routeKey) where TDomainObject : DomainObject
        {
            Register(typeof(TDomainObject), routeKey);
        }
    }
}
