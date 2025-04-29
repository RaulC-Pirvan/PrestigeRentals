using PrestigeRentals.Domain.Entities;
using PrestigeRentals.Domain.Interfaces;
using PrestigeRentals.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigeRentals.Infrastructure.Repositories
{
    public class LogEntryRepository : ILogEntryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public LogEntryRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(LogEntry logEntry)
        {
            await _dbContext.Logs.AddAsync(logEntry);
            await _dbContext.SaveChangesAsync();
        }
    }
}
