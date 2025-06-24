using System;
using System.ComponentModel.DataAnnotations;

namespace PrestigeRentals.Domain.Entities
{
    /// <summary>
    /// Represents a support ticket submitted by a user.
    /// </summary>
    public class Ticket
    {
        /// <summary>
        /// Gets or sets the unique identifier of the ticket.
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the first name of the user who submitted the ticket.
        /// </summary>
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the user who submitted the ticket.
        /// </summary>
        [Required]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user submitting the ticket.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the phone number of the user submitting the ticket.
        /// </summary>
        [Required]
        [Phone]
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the description of the issue or inquiry submitted by the user.
        /// </summary>
        [Required]
        public string Description { get; set; }
    }
}
