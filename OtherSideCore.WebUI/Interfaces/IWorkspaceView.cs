using OtherSideCore.Application.Descriptors;
using System;
using System.Collections.Generic;
using System.Text;

namespace OtherSideCore.WebUI.Interfaces
{
    public interface IWorkspaceView<TDescriptor> where TDescriptor : WorkspaceDescriptor
    {
        TDescriptor Descriptor { get; set; }
    }
}
