using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using PrestigeRentals.Application.Services;
using PrestigeRentals.Application.Services.Interfaces;
using PrestigeRentals.Domain.Entities;
using PrestigeRentals.Infrastructure.Persistence;
using Xunit;

namespace PrestigeRentals.Tests.Services
{
    public class VehicleServiceTests
    {
        private readonly VehicleService _vehicleService;
        private readonly Mock<ApplicationDbContext> _dbContextMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<VehicleService>> _loggerMock;

        public VehicleServiceTests()
        {
            _dbContextMock = new Mock<ApplicationDbContext>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<VehicleService>>();

            _vehicleService = new VehicleService(_dbContextMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetVehicleById_ShouldReturnVehicle_WhenVehicleExists()
        {
            // Arrange
            var vehicle = new Vehicle { Id = 1, Make = "Tesla", Model = "Model X", EngineSize = 0, FuelType = "Electric", Transmission = "Automatic", Active = true, Deleted = false };
            var mockSet = new Mock<DbSet<Vehicle>>();
            mockSet.Setup(m => m.FindAsync(1)).ReturnsAsync(vehicle);
            _dbContextMock.Setup(db => db.Vehicles).Returns(mockSet.Object);

            // Act
            var result = await _vehicleService.GetVehicleByID(1);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Make.Should().Be("Tesla");
        }

        [Fact]
        public async Task GetVehicleById_ShouldReturnNull_WhenVehicleDoesNotExist()
        {
            // Arrange
            var mockSet = new Mock<DbSet<Vehicle>>();
            mockSet.Setup(m => m.FindAsync(2)).ReturnsAsync((Vehicle)null);
            _dbContextMock.Setup(db => db.Vehicles).Returns(mockSet.Object);

            // Act
            var result = await _vehicleService.GetVehicleByID(2);

            // Assert
            result.Should().BeNull();  // Use FluentAssertions instead of Assert.Null
        }

        [Fact]
        public async Task DeactivateVehicle_ShouldSetActiveToFalse_WhenVehicleExists()
        {
            // Arrange
            var vehicle = new Vehicle { Id = 1, Active = true, Deleted = false };
            var mockSet = new Mock<DbSet<Vehicle>>();
            mockSet.Setup(m => m.FindAsync(1)).ReturnsAsync(vehicle);
            _dbContextMock.Setup(db => db.Vehicles).Returns(mockSet.Object);

            // Act
            var result = await _vehicleService.DeactivateVehicle(1);

            // Assert
            result.Should().BeTrue();
            vehicle.Active.Should().BeFalse();
            vehicle.Deleted.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteVehicle_ShouldRemoveVehicle_WhenVehicleExists()
        {
            // Arrange
            var vehicle = new Vehicle { Id = 1 };
            var mockSet = new Mock<DbSet<Vehicle>>();
            mockSet.Setup(m => m.FindAsync(1)).ReturnsAsync(vehicle);
            _dbContextMock.Setup(db => db.Vehicles).Returns(mockSet.Object);
            _dbContextMock.Setup(db => db.SaveChangesAsync(default)).ReturnsAsync(1); // Mock SaveChangesAsync

            // Act
            var result = await _vehicleService.DeleteVehicle(1);

            // Assert
            result.Should().BeTrue();
            mockSet.Verify(m => m.Remove(vehicle), Times.Once());
            _dbContextMock.Verify(db => db.SaveChangesAsync(default), Times.Once());
        }
    }
}
