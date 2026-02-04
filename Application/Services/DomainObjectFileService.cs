using Application.AppConfiguration;
using System.Diagnostics;

namespace Application.Services
{
   public abstract class DomainObjectFileService : IDomainObjectFileService
   {
      #region Fields

      protected IAppConfiguration _appConfiguration;
      protected IImageCompressionService _imageCompressionService;

      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectFileService(IAppConfiguration appConfiguration, IImageCompressionService imageCompressionService)
      {
         _appConfiguration = appConfiguration;
         _imageCompressionService = imageCompressionService;
      }

      #endregion

      #region Public Methods

      public List<FileInfo> GetAssociatedFileInfos(int domainObjectId)
      {
         var fileInfos = new List<FileInfo>();
         var directoryInfo = GetAssociatedDirectoryInfo(domainObjectId);

         if (Directory.Exists(directoryInfo.FullName))
         {
            var fileNames = Directory.GetFiles(directoryInfo.FullName);
            fileInfos = fileNames.Select(fileName => new FileInfo(fileName)).ToList();
         }

         return fileInfos.OrderBy(f => f.Name).ToList();
      }

      public List<DirectoryInfo> GetAssociatedNestedDirectoriesInfos(int domainObjectId)
      {
         var directoryInfos = new List<DirectoryInfo>();
         var directoryInfo = GetAssociatedDirectoryInfo(domainObjectId);

         if (Directory.Exists(directoryInfo.FullName))
         {
            var directoryNames = Directory.GetDirectories(directoryInfo.FullName);
            directoryInfos = directoryNames.Select(directoryName => new DirectoryInfo(directoryName)).ToList();
         }

         return directoryInfos.OrderBy(d => d.Name).ToList();
      }

      public abstract DirectoryInfo? GetAssociatedDirectoryInfo(int domainObjectId);

      public void CreateFolder(int domainObjectId)
      {
         var folderName = GetAssociatedDirectoryInfo(domainObjectId)?.FullName;

         if (folderName != null)
         {
            Directory.CreateDirectory(folderName);
         }
      }

      public void OpenFolder(int domainObjectId)
      {
         OpenFolder(GetAssociatedDirectoryInfo(domainObjectId));
      }

      public void OpenFolder(DirectoryInfo? directoryInfo)
      {
         if (directoryInfo != null && directoryInfo.Exists)
         {
            Process.Start("explorer.exe", directoryInfo.FullName);
         }
      }

      public void OpenFile(FileInfo fileInfo)
      {
         if (fileInfo != null)
         {
            var p = new Process();
            p.StartInfo = new ProcessStartInfo(fileInfo.FullName) { UseShellExecute = true };
            p.Start();
         }
      }
      public void CopyFilesInAssociatedFolder(int domainObjectId, List<ManagedFile> managedFiles)
      {
         var associatedFolder = GetAssociatedDirectoryInfo(domainObjectId);

         foreach (var managedFile in managedFiles)
         {
            var destinationFilePath = Path.Combine(associatedFolder.FullName, managedFile.FileName);

            if (managedFile.IsInMemory)
            {
               using (var fileStream = new FileStream(destinationFilePath, FileMode.Create, FileAccess.Write))
               {
                  managedFile.InMemoryContent.Position = 0;
                  managedFile.InMemoryContent.CopyTo(fileStream);
               }
            }
            else
            {
               if (IsImage(managedFile.PhysicalFile))
               {
                  _imageCompressionService.CompressAndSaveImageAsJpeg(managedFile.PhysicalFile.FullName, destinationFilePath);
               }
               else
               {
                  File.Copy(managedFile.PhysicalFile.FullName, destinationFilePath);
               }
            }
         }
      }

      public bool IsImage(FileInfo fileInfo)
      {
         return Path.GetExtension(fileInfo.Name).ToLower().Equals(".jpg") ||
                Path.GetExtension(fileInfo.Name).ToLower().Equals(".jpeg") ||
                Path.GetExtension(fileInfo.Name).ToLower().Equals(".bmp") ||
                Path.GetExtension(fileInfo.Name).ToLower().Equals(".png") ||
                Path.GetExtension(fileInfo.Name).ToLower().Equals(".gif") ||
                Path.GetExtension(fileInfo.Name).ToLower().Equals(".heic") ||
                Path.GetExtension(fileInfo.Name).ToLower().Equals(".heif");
      }

      public void TryDeleteAssociatedFolder(int domainObjectId)
      {
         var associatedFolder = GetAssociatedDirectoryInfo(domainObjectId);

         if (associatedFolder != null && associatedFolder.Exists)
         {
            try
            {
               associatedFolder.Delete(true);
            }
            catch (Exception ex)
            {
               throw new IOException($"Failed to delete folder '{associatedFolder.FullName}'.", ex);
            }
         }
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
