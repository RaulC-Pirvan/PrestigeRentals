using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PrestigeRentals.Domain.Entities
{
    /// <summary>
    /// Represents additional personal details for a user in the Prestige Rentals system.
    /// </summary>
    public class UserDetails
    {
        /// <summary>
        /// Primary key for the UserDetails entity.
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// Foreign key referencing the associated User.
        /// </summary>
        [ForeignKey(nameof(User))]
        public long UserID { get; set; }

        /// <summary>
        /// Navigation property for the associated User.
        /// Marked with JsonIgnore to prevent circular references in serialization.
        /// </summary>
        [JsonIgnore]
        public User User { get; set; }

        /// <summary>
        /// First name of the user.
        /// </summary>
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// Last name of the user.
        /// </summary>
        [Required]
        public string LastName { get; set; }

        /// <summary>
        /// Date of birth of the user.
        /// </summary>
        [Required]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Profile image of the user, stored as a byte array.
        /// </summary>
        [Required]
        public byte[] ImageData { get; set; }

        /// <summary>
        /// Indicates whether this user details record is active.
        /// </summary>
        [Required]
        public bool Active { get; set; } = true;

        /// <summary>
        /// Indicates whether this user details record is marked as deleted.
        /// </summary>
        [Required]
        public bool Deleted { get; set; } = false;
    }
}
