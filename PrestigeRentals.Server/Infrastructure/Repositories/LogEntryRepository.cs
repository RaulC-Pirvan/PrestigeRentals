using System.Threading.Tasks;
using PrestigeRentals.Domain.Entities;
using PrestigeRentals.Domain.Interfaces;
using PrestigeRentals.Infrastructure.Persistence;

namespace PrestigeRentals.Infrastructure.Repositories
{
    /// <summary>
    /// Repository for managing persistent log entries using the application's database context.
    /// Implements <see cref="ILogEntryRepository"/>.
    /// </summary>
    public class LogEntryRepository : ILogEntryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogEntryRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context used for data access.</param>
        public LogEntryRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Adds a new log entry asynchronously to the database and saves changes.
        /// </summary>
        /// <param name="logEntry">The log entry to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task AddAsync(LogEntry logEntry)
        {
            await _dbContext.Logs.AddAsync(logEntry);
            await _dbContext.SaveChangesAsync();
        }
    }
}
