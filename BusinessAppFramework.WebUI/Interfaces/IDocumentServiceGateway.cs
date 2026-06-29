using BusinessAppFramework.Domain.DomainObjects;
using BusinessAppFramework.WebUI.Documents;

namespace BusinessAppFramework.WebUI.Interfaces
{
    public interface IDocumentServiceGateway
    {
        Task<int> UploadDocumentAsync(int parentId, string relationKey, string fileName, string contentType, Stream fileStream, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(int domainObjectId, CancellationToken cancellationToken = default);
        Task DeleteDocumentAsync(int domainObjectId, CancellationToken cancellationToken = default);
        Task<DocumentDownloadResult?> DownloadDocumentAsync(int documentId, CancellationToken cancellationToken = default);
    }
}
