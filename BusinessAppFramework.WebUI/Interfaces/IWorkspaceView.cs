using BusinessAppFramework.Application.Descriptors;

namespace BusinessAppFramework.WebUI.Interfaces
{
   public interface IWorkspaceView<TDescriptor> where TDescriptor : WorkspaceDescriptor
   {
      TDescriptor Descriptor { get; set; }
   }
}
