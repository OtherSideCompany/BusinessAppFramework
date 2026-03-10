using BusinessAppFramework.Application.Workflows;

namespace BusinessAppFramework.DocumentRendering
{
   public interface IDocumentModelLoader
   {
      Task<object?> LoadAsync(int domainObjectId, string cultureInfo, CancellationToken cancellationToken = default);
   }
}
