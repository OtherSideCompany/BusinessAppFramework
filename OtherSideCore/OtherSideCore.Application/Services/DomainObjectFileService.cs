using OtherSideCore.Application.AppConfiguration;
using OtherSideCore.Appplication.Services;
using OtherSideCore.Domain.DomainObjects;
using System.Diagnostics;

namespace OtherSideCore.Application.Services
{
   public abstract class DomainObjectFileService : IDomainObjectFileService
   {
      #region Fields

      protected IAppConfiguration _appConfiguration;
      protected IImageCompressionService _imageCompressionService;
      protected IUserDialogService _userDialogService;

      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectFileService(IAppConfiguration appConfiguration, IImageCompressionService imageCompressionService, IUserDialogService userDialogService)
      {
         _appConfiguration = appConfiguration;
         _imageCompressionService = imageCompressionService;
         _userDialogService = userDialogService;
      }

      #endregion

      #region Public Methods

      public List<FileInfo> GetAssociatedFileInfos(DomainObject domainObject)
      {
         var fileInfos = new List<FileInfo>();
         var directoryInfo = GetAssociatedDirectoryInfo(domainObject);

         if (Directory.Exists(directoryInfo.FullName))
         {
            var fileNames = Directory.GetFiles(directoryInfo.FullName);
            fileInfos = fileNames.Select(fileName => new FileInfo(fileName)).ToList();
         }

         return fileInfos.OrderBy(f => f.Name).ToList();
      }

      public List<DirectoryInfo> GetAssociatedNestedDirectoriesInfos(DomainObject domainObject)
      {
         var directoryInfos = new List<DirectoryInfo>();
         var directoryInfo = GetAssociatedDirectoryInfo(domainObject);

         if (Directory.Exists(directoryInfo.FullName))
         {
            var directoryNames = Directory.GetDirectories(directoryInfo.FullName);
            directoryInfos = directoryNames.Select(directoryName => new DirectoryInfo(directoryName)).ToList();
         }

         return directoryInfos.OrderBy(d => d.Name).ToList();
      }

      public abstract DirectoryInfo GetAssociatedDirectoryInfo(DomainObject domainObject);

      public void CreateFolder(DomainObject domainObject)
      {
         var folderName = GetAssociatedDirectoryInfo(domainObject).FullName;
         Directory.CreateDirectory(folderName);
      }

      public void OpenFolder(DomainObject domainObject)
      {
         OpenFolder(GetAssociatedDirectoryInfo(domainObject));
      }

      public void OpenFolder(DirectoryInfo directoryInfo)
      {
         try
         {
            Process.Start("explorer.exe", directoryInfo.FullName);
         }
         catch (System.ComponentModel.Win32Exception e)
         {
            _userDialogService.Error("Une erreur s'est produite lors de l'ouverture du dossier " + directoryInfo.FullName);
         }
         catch (ObjectDisposedException e)
         {
            _userDialogService.Error("Le dossier " + directoryInfo.FullName + " a été supprimé depuis la demande d'ouverture.");
         }
         catch (FileNotFoundException e)
         {
            _userDialogService.Error("Le dossier " + directoryInfo.FullName + " n'a pas été trouvé.");
         }         
      }

      public void OpenFile(FileInfo fileInfo)
      {       
         try
         {
            if (fileInfo != null)
            {
               var p = new Process();
               p.StartInfo = new ProcessStartInfo(fileInfo.FullName) { UseShellExecute = true };
               p.Start();
            }
         }
         catch (System.ComponentModel.Win32Exception e)
         {
            _userDialogService.Error("Une erreur s'est produite lors de l'ouverture du fichier " + fileInfo.FullName);
         }
         catch (ObjectDisposedException e)
         {
            _userDialogService.Error("Le fichier " + fileInfo.FullName + " a été supprimé depuis la demande d'ouverture.");
         }
         catch (FileNotFoundException e)
         {
            _userDialogService.Error("Le fichier " + fileInfo.FullName + " n'a pas été trouvé.");
         }
      }


      public void CopyFilesInAssociatedFolder(DomainObject domainObject, List<ManagedFile> managedFiles)
      {
         var associatedFolder = GetAssociatedDirectoryInfo(domainObject);

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

      #endregion

      #region Private Methods

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

      #endregion
   }
}
