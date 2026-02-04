
using OtherSideCore.Application.Services;

namespace OtherSideCore.Adapter
{
   public interface IFileDropZone
   {
      void DropFiles(List<ManagedFile> managedFiles);
   }
}
