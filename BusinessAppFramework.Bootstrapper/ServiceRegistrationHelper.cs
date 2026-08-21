using Microsoft.Extensions.DependencyInjection;

namespace BusinessAppFramework.Bootstrapper
{
    public static class ServiceRegistrationHelper
    {
        public static IServiceCollection AddScopedWithGenericContractsFor(this IServiceCollection services, Type implementationType, Type aggregateType)
        {
            services.AddScoped(implementationType);

            foreach (var contract in implementationType.GetInterfaces().Where(contract => IsContractFor(contract, aggregateType)))
                services.AddScoped(contract, provider => provider.GetRequiredService(implementationType));

            return services;
        }

        public static IServiceCollection AddScopedWithGenericContractsFor<TImplementation, TAggregate>(this IServiceCollection services)
        where TImplementation : class
        {
            return services.AddScopedWithGenericContractsFor(typeof(TImplementation), typeof(TAggregate));
        }

        private static bool IsContractFor(Type contract, Type aggregateType)
        {
            return contract.IsGenericType && contract.GetGenericArguments().Contains(aggregateType);
        }
    }
}
