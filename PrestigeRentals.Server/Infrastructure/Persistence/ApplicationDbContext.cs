using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PrestigeRentals.Domain.Entities;

namespace PrestigeRentals.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehicleOptions> VehicleOptions { get; set; }
        public DbSet<VehiclePhotos> VehiclePhotos { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserDetails> UsersDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VehicleOptions>().HasIndex(vo => vo.VehicleId).IsUnique();
            modelBuilder.Entity<UserDetails>().HasOne(ud => ud.User).WithOne().HasForeignKey<UserDetails>(ud => ud.UserID).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
