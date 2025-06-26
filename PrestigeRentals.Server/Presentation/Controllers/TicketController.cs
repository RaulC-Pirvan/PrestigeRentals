using Microsoft.AspNetCore.Mvc;
using PrestigeRentals.Application.Requests;
using PrestigeRentals.Application.Services.Interfaces;
using System.Threading.Tasks;

namespace PrestigeRentals.Presentation.Controllers
{
    /// <summary>
    /// Controller responsible for ticket-related actions such as submitting support tickets and retrieving tickets.
    /// </summary>
    [ApiController]
    [Route("api/ticket")]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        private readonly IEmailService _emailService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketController"/> class.
        /// </summary>
        /// <param name="ticketService">Service for ticket operations.</param>
        /// <param name="emailService">Service for sending emails.</param>
        public TicketController(ITicketService ticketService, IEmailService emailService)
        {
            _ticketService = ticketService;
            _emailService = emailService;
        }

        /// <summary>
        /// Submits a new support ticket.
        /// </summary>
        /// <param name="request">The ticket creation request.</param>
        /// <returns>Returns the ID of the created ticket or validation errors.</returns>
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

        /// <summary>
        /// Retrieves all support tickets.
        /// </summary>
        /// <returns>A list of all tickets.</returns>
        [HttpGet]
        public async Task<IActionResult> GetTickets()
        {
            var tickets = await _ticketService.GetAllTicketsAsync();
            return Ok(tickets);
        }

        /// <summary>
        /// Retrieves a support ticket by its unique identifier.
        /// </summary>
        /// <param name="id">The ID of the ticket to retrieve.</param>
        /// <returns>The ticket details if found; otherwise, NotFound.</returns>
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
