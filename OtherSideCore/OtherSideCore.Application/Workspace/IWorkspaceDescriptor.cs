using OtherSideCore.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace OtherSideCore.Application.Workspace
{
    public interface IWorkspaceDescriptor
    {
        StringKey WorkspaceKey { get; }
        Type ComponentType { get; }
    }
}
