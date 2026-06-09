using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessAppFramework.Application.Interfaces
{
    public interface IReferenceNavigationKeyRegistry
    {
        string GetNavigationKey(string referenceKey);
        string? TryGetNavigationKey(string referenceKey);
        void RegisterNavigationKey(string referenceKey, string navigationKey);
    }
}
