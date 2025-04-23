using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PrestigeRentals.Infrastructure.Persistence;

namespace PrestigeRentals.Infrastructure
{
    /// <summary>
    /// Provides extension methods for registering infrastructure services into the <see cref="IServiceCollection"/>.
    /// This includes services like the database context for data access.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Registers the infrastructure services, including the database context, into the provided <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to register the services into.</param>
        /// <param name="configuration">The application's configuration settings, typically from appsettings.json or environment variables.</param>
        /// <returns>The updated <see cref="IServiceCollection"/> with the registered services.</returns>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Add the ApplicationDbContext to the DI container with the PostgreSQL database connection string
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("PostgreSQLConnection"))
            );

            // Optionally, you can add more infrastructure services here, like repositories or other services

            return services;
        }
    }
}
