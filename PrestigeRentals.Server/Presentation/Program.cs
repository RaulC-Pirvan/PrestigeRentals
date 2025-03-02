using System.Reflection;
using PrestigeRentals.Infrastructure;
using Microsoft.OpenApi.Models;
using PrestigeRentals.Application.Services;
using AutoMapper;
using PrestigeRentals.Application.Helpers;
using PrestigeRentals.Application.Services.Interfaces;

namespace Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            MapperConfiguration mapperConfiguration = new(configuration => configuration.AddProfile<MappingProfile>());
            mapperConfiguration.CompileMappings();

            // Add services to the container
            builder.Services.AddControllers();

            builder.Services.AddScoped<IVehicleService, VehicleService>();
            builder.Services.AddScoped<IVehicleOptionsService, VehicleOptionsService>();

            builder.Services.AddSingleton(mapperConfiguration.CreateMapper());

            // Configure Swagger
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Prestige Rentals API",
                    Version = "v1",
                    Description = "API for managing products in Prestige Rentals"
                });
            });

            // Add and configure CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularApp", policy =>
                {
                    policy.WithOrigins("http://localhost:4200") // Allow only Angular app's origin
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            builder.Services.AddInfrastructure(builder.Configuration);

            var app = builder.Build();

            // Enable Swagger in development
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Configure CORS middleware
            app.UseCors("AllowAngularApp");

            // Other middleware
            app.UseRouting();
            app.UseAuthorization();

            // Map controllers
            app.MapControllers();

            // Run the app
            app.Run();
        }
    }
}

