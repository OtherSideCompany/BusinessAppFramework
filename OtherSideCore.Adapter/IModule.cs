using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OtherSideCore.Adapter.Factories;
using OtherSideCore.Adapter.Relations;
using OtherSideCore.Adapter.Services;
using OtherSideCore.Adapter.Views;
using OtherSideCore.Application.Factories;

namespace OtherSideCore.Adapter
{
   public interface IModule
   {
      void RegisterDomainObjectEntityMapping(IDomainObjectEntityTypeMap domainObjectEntityTypeMap);
      void RegisterServices(IServiceCollection services);
      void RegisterRepositories(IRepositoryFactory repositoryFactory);
      void RegisterDomainObjectServices(IDomainObjectServiceFactory domainObjectServiceFactory, IServiceProvider serviceProvider);
      void RegisterDomainObjectViewModels(IDomainObjectViewModelFactory domainObjectViewModelFactory, IServiceProvider serviceProvider);
      void RegisterDomainObjectReferences(IDomainObjectReferenceFactory domainObjectReferenceFactory);
      void RegisterWorkspaces(WorkspaceFactory workspaceFactory, IServiceProvider serviceProvider);
      void RegisterDomainObjectInteractions(IDomainObjectInteractionService domainObjectInteractionService, IServiceProvider serviceProvider);
      void RegisterResourceManagers(ILocalizationService localizationService);
      List<NavigationItem> GetNavigationItems();
      Task SeedDatabaseAsync(DbContext dbContext);
   }
}
