using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PrestigeRentals.Domain.Entities
{
    /// <summary>
    /// Represents additional optional features for a specific vehicle.
    /// </summary>
    public class VehicleOptions
    {
        /// <summary>
        /// Primary key for the vehicle options entity.
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// Foreign key referencing the associated vehicle.
        /// </summary>
        [ForeignKey("VehicleId")]
        public long VehicleId { get; set; }

        /// <summary>
        /// Navigation property for the associated vehicle.
        /// </summary>
        [JsonIgnore]
        public Vehicle Vehicle { get; set; }

        /// <summary>
        /// Indicates whether the vehicle has a navigation system.
        /// </summary>
        [Required]
        public bool Navigation { get; set; } = false;

        /// <summary>
        /// Indicates whether the vehicle has a heads-up display.
        /// </summary>
        [Required]
        public bool HeadsUpDisplay { get; set; } = false;

        /// <summary>
        /// Indicates whether the vehicle has a hill assist feature.
        /// </summary>
        [Required]
        public bool HillAssist { get; set; } = false;

        /// <summary>
        /// Indicates whether the vehicle has cruise control.
        /// </summary>
        [Required]
        public bool CruiseControl { get; set; } = false;

        /// <summary>
        /// Indicates whether this options entry is active.
        /// </summary>
        [Required]
        public bool Active { get; set; } = true;

        /// <summary>
        /// Indicates whether this options entry has been soft-deleted.
        /// </summary>
        [Required]
        public bool Deleted { get; set; } = false;
    }
}
