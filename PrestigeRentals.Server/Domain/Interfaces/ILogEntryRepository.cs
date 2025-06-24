using System.Threading.Tasks;
using PrestigeRentals.Domain.Entities;

namespace PrestigeRentals.Domain.Interfaces
{
    /// <summary>
    /// Defines the contract for logging operations related to persistent log entries.
    /// </summary>
    public interface ILogEntryRepository
    {
        /// <summary>
        /// Adds a new log entry to the data store.
        /// </summary>
        /// <param name="logEntry">The log entry to be recorded.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddAsync(LogEntry logEntry);
    }
}
