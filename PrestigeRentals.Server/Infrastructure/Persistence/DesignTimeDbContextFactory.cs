using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PrestigeRentals.Infrastructure.Persistence
{
    /// <summary>
    /// A factory for creating an instance of <see cref="ApplicationDbContext"/> at design-time.
    /// This is used by tools like EF Core migrations to configure the DbContext in a development environment.
    /// </summary>
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        /// <summary>
        /// Creates a new instance of <see cref="ApplicationDbContext"/> with the provided arguments.
        /// This method is called at design-time when running commands like migrations.
        /// </summary>
        /// <param name="args">The command-line arguments passed to the application.</param>
        /// <returns>An instance of <see cref="ApplicationDbContext"/> configured for design-time use.</returns>
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // Build the options for the ApplicationDbContext using the PostgreSQL connection string
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            // Define the PostgreSQL connection string (to be adjusted for your specific environment)
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=PrestigeRentals;Username=postgres;Password=root");

            // Return a new instance of the ApplicationDbContext configured with the options
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
