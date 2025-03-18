using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PrestigeRentals.Application.Services;
using PrestigeRentals.Application.Services.Interfaces;

namespace PrestigeRentals.Application.DependencyInjection
{
    public static class ServiceRegistration
    {
        public static void AddAplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IVehicleService, VehicleService>();
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IVehicleOptionsRepository, VehicleOptionsRepository>();
            services.AddScoped<IVehiclePhotosService, VehiclePhotosService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserDetailsRepository, UserDetailsRepository>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IUserManagementService, UserManagementService>();
        }
    }
}
