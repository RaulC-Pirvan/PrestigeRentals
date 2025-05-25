using System;
using System.ComponentModel.DataAnnotations;

namespace PrestigeRentals.Domain.Entities
{
    /// <summary>
    /// Represents a vehicle in the Prestige Rentals system.
    /// </summary>
    public class Vehicle
    {
        /// <summary>
        /// Primary key for the vehicle entity.
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// Manufacturer or brand of the vehicle (e.g., BMW, Audi).
        /// </summary>
        [Required]
        public string Make { get; set; }

        /// <summary>
        /// Model name of the vehicle (e.g., A4, X5).
        /// </summary>
        [Required]
        public string Model { get; set; }

        [Required]
        public string Chassis { get; set; }

        [Required]
        public int Horsepower { get; set; }

        /// <summary>
        /// Engine size in liters (e.g., 2.0, 3.5).
        /// </summary>
        [Required]
        public int EngineSize { get; set; }

        /// <summary>
        /// Type of fuel the vehicle uses (e.g., Petrol, Diesel, Electric).
        /// </summary>
        [Required]
        public string FuelType { get; set; }

        /// <summary>
        /// Type of transmission (e.g., Automatic, Manual).
        /// </summary>
        [Required]
        public string Transmission { get; set; }

        /// <summary>
        /// Indicates whether the vehicle is currently active in the system.
        /// </summary>
        [Required]
        public bool Active { get; set; } = true;

        /// <summary>
        /// Indicates whether the vehicle has been soft-deleted.
        /// </summary>
        [Required]
        public bool Deleted { get; set; } = false;
        
        /// <summary>
        /// Indicates wheter the vehicle is currently available for renting.
        /// </summary>
        [Required]
        public bool Available { get; set; } = true;

    }
}
