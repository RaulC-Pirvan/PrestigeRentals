using System.Reflection;
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
using PrestigeRentals.Infrastructure.DependencyInjection;
using Swashbuckle.AspNetCore.Filters;


namespace Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure JwtSettings from appsettings.json
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

            // Add AutoMapper configuration
            MapperConfiguration mapperConfiguration = new(configuration => configuration.AddProfile<MappingProfile>());
            mapperConfiguration.CompileMappings();

            // Add services to the container
            builder.Services.AddControllers();

            // Register application services
            builder.Services.AddApplicationServices();

            // Register AutoMapper singleton
            builder.Services.AddSingleton(mapperConfiguration.CreateMapper());

            // Configure authentication using JWT
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
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            // Configure Swagger with JWT Bearer Authentication
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Prestige Rentals API",
                    Version = "v1",
                    Description = "API for managing products in Prestige Rentals"
                });

                // Add Bearer Authentication to Swagger UI
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "Enter 'Bearer' followed by your JWT token"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            // Add and configure CORS policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularApp", policy =>
                {
                    policy.WithOrigins("http://localhost:4200") // Allow only Angular app's origin
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            // Add Infrastructure services
            builder.Services.AddInfrastructure(builder.Configuration);

            // Build the application
            var app = builder.Build();

            // Use Logging Middleware
            app.UseMiddleware<LoggingMiddleware>();

            app.UseStaticFiles(); // enables wwwroot to serve images

            // Enable Swagger UI in development
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Redirect HTTP to HTTPS (for production environments)
            app.UseHttpsRedirection();

            // Enable CORS
            app.UseCors("AllowAngularApp");

            // Use routing middleware
            app.UseRouting();

            // Use authentication and authorization middleware
            app.UseAuthentication();
            app.UseAuthorization();

            // Map controllers
            app.MapControllers();

            // Run the application
            app.Run();
        }
    }
}
