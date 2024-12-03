using System.Reflection;
using PrestigeRentals.Application.Interfaces;
using PrestigeRentals.Application.Services;
using Microsoft.OpenApi.Models;

namespace Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddControllers();
            builder.Services.AddScoped<IProductService, ProductService>();

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
