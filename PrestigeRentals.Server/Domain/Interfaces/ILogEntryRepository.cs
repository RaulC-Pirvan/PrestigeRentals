using PrestigeRentals.Domain.Entities;

namespace PrestigeRentals.Domain.Interfaces
{
    public interface ILogEntryRepository
    {
        Task AddAsync(LogEntry logEntry);
    }
}
