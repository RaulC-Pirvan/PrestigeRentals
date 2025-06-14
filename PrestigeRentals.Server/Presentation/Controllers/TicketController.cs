using System.Formats.Asn1;
using Microsoft.AspNetCore.Mvc;
using PrestigeRentals.Application.Requests;
using PrestigeRentals.Application.Services.Interfaces;

namespace PrestigeRentals.Presentation.Controllers
{
    [ApiController]
    [Route("api/ticket")]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        private readonly IEmailService _emailService;

        public TicketController(ITicketService ticketService, IEmailService emailService)
        {
            _ticketService = ticketService;
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitTicket([FromBody] CreateTicketRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ticketId = await _ticketService.CreateTicketAsync(request);

            await _emailService.SendContactFormTicketToAdminAsync(userEmail: request.Email, message: request.Description);

            return Ok(new { id = ticketId });
        }

        [HttpGet]
        public async Task<IActionResult> GetTickets()
        {
            var tickets = await _ticketService.GetAllTicketsAsync();
            return Ok(tickets);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTicketById(long id)
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id);
            if (ticket == null)
                return NotFound();

            return Ok(ticket);
        }
    }
}
