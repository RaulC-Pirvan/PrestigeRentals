using Microsoft.Extensions.DependencyInjection;
using PrestigeRentals.Application.Services;
using PrestigeRentals.Application.Services.Interfaces;

namespace PrestigeRentals.Application.DependencyInjection
{
    /// <summary>
    /// Extension method to register application-level services and repositories.
    /// </summary>
    public static class ServiceRegistration
    {
        /// <summary>
        /// Registers services and repositories for dependency injection.
        /// </summary>
        /// <param name="services">The IServiceCollection instance.</param>
        public static void AddApplicationServices(this IServiceCollection services)
        {
            // Services
            services.AddScoped<IVehicleService, VehicleService>();
            services.AddScoped<IVehiclePhotosService, VehiclePhotosService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IUserManagementService, UserManagementService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IOrderService, OrderService>();

            // Repositories
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IVehicleOptionsRepository, VehicleOptionsRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserDetailsRepository, UserDetailsRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
        }
    }
}
