using System.Collections.Generic;
using System.Threading.Tasks;
using PrestigeRentals.Application.DTO;
using PrestigeRentals.Application.Requests;

namespace PrestigeRentals.Application.Services.Interfaces
{
    /// <summary>
    /// Defines operations for managing support tickets in the system.
    /// </summary>
    public interface ITicketService
    {
        /// <summary>
        /// Creates a new support ticket based on user input.
        /// </summary>
        /// <param name="request">The request containing ticket details.</param>
        /// <returns>The unique identifier (ID) of the newly created ticket.</returns>
        Task<long> CreateTicketAsync(CreateTicketRequest request);

        /// <summary>
        /// Retrieves all support tickets in the system.
        /// </summary>
        /// <returns>A list of all submitted support tickets as DTOs.</returns>
        Task<List<TicketDTO>> GetAllTicketsAsync();

        /// <summary>
        /// Retrieves a specific support ticket by its ID.
        /// </summary>
        /// <param name="id">The ID of the ticket to retrieve.</param>
        /// <returns>The matching ticket as a DTO, or null if not found.</returns>
        Task<TicketDTO> GetTicketByIdAsync(long id);
    }
}
