using Microsoft.EntityFrameworkCore;
using PrestigeRentals.Application.DTO;
using PrestigeRentals.Application.Services.Interfaces;
using PrestigeRentals.Domain.Entities;
using PrestigeRentals.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigeRentals.Application.Services
{
    public class LogService : ILogService
    {
        private readonly ApplicationDbContext _dbContext;

        public LogService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task LogAsync(LogEntryDTO entry)
        {
            var log = new LogEntry
            {
                Path = entry.Path,
                Method = entry.Method,
                StatusCode = entry.StatusCode,
                DurationMs = entry.DurationMs,
                Timestamp = entry.Timestamp
            };

            _dbContext.LogEntries.Add(log);
            await _dbContext.SaveChangesAsync();
        }
    }
}
