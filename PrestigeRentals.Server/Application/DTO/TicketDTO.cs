using System;

namespace PrestigeRentals.Application.DTO
{
    /// <summary>
    /// Data Transfer Object representing a support ticket submitted by a user.
    /// </summary>
    public class TicketDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the support ticket.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the first name of the user who submitted the ticket.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the user who submitted the ticket.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the email address associated with the support ticket.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the phone number provided by the user.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the description of the issue or request made by the user.
        /// </summary>
        public string Description { get; set; }
    }
}
