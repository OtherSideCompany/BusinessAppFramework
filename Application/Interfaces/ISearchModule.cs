using Microsoft.Extensions.DependencyInjection;

namespace Application.Interfaces
{
   public interface ISearchModule
   {
      void RegisterServices(IServiceCollection services);
   }
}
