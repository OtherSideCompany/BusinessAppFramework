using Microsoft.Extensions.DependencyInjection;

namespace BusinessAppFramework.Bootstrapper
{
    public static class ServiceRegistrationHelper
    {
        public static IServiceCollection AddAggregateService(this IServiceCollection services, Type implementationType, Type aggregateType)
        {
            services.AddScoped(implementationType);

            foreach (var contract in implementationType.GetInterfaces().Where(contract => IsContractFor(contract, aggregateType)))
                services.AddScoped(contract, provider => provider.GetRequiredService(implementationType));

            return services;
        }

        private static bool IsContractFor(Type contract, Type aggregateType)
        {
            return contract.IsGenericType && contract.GetGenericArguments().Contains(aggregateType);
        }
    }
}
