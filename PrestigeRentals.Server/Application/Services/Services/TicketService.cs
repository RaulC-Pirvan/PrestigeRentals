using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrestigeRentals.Application.DTO;
using PrestigeRentals.Application.Requests;
using PrestigeRentals.Application.Services.Interfaces;
using PrestigeRentals.Domain.Entities;
using PrestigeRentals.Domain.Interfaces;

namespace PrestigeRentals.Application.Services.Services
{
    /// <summary>
    /// Service responsible for managing user support tickets, including creation and retrieval.
    /// Implements <see cref="ITicketService"/>.
    /// </summary>
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketService"/> class.
        /// </summary>
        /// <param name="ticketRepository">The repository used for ticket data access.</param>
        public TicketService(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        /// <summary>
        /// Creates a new support ticket based on the provided request data.
        /// </summary>
        /// <param name="request">The ticket creation request containing user input.</param>
        /// <returns>The ID of the newly created ticket.</returns>
        public async Task<long> CreateTicketAsync(CreateTicketRequest request)
        {
            var ticket = new Ticket
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Phone = request.Phone,
                Description = request.Description,
            };

            await _ticketRepository.AddAsync(ticket);
            return ticket.Id;
        }


        /// <summary>
        /// Retrieves all submitted support tickets.
        /// </summary>
        /// <returns>A list of all tickets represented as DTOs.</returns>
        public async Task<List<TicketDTO>> GetAllTicketsAsync()
        {
            var tickets = await _ticketRepository.GetAllAsync();

            return tickets.Select(t => new TicketDTO
            {
                Id = t.Id,
                FirstName = t.FirstName,
                LastName = t.LastName,
                Email = t.Email,
                Phone = t.Phone,
                Description = t.Description
            }).ToList();
        }


        /// <summary>
        /// Retrieves a specific support ticket by its ID.
        /// </summary>
        /// <param name="id">The ID of the ticket to retrieve.</param>
        /// <returns>The matching ticket as a DTO, or null if not found.</returns>
        public async Task<TicketDTO> GetTicketByIdAsync(long id)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);
            if (ticket == null)
                return null;

            return new TicketDTO
            {
                Id = ticket.Id,
                FirstName = ticket.FirstName,
                LastName = ticket.LastName,
                Email = ticket.Email,
                Phone = ticket.Phone,
                Description = ticket.Description,
            };
        }

    }
}
