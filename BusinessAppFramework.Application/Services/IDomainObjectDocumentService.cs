using BusinessAppFramework.Domain.DomainObjects;

namespace BusinessAppFramework.Application.Services
{
   public interface IDomainObjectDocumentService<T> where T : DomainObject
   {
      Task<int> AttachAsync(int parentId, string relationKey, string fileName, string contentType, long size, Stream content, CancellationToken ct = default);
      Task<(Stream stream, string contentType, string fileName)?> OpenReadAsync(int documentId, CancellationToken ct = default);
      Task DeleteAsync(int documentId, CancellationToken ct = default);
      Task<bool> ExistsAsync(int documentId, CancellationToken ct = default);
   }
}
