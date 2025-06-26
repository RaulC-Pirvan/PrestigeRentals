using System.Collections.Generic;
using System.Threading.Tasks;
using PrestigeRentals.Domain.Entities;

namespace PrestigeRentals.Domain.Interfaces
{
    /// <summary>
    /// Defines the contract for data access operations on <see cref="Ticket"/> entities.
    /// </summary>
    public interface ITicketRepository
    {
        /// <summary>
        /// Retrieves a support ticket by its unique identifier.
        /// </summary>
        /// <param name="id">The ID of the ticket to retrieve.</param>
        /// <returns>The ticket if found; otherwise, null.</returns>
        Task<Ticket> GetByIdAsync(long id);

        /// <summary>
        /// Retrieves all support tickets.
        /// </summary>
        /// <returns>A collection of all tickets.</returns>
        Task<IEnumerable<Ticket>> GetAllAsync();

        /// <summary>
        /// Adds a new support ticket to the data store.
        /// </summary>
        /// <param name="ticket">The ticket to add.</param>
        Task AddAsync(Ticket ticket);
    }
}
