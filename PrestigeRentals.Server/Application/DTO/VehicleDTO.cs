using System;

namespace PrestigeRentals.Application.DTO
{
    /// <summary>
    /// Represents the data transfer object (DTO) for a vehicle.
    /// This class contains the properties of a vehicle, including make, model, engine size, fuel type, and various features.
    /// </summary>
    public class VehicleDTO
    {
        public long Id { get; set; }
        /// <summary>
        /// Gets or sets the make of the vehicle (e.g., Toyota, Ford, etc.).
        /// </summary>
        public string Make { get; set; }

        /// <summary>
        /// Gets or sets the model of the vehicle (e.g., Corolla, Mustang, etc.).
        /// </summary>
        public string Model { get; set; }

        public string Chassis { get; set; }

        public int Horsepower { get; set; }

        public decimal PricePerDay { get; set; }

        /// <summary>
        /// Gets or sets the engine size of the vehicle in liters.
        /// </summary>
        public decimal EngineSize { get; set; }

        /// <summary>
        /// Gets or sets the fuel type of the vehicle (e.g., Petrol, Diesel, Electric, etc.).
        /// </summary>
        public string FuelType { get; set; }

        /// <summary>
        /// Gets or sets the transmission type of the vehicle (e.g., Automatic, Manual).
        /// </summary>
        public string Transmission { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the vehicle has navigation features.
        /// </summary>
        public bool Navigation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the vehicle has a heads-up display.
        /// </summary>
        public bool HeadsUpDisplay { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the vehicle is equipped with hill assist.
        /// </summary>
        public bool HillAssist { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the vehicle is equipped with cruise control.
        /// </summary>
        public bool CruiseControl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the vehicle is active or available.
        /// </summary>
        public bool Active { get; set; }

        public bool Available { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the vehicle is marked as deleted.
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleDTO"/> class.
        /// </summary>
        public VehicleDTO() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleDTO"/> class with specified values.
        /// </summary>
        /// <param name="make">The make of the vehicle.</param>
        /// <param name="model">The model of the vehicle.</param>
        /// <param name="engineSize">The engine size of the vehicle.</param>
        /// <param name="fuelType">The fuel type of the vehicle.</param>
        /// <param name="transmission">The transmission type of the vehicle.</param>
        /// <param name="navigation">Indicates whether the vehicle has navigation features.</param>
        /// <param name="headsUpDisplay">Indicates whether the vehicle has a heads-up display.</param>
        /// <param name="hillAssist">Indicates whether the vehicle has hill assist.</param>
        /// <param name="cruiseControl">Indicates whether the vehicle has cruise control.</param>
        /// <param name="active">Indicates whether the vehicle is active.</param>
        /// <param name="deleted">Indicates whether the vehicle is marked as deleted.</param>
        public VehicleDTO(long id, string make, string model, decimal engineSize, string fuelType, string transmission,
                          bool navigation, bool headsUpDisplay, bool hillAssist, bool cruiseControl, bool available,
                          bool active, bool deleted)
        {
            Id = id;
            Make = make;
            Model = model;
            EngineSize = engineSize;
            FuelType = fuelType;
            Transmission = transmission;
            Navigation = navigation;
            HeadsUpDisplay = headsUpDisplay;
            HillAssist = hillAssist;
            CruiseControl = cruiseControl;
            Available = available;
            Active = active;
            Deleted = deleted;
        }
    }
}
