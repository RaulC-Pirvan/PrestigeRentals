using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PrestigeRentals.Domain.Entities
{
    /// <summary>
    /// Represents a photo associated with a specific vehicle.
    /// </summary>
    public class VehiclePhotos
    {
        /// <summary>
        /// Primary key for the vehicle photo entity.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Foreign key referencing the associated vehicle.
        /// </summary>
        [ForeignKey("VehicleId")]
        public int VehicleId { get; set; }

        /// <summary>
        /// Navigation property for the associated vehicle.
        /// </summary>
        [JsonIgnore]
        public Vehicle Vehicle { get; set; }

        /// <summary>
        /// The raw image data for the photo, stored as a byte array.
        /// </summary>
        [Required]
        public byte[] ImageData { get; set; }

        /// <summary>
        /// Indicates whether the photo is active and in use.
        /// </summary>
        [Required]
        public bool Active { get; set; } = true;

        /// <summary>
        /// Indicates whether the photo has been soft-deleted.
        /// </summary>
        [Required]
        public bool Deleted { get; set; } = false;
    }
}
