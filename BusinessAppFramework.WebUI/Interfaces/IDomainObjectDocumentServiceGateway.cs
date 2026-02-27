using BusinessAppFramework.Domain.DomainObjects;

namespace BusinessAppFramework.WebUI.Interfaces
{
   public interface IDomainObjectDocumentServiceGateway<T> where T : DomainObject
   {
      Task<int> UploadDocumentAsync(int parentId, string relationKey, string fileName, string contentType, Stream fileStream, CancellationToken cancellationToken = default);
      Task<bool> ExistsAsync(int domainObjectId, CancellationToken cancellationToken = default);
      string GetDownloadDocumentUrl(int domainObjectId, CancellationToken cancellationToken = default);
      Task DeleteDocumentAsync(int domainObjectId, CancellationToken cancellationToken = default);
   }
}
