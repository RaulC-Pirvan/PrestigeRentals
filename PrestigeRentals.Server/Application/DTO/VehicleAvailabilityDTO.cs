using System;

namespace PrestigeRentals.Application.DTO
{
    /// <summary>
    /// Data Transfer Object representing the availability status of a vehicle.
    /// </summary>
    public class VehicleAvailabilityDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the vehicle.
        /// </summary>
        public long VehicleID { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer of the vehicle (e.g., Toyota, BMW).
        /// </summary>
        public string Make { get; set; }

        /// <summary>
        /// Gets or sets the specific model of the vehicle.
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the vehicle is currently available for rental.
        /// </summary>
        public bool IsAvailable { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the vehicle will next become available, if it is currently unavailable.
        /// Null if the vehicle is available or availability is unknown.
        /// </summary>
        public DateTime? AvailableAt { get; set; }
    }
}
