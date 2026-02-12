using System;

namespace PrestigeRentals.Application.DTO
{
    /// <summary>
    /// Represents the data transfer object (DTO) for vehicle options.
    /// This class contains various optional features available for a vehicle such as navigation, 
    /// heads-up display, hill assist, and cruise control.
    /// </summary>
    public class VehicleOptionsDTO
    {
        /// <summary>
        /// Gets or sets a value indicating whether the vehicle has navigation capabilities.
        /// </summary>
        public bool Navigation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the vehicle has a heads-up display feature.
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
        /// Gets or sets a value indicating whether the vehicle is currently active and available for use.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the vehicle has been marked as deleted.
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleOptionsDTO"/> class.
        /// </summary>
        public VehicleOptionsDTO() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleOptionsDTO"/> class with specified values.
        /// </summary>
        /// <param name="navigation">Indicates whether the vehicle has navigation features.</param>
        /// <param name="headsUpDisplay">Indicates whether the vehicle has a heads-up display.</param>
        /// <param name="hillAssist">Indicates whether the vehicle has hill assist.</param>
        /// <param name="cruiseControl">Indicates whether the vehicle has cruise control.</param>
        /// <param name="active">Indicates whether the vehicle is active and available.</param>
        /// <param name="deleted">Indicates whether the vehicle has been marked as deleted.</param>
        public VehicleOptionsDTO(bool navigation, bool headsUpDisplay, bool hillAssist, bool cruiseControl,
                                 bool active, bool deleted)
        {
            Navigation = navigation;
            HeadsUpDisplay = headsUpDisplay;
            HillAssist = hillAssist;
            CruiseControl = cruiseControl;
            Active = active;
            Deleted = deleted;
        }
    }
}
