namespace PrestigeRentals.Application.DTO
{
    /// <summary>
    /// Represents a simplified log entry for API requests and responses.
    /// </summary>
    public class LogEntryDTO
    {
        /// <summary>
        /// The path of the HTTP request.
        /// </summary>
        public string Path { get; set; } = string.Empty;

        /// <summary>
        /// The HTTP method used for the request (e.g., GET, POST).
        /// </summary>
        public string Method { get; set; } = string.Empty;

        /// <summary>
        /// The HTTP response status code.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// The duration of the request in milliseconds.
        /// </summary>
        public long DurationMs { get; set; }

        /// <summary>
        /// The UTC timestamp when the request was logged.
        /// </summary>
        public DateTime Timestamp { get; set; }
    }
}
