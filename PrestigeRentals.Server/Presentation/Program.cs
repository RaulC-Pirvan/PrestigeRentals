using System.Reflection;
using PrestigeRentals.Infrastructure;
using Microsoft.OpenApi.Models;
using PrestigeRentals.Application.Services;
using AutoMapper;
using PrestigeRentals.Application.Helpers;
using PrestigeRentals.Application.Services.Interfaces;
using System.Configuration;
using PrestigeRentals.Application.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using PrestigeRentals.Presentation.Middleware;

namespace Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

            MapperConfiguration mapperConfiguration = new(configuration => configuration.AddProfile<MappingProfile>());
            mapperConfiguration.CompileMappings();


            // Add services to the container
            builder.Services.AddControllers();

            builder.Services.AddAplicationServices();

            builder.Services.AddSingleton(mapperConfiguration.CreateMapper());

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();
                var key = Encoding.ASCII.GetBytes(jwtSettings.SecretKey);

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

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

            app.UseAuthentication();
            app.UseAuthorization();

            // Other middleware
            app.UseRouting();
            app.UseAuthorization();

            app.UseMiddleware<LoggingMiddleware>();

            // Map controllers
            app.MapControllers();

            // Run the app
            app.Run();
        }
    }
}

