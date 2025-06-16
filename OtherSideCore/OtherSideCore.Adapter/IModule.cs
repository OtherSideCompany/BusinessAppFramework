using Microsoft.Extensions.DependencyInjection;
using OtherSideCore.Adapter.Factories;
using OtherSideCore.Adapter.Views;
using OtherSideCore.Application.Factories;

namespace OtherSideCore.Adapter
{
   public interface IModule
   { 
      void RegisterServices(IServiceCollection services);
      void RegisterRepositories(IRepositoryFactory repositoryFactory);
      void RegisterDomainObjectServices(IDomainObjectServiceFactory domainObjectServiceFactory);
      void RegisterDomainObjectReferences(IDomainObjectReferenceFactory domainObjectReferenceFactory);
      void RegisterDomainObjectInteractions(IDomainObjectInteractionService domainObjectInteractionService, IServiceProvider serviceProvider);
      List<NavigationItem> GetNavigationItems();
   }
}
