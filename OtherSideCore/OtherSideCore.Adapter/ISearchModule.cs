using Microsoft.Extensions.DependencyInjection;
using OtherSideCore.Adapter.Factories;
using OtherSideCore.Application.Factories;

namespace OtherSideCore.Adapter
{
   public interface ISearchModule
   {
      void RegisterServices(IServiceCollection services);
      void RegisterSearchServices(ISearchServiceFactory searchServiceFactory, IServiceProvider serviceProvider);
      void RegisterDomainObjectSearchFactory(IDomainObjectSearchFactory domainObjectSearchFactory, IServiceProvider serviceProvider);
      void RegisterDomainObjectSearchViewModelFactory(IDomainObjectsSearchViewModelFactory domainObjectSearchFactory, IServiceProvider serviceProvider);
   }
}
