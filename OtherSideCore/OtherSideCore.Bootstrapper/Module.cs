using Microsoft.Extensions.DependencyInjection;
using OtherSideCore.Adapter;
using OtherSideCore.Adapter.Factories;
using OtherSideCore.Adapter.Views;
using OtherSideCore.Application.Factories;

namespace OtherSideCore.Bootstrapper
{
   public abstract class Module : IModule
   {
      #region Fields

      

      #endregion

      #region Properties

      

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public Module()
      {
         
      }      

      #endregion

      #region Public Methods

      public abstract void RegisterServices(IServiceCollection services);
      public abstract void RegisterRepositories(IRepositoryFactory repositoryFactory);
      public abstract void RegisterDomainObjectServices(IDomainObjectServiceFactory domainObjectServiceFactory);
      public abstract void RegisterDomainObjectReferences(IDomainObjectReferenceFactory domainObjectReferenceFactory);
      public abstract List<NavigationItem> GetNavigationItems();

      #endregion

      #region Private Methods



      #endregion
   }
}
