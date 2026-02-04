using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Application.Services
{
   public interface IDomainObjectFileService
   {
      DirectoryInfo GetAssociatedDirectoryInfo(int domainObjectId);
      List<FileInfo> GetAssociatedFileInfos(int domainObjectId);
      List<DirectoryInfo> GetAssociatedNestedDirectoriesInfos(int domainObjectId);
      void CreateFolder(int domainObjectId);
      void OpenFolder(int domainObjectId);
      void OpenFolder(DirectoryInfo directoryInfo);
      void OpenFile(FileInfo fileInfo);
      void CopyFilesInAssociatedFolder(int domainObjectId, List<ManagedFile> managedFiles);
      void TryDeleteAssociatedFolder(int domainObjectId);
   }
}
