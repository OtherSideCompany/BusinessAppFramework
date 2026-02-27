using BusinessAppFramework.Application.Interfaces;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessAppFramework.Infrastructure.DocumentStorage
{
   public class FileSystemDocumentStorageService : IDocumentStorageService
   {
      #region Fields

      private readonly string _rootPath;

      #endregion

      #region Properties



      #endregion

      #region Constructor

      public FileSystemDocumentStorageService(string rootPath)
      {
         _rootPath = Path.GetFullPath(rootPath);
         Directory.CreateDirectory(_rootPath);
      }

      #endregion

      #region Public Methods

      public async Task StoreAsync(Guid storageKey, Stream content, string contentType, CancellationToken ct = default)
      {
         var path = GetPath(storageKey);

         await using var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 81920, FileOptions.Asynchronous);

         await content.CopyToAsync(fs, ct);
      }

      public async Task DeleteAsync(Guid storageKey, CancellationToken ct = default)
      {
         var path = GetPath(storageKey);

         if (File.Exists(path))
            File.Delete(path);

         await Task.CompletedTask;
      }

      public async Task<bool> ExistsAsync(Guid storageKey, CancellationToken ct = default)
      {
         var path = GetPath(storageKey);
         return await Task.FromResult(File.Exists(path));
      }

      public async Task<Stream> OpenReadAsync(Guid storageKey, CancellationToken ct = default)
      {
         var path = GetPath(storageKey);

         Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 81920, FileOptions.Asynchronous | FileOptions.SequentialScan);

         return await Task.FromResult(stream);
      }

      #endregion

      #region Private Methods

      private string GetPath(Guid storageKey)
      {
         return Path.Combine(_rootPath, storageKey.ToString("N"));
      }       

      #endregion
   }
}
