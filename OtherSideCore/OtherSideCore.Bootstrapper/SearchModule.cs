using Microsoft.Extensions.DependencyInjection;
using OtherSideCore.Adapter;
using OtherSideCore.Adapter.Factories;
using OtherSideCore.Application.Factories;

namespace OtherSideCore.Bootstrapper
{
   public abstract class SearchModule : ISearchModule
   {
      #region Fields

      

      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public SearchModule()
      {
         
      }

      #endregion

      #region Public Methods

      public abstract void RegisterServices(IServiceCollection services);
      public abstract void RegisterSearchServices(ISearchServiceFactory searchServiceFactory, IServiceProvider serviceProvider);
      public abstract void RegisterDomainObjectSearchFactory(IDomainObjectSearchFactory domainObjectSearchFactory, IServiceProvider serviceProvider);      
      public abstract void RegisterDomainObjectSearchViewModelFactory(IDomainObjectsSearchViewModelFactory domainObjectSearchFactory, IServiceProvider serviceProvider);

      #endregion

      #region Private Methods



      #endregion
   }
}
