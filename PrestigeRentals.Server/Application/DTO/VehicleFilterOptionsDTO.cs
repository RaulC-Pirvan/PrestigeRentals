using System;
using System.Collections.Generic;

namespace PrestigeRentals.Application.DTO
{
    /// <summary>
    /// Data Transfer Object representing available filter options for vehicle searches.
    /// This is typically used to populate dropdowns or checkboxes in the UI.
    /// </summary>
    public class VehicleFilterOptionsDto
    {
        /// <summary>
        /// Gets or sets the list of available vehicle makes (e.g., Toyota, BMW).
        /// </summary>
        public List<string> Makes { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of available vehicle models.
        /// </summary>
        public List<string> Models { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of supported fuel types (e.g., Petrol, Diesel, Electric).
        /// </summary>
        public List<string> FuelTypes { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of available transmission types (e.g., Manual, Automatic).
        /// </summary>
        public List<string> Transmissions { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of chassis types or identifiers (e.g., SUV, Sedan, Coupe).
        /// </summary>
        public List<string> Chassis { get; set; } = new();
    }
}
