using OtherSideCore.Application.Services;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Application.DomainObjectBrowser
{
   public abstract class DomainObjectTreeSearch : IDomainObjectTreeSearch
   {
      #region Fields

      protected IDomainObjectQueryServiceFactory _domainObjectQueryServiceFactory;

      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectTreeSearch(IDomainObjectQueryServiceFactory domainObjectQueryServiceFactory)
      {
         _domainObjectQueryServiceFactory = domainObjectQueryServiceFactory;
      }

      #endregion

      #region Public Methods      

      public abstract Task SearchAsync(DomainObject parent);

      public void Dispose()
      {
         
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
