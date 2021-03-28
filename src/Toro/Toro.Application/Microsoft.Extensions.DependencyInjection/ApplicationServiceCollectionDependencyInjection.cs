using Toro.Application.Services;
using Toro.Domain.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ApplicationServiceCollectionDependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            if (services is null)
                throw new System.ArgumentNullException(nameof(services));

            services.AddScoped<IAccountService, AccountService>();

            return services;
        }
    }
}
