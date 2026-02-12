using System;
using System.ComponentModel.DataAnnotations;

namespace PrestigeRentals.Domain.Entities
{
    /// <summary>
    /// Represents a log entry capturing metadata about an HTTP request and response.
    /// </summary>
    public class LogEntry
    {
        /// <summary>
        /// Primary key of the log entry.
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// The URL path of the HTTP request.
        /// </summary>
        [Required]
        public string? Path { get; set; }

        /// <summary>
        /// The HTTP method used in the request (e.g., GET, POST).
        /// </summary>
        [Required]
        public string? Method { get; set; }

        /// <summary>
        /// The HTTP status code returned in the response.
        /// </summary>
        [Required]
        public int? StatusCode { get; set; }

        /// <summary>
        /// Raw body of the HTTP request.
        /// </summary>
        [Required]
        public string? RequestBody { get; set; }

        /// <summary>
        /// Raw body of the HTTP response.
        /// </summary>
        [Required]
        public string? ResponseBody { get; set; }

        /// <summary>
        /// Identifier for the user who made the request (if authenticated).
        /// </summary>
        [Required]
        public string? UserId { get; set; }

        /// <summary>
        /// Time taken to handle the request, in milliseconds.
        /// </summary>
        [Required]
        public long DurationMs { get; set; }

        /// <summary>
        /// The UTC timestamp when the request was processed.
        /// </summary>
        [Required]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
