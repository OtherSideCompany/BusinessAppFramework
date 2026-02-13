using BusinessAppFramework.Domain;

namespace BusinessAppFramework.DocumentRendering
{
   public interface IDocumentModelLoaderFactory
   {
      DocumentModelLoader CreateDocumentModelLoader(Type documentModelLoaderType);
   }
}
