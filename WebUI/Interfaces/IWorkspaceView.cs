using Application.Descriptors;

namespace WebUI.Interfaces
{
   public interface IWorkspaceView<TDescriptor> where TDescriptor : WorkspaceDescriptor
   {
      TDescriptor Descriptor { get; set; }
   }
}
