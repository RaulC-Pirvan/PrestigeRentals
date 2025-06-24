using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PrestigeRentals.Domain.Entities;
using PrestigeRentals.Domain.Interfaces;
using PrestigeRentals.Infrastructure.Persistence;

namespace PrestigeRentals.Infrastructure.Repositories
{
    /// <summary>
    /// Repository implementation for managing <see cref="Ticket"/> entities in the database.
    /// Implements <see cref="ITicketRepository"/>.
    /// </summary>
    public class TicketRepository : ITicketRepository
    {
        private readonly ApplicationDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The application's database context.</param>
        public TicketRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Retrieves a ticket by its unique identifier.
        /// </summary>
        /// <param name="id">The ID of the ticket.</param>
        /// <returns>The ticket entity if found; otherwise, null.</returns>
        public async Task<Ticket> GetByIdAsync(long id)
        {
            return await _dbContext.Tickets.FindAsync(id);
        }

        /// <summary>
        /// Retrieves all tickets from the database.
        /// </summary>
        /// <returns>A collection of all ticket entities.</returns>
        public async Task<IEnumerable<Ticket>> GetAllAsync()
        {
            return await _dbContext.Tickets.ToListAsync();
        }

        /// <summary>
        /// Adds a new ticket entity to the database.
        /// </summary>
        /// <param name="ticket">The ticket entity to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task AddAsync(Ticket ticket)
        {
            await _dbContext.Tickets.AddAsync(ticket);
            await _dbContext.SaveChangesAsync();
        }
    }
}
