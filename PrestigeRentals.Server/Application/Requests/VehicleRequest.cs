using System;

namespace PrestigeRentals.Application.Requests
{
    /// <summary>
    /// Represents a request to create or update a vehicle in the system.
    /// This includes details about the vehicle's make, model, features, and specifications.
    /// </summary>
    public class VehicleRequest
    {
        /// <summary>
        /// Gets or sets the make of the vehicle (e.g., Toyota, Ford, etc.).
        /// </summary>
        public string Make { get; set; }

        /// <summary>
        /// Gets or sets the model of the vehicle (e.g., Camry, Focus, etc.).
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Gets or sets the engine size of the vehicle (e.g., 2.5, 3.0, etc.).
        /// </summary>
        public decimal EngineSize { get; set; }

        /// <summary>
        /// Gets or sets the fuel type of the vehicle (e.g., Petrol, Diesel, Electric).
        /// </summary>
        public string FuelType { get; set; }

        /// <summary>
        /// Gets or sets the transmission type of the vehicle (e.g., Automatic, Manual).
        /// </summary>
        public string Transmission { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the vehicle has navigation functionality.
        /// </summary>
        public bool Navigation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the vehicle has a heads-up display feature.
        /// </summary>
        public bool HeadsUpDisplay { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the vehicle has hill assist functionality.
        /// </summary>
        public bool HillAssist { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the vehicle has cruise control functionality.
        /// </summary>
        public bool CruiseControl { get; set; }
    }
}
