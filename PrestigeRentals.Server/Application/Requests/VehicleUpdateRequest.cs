using System;

namespace PrestigeRentals.Application.Requests
{
    /// <summary>
    /// Represents a request to update the details of a vehicle. 
    /// All properties are nullable to allow partial updates (only the provided fields will be updated).
    /// </summary>
    public class VehicleUpdateRequest
    {
        /// <summary>
        /// Gets or sets the make of the vehicle (e.g., Toyota, Ford). 
        /// This is an optional field for updating the vehicle.
        /// </summary>
        public string? Make { get; set; }

        /// <summary>
        /// Gets or sets the model of the vehicle (e.g., Camry, Focus).
        /// This is an optional field for updating the vehicle.
        /// </summary>
        public string? Model { get; set; }

        public string? Chassis { get; set; }

        public int? Horsepower { get; set; }
        /// <summary>
        /// Gets or sets the engine size of the vehicle (e.g., 2.5, 3.0).
        /// This is an optional field for updating the vehicle.
        /// </summary>
        public int? EngineSize { get; set; }

        /// <summary>
        /// Gets or sets the fuel type of the vehicle (e.g., Petrol, Diesel, Electric).
        /// This is an optional field for updating the vehicle.
        /// </summary>
        public string? FuelType { get; set; }

        /// <summary>
        /// Gets or sets the transmission type of the vehicle (e.g., Automatic, Manual).
        /// This is an optional field for updating the vehicle.
        /// </summary>
        public string? Transmission { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the vehicle has navigation functionality.
        /// This is an optional field for updating the vehicle.
        /// </summary>
        public bool? Navigation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the vehicle has a heads-up display feature.
        /// This is an optional field for updating the vehicle.
        /// </summary>
        public bool? HeadsUpDisplay { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the vehicle has hill assist functionality.
        /// This is an optional field for updating the vehicle.
        /// </summary>
        public bool? HillAssist { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the vehicle has cruise control functionality.
        /// This is an optional field for updating the vehicle.
        /// </summary>
        public bool? CruiseControl { get; set; }
    }
}
