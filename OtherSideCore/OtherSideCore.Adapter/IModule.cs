using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OtherSideCore.Adapter.Factories;
using OtherSideCore.Adapter.Services;
using OtherSideCore.Adapter.Views;
using OtherSideCore.Application.Factories;
using OtherSideCore.Application.Services;

namespace OtherSideCore.Adapter
{
    public interface IModule
   { 
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
