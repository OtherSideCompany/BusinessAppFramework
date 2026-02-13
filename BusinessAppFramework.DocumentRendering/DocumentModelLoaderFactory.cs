using Microsoft.Extensions.DependencyInjection;

namespace BusinessAppFramework.DocumentRendering
{
   public class DocumentModelLoaderFactory : IDocumentModelLoaderFactory
   {
      #region Fields

      private readonly IServiceProvider _serviceProvider;

      #endregion

      #region Properties



      #endregion

      #region Events



      #endregion

      #region Constructor

      public DocumentModelLoaderFactory(IServiceProvider serviceProvider)
      {
         _serviceProvider = serviceProvider;
      }

      #endregion

      #region Public Methods

      public DocumentModelLoader CreateDocumentModelLoader(Type documentModelLoaderType)
      {
         return (DocumentModelLoader)_serviceProvider.GetRequiredService(documentModelLoaderType);
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
