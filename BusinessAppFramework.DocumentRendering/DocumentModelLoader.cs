using BusinessAppFramework.Application.Factories;
using BusinessAppFramework.Application.Relations;

namespace BusinessAppFramework.DocumentRendering
{
   public abstract class DocumentModelLoader : IDocumentModelLoader
   {
      #region Fields

      protected readonly IDomainObjectServiceFactory _domainObjectServiceFactory;
      protected readonly IRelationService _relationService;

      #endregion

      #region Properties



      #endregion

      #region Events



      #endregion

      #region Constructor

      public DocumentModelLoader(
         IDomainObjectServiceFactory domainObjectServiceFactory,
         IRelationService relationService)
      {
         _domainObjectServiceFactory = domainObjectServiceFactory;
         _relationService = relationService;
      }

      #endregion

      #region Public Methods

      public abstract Task<object?> LoadAsync(int domainObjectId, string cultureInfo, CancellationToken cancellationToken = default);

      #endregion

      #region Private Methods



      #endregion
   }
}
