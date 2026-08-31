using BusinessAppFramework.Application.Documents;
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
        Task<List<DocumentCategory<TDocument>>> GetDocumentCategoriesAsync<TDocument>(int domainObjectId, string relationKey, CancellationToken cancellationToken = default) where TDocument : IDocument;
        Task<int> GetDocumentsCountAsync(int domainObjectId, string relationKey, CancellationToken cancellationToken = default);
        Task MoveDocumentAsync(int documentId, int targetParentId, string targetRelationKey, CancellationToken cancellationToken = default);
    }
}
