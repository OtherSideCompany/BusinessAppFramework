using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessAppFramework.Application.Interfaces
{
   public interface IDocumentStorageService
   {
      Task StoreAsync(Guid storageKey, Stream content, string contentType, CancellationToken ct = default);
      Task<Stream> OpenReadAsync(Guid storageKey, CancellationToken ct = default);
      Task DeleteAsync(Guid storageKey, CancellationToken ct = default);
      Task<bool> ExistsAsync(Guid storageKey, CancellationToken ct = default);
   }
}
