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
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;

        public TicketService(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }
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
