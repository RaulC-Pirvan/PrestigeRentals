using System;

namespace PrestigeRentals.Application.Requests
{
    /// <summary>
    /// Request model for submitting a new support ticket.
    /// </summary>
    public class CreateTicketRequest
    {
        /// <summary>
        /// Gets or sets the first name of the user submitting the ticket.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the user submitting the ticket.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the email address associated with the support ticket.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the phone number provided by the user, if any.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the message or description of the issue reported by the user.
        /// </summary>
        public string Description { get; set; }
    }
}
