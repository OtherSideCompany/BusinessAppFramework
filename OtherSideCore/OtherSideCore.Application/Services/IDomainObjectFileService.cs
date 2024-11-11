using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Application.Services
{
   public interface IDomainObjectFileService
   {
      DirectoryInfo GetAssociatedDirectoryInfo(DomainObject domainObject);
      List<FileInfo> GetAssociatedFileInfos(DomainObject domainObject);
      List<DirectoryInfo> GetAssociatedNestedDirectoriesInfos(DomainObject domainObject);
      void CreateFolder(DomainObject domainObject);
      void OpenFolder(DomainObject domainObject);
      void OpenFolder(DirectoryInfo directoryInfo);
      void OpenFile(FileInfo fileInfo);
      void CopyFilesInAssociatedFolder(DomainObject domainObject, List<ManagedFile> managedFiles);
   }
}
