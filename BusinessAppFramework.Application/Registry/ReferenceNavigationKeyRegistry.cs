using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Domain.DomainObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessAppFramework.Application.Registry
{
    public class ReferenceNavigationKeyRegistry : Registry<string, string>, IReferenceNavigationKeyRegistry
    {
        public string GetNavigationKey(string referenceKey)
        {
            return Resolve(referenceKey);
        }

        public string? TryGetNavigationKey(string referenceKey)
        {
            TryResolve(referenceKey, out string? navigationKey);
            return navigationKey;
        }

        public void RegisterNavigationKey(string referenceKey, string navigationKey)
        {
            Register(referenceKey, navigationKey);
        }
    }
}
