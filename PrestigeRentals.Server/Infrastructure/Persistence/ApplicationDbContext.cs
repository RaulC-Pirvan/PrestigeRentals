using Microsoft.EntityFrameworkCore;
using PrestigeRentals.Domain.Entities;

namespace PrestigeRentals.Infrastructure.Persistence
{
    /// <summary>
    /// The application's database context that provides access to the database via DbSet properties.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        /// <param name="options">The database context options.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        /// <summary>
        /// Table for vehicle data.
        /// </summary>
        public DbSet<Vehicle> Vehicles { get; set; }

        /// <summary>
        /// Table for vehicle option configurations.
        /// </summary>
        public DbSet<VehicleOptions> VehicleOptions { get; set; }

        /// <summary>
        /// Table for vehicle photos.
        /// </summary>
        public DbSet<VehiclePhotos> VehiclePhotos { get; set; }

        /// <summary>
        /// Table for user accounts.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Table for detailed user profile information.
        /// </summary>
        public DbSet<UserDetails> UsersDetails { get; set; }

        /// <summary>
        /// Table for request/response logs.
        /// </summary>
        public DbSet<LogEntry> Logs { get; set; }

        /// <summary>
        /// Model configuration using Fluent API.
        /// </summary>
        /// <param name="modelBuilder">The builder used to construct the model.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Enforce one-to-one relationship between Vehicle and VehicleOptions
            modelBuilder.Entity<VehicleOptions>()
                .HasIndex(vo => vo.VehicleId)
                .IsUnique();

            // Define one-to-one relationship between User and UserDetails with cascade delete
            modelBuilder.Entity<UserDetails>()
                .HasOne(ud => ud.User)
                .WithOne()
                .HasForeignKey<UserDetails>(ud => ud.UserID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
