using BusinessAppFramework.Application.Workflows;

namespace BusinessAppFramework.DocumentRendering
{
   public interface IDocumentModelLoader
   {
      Task<object?> LoadAsync(int domainObjectId, CancellationToken cancellationToken = default);
   }
}
