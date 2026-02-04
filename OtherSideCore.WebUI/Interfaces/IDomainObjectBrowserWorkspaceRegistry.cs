using OtherSideCore.Domain;
using OtherSideCore.Domain.DomainObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace OtherSideCore.WebUI.Interfaces
{
    public interface IDomainObjectBrowserWorkspaceRegistry
    {
        void Register<T>(StringKey browserKey) where T : DomainObject;
        StringKey Resolve<T>() where T : DomainObject;
        bool TryResolve<T>(out StringKey browserKey) where T : DomainObject;
    }
}
