using System.Data;
using System.Data.SqlClient;
using Toro.Domain.Repositories;
using Toro.Infra.Repository;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class InfraServiceCollectionDependencyInjection
    {
        public static IServiceCollection AddInfra(this IServiceCollection services, RepositoryConfiguration configuration)
        {
            if (services is null)
                throw new System.ArgumentNullException(nameof(services));

            if (configuration is null)
                throw new System.ArgumentNullException(nameof(configuration));

            services.AddScoped<IAccountReadOnlyRepository, AccountRepository>();
            services.AddScoped<IAccountWriteOnlyRepository, AccountRepository>();
            services.AddScoped<IAccountHistoryReadOnlyRepository, AccountHistoryRepository>();
            services.AddScoped<IAccountHistoryWriteOnlyRepository, AccountHistoryRepository>();

            services.AddSingleton(configuration);
            services.AddScoped<IDbConnection>(f => {
                return new SqlConnection(configuration.SqlConnectionString); 
            });

            return services;
        }
    }
}
