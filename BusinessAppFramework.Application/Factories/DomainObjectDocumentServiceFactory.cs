using BusinessAppFramework.Application.Services;
using BusinessAppFramework.Domain.DomainObjects;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessAppFramework.Application.Factories
{
   public class DomainObjectDocumentServiceFactory : IDomainObjectDocumentServiceFactory
   {
      #region Fields

      private readonly IServiceProvider _serviceProvider;

      #endregion

      #region Properties



      #endregion

      #region Constructor

      public DomainObjectDocumentServiceFactory(IServiceProvider serviceProvider)
      {
         _serviceProvider = serviceProvider;
      }

      #endregion

      #region Public Methods

      public IDomainObjectDocumentService<T> CreateDomainObjectDocumentService<T>() where T : DomainObject, new()
      {
         return _serviceProvider.GetRequiredService<IDomainObjectDocumentService<T>>();
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
