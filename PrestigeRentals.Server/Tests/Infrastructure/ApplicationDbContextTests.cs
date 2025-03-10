using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PrestigeRentals.Infrastructure.Persistence;
using PrestigeRentals.Domain.Entities;
using Xunit;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace PrestigeRentals.Tests
{
    public class ApplicationDbContextTests
    {
        private DbContextOptions<ApplicationDbContext> _dbContextOptions;

        public ApplicationDbContextTests()
        {
            // Set up in-memory database for testing
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "PrestigeRentalsTestDB")
                .Options;
        }

        [Fact]
        public void AddVehicle_ShouldAddVehicleToDatabase()
        {
            // Arrange
            using var context = new ApplicationDbContext(_dbContextOptions);
            var vehicle = new Vehicle
            {
                Id = 1,
                Make = "Toyota",
                Model = "Camry",
                EngineSize = 2000,
                FuelType = "Petrol",
                Transmission = "Manual",
                Active = true,
                Deleted = false
            };

            // Act
            context.Vehicles.Add(vehicle);
            context.SaveChanges();

            // Assert
            var addedVehicle = context.Vehicles.FirstOrDefault(v => v.Id == vehicle.Id);
            Assert.NotNull(addedVehicle);
            Assert.Equal("Toyota", addedVehicle.Make);
            Assert.Equal("Camry", addedVehicle.Model);
            Assert.Equal(2000, addedVehicle.EngineSize);
            Assert.Equal("Petrol", addedVehicle.FuelType);
            Assert.Equal("Manual", addedVehicle.Transmission);
            Assert.True(addedVehicle.Active);
            Assert.False(addedVehicle.Deleted);
        }

        [Fact]
        public void AddVehicleOptions_ShouldAddVehicleOptionsToDatabase()
        {
            // Arrange
            using var context = new ApplicationDbContext(_dbContextOptions);

            var vehicle = new Vehicle
            {
                Id = 1,
                Make = "Toyota",
                Model = "Camry",
                EngineSize = 2000,
                FuelType = "Petrol",
                Transmission = "Manual",
                Active = true,
                Deleted = false
            };

            context.Vehicles.Add(vehicle);
            context.SaveChanges();

            var vehicleOptions = new VehicleOptions
            {
                Id = 1,
                VehicleId = vehicle.Id,
                Navigation = true,
                HeadsUpDisplay = false,
                HillAssist = true,
                CruiseControl = true,
                Active = true,
                Deleted = false
            };

            // Act
            context.VehicleOptions.Add(vehicleOptions);
            context.SaveChanges();

            // Assert
            var addedOption = context.VehicleOptions
                .FirstOrDefault(vo => vo.VehicleId == vehicle.Id);

            Assert.NotNull(addedOption);
            Assert.Equal(vehicle.Id, addedOption.VehicleId); // Check if the vehicle ID matches
            Assert.True(addedOption.Navigation); // Check if the Navigation option is set to true
            Assert.False(addedOption.HeadsUpDisplay); // Check if the HeadsUpDisplay option is set to false
            Assert.True(addedOption.HillAssist); // Check if HillAssist is set to true
            Assert.True(addedOption.CruiseControl); // Check if CruiseControl is set to true
        }

        [Fact]
        public void VehicleOptions_ShouldHaveUniqueVehicleId()
        {
            // Arrange
            using var context = new ApplicationDbContext(_dbContextOptions);

            var vehicle1 = new Vehicle
            {
                Id = 1,
                Make = "Toyota",
                Model = "Camry",
                EngineSize = 2000,
                FuelType = "Petrol",
                Transmission = "Manual",
                Active = true,
                Deleted = false
            };
            var vehicle2 = new Vehicle
            {
                Id = 2,
                Make = "Honda",
                Model = "Civic",
                EngineSize = 1800,
                FuelType = "Diesel",
                Transmission = "Automatic",
                Active = true,
                Deleted = false
            };

            context.Vehicles.Add(vehicle1);
            context.Vehicles.Add(vehicle2);
            context.SaveChanges();

            var vehicleOptions1 = new VehicleOptions
            {
                VehicleId = vehicle1.Id,
                Navigation = true,
                HeadsUpDisplay = false,
                HillAssist = true,
                CruiseControl = true,
                Active = true,
                Deleted = false
            };

            var vehicleOptions2 = new VehicleOptions
            {
                VehicleId = vehicle2.Id,
                Navigation = true,
                HeadsUpDisplay = true,
                HillAssist = false,
                CruiseControl = false,
                Active = true,
                Deleted = false
            };

            // Act
            context.VehicleOptions.Add(vehicleOptions1);
            context.VehicleOptions.Add(vehicleOptions2);
            context.SaveChanges();

            // Assert
            var option1 = context.VehicleOptions
                .FirstOrDefault(vo => vo.VehicleId == vehicle1.Id);
            var option2 = context.VehicleOptions
                .FirstOrDefault(vo => vo.VehicleId == vehicle2.Id);

            Assert.NotNull(option1);
            Assert.NotNull(option2);
        }
    }
}
