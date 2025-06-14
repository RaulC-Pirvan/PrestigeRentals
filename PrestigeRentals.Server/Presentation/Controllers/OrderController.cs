
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrestigeRentals.Application.DTO;
using PrestigeRentals.Application.Exceptions;
using PrestigeRentals.Application.Requests;
using PrestigeRentals.Application.Services.Interfaces;

namespace PrestigeRentals.Presentation.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Creates a new order in the system.
        /// </summary>
        /// <param name="createOrderRequest">The request containing the details of the order to create.</param>
        /// <returns>The created order.</returns>   
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<OrderDTO>> CreateOrder([FromBody] CreateOrderRequest createOrderRequest)
        {
            if (createOrderRequest == null)
                return BadRequest("Invalid data.");

            try
            {
                var createdOrder = await _orderService.CreateOrder(createOrderRequest);
                return Ok(createdOrder);
            }

            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all orders in the system.
        /// </summary>
        /// <returns>A list of all orders.</returns>
        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetAllOrders()
        {
            try
            {
                var orders = await _orderService.GetAllOrders();
                return Ok(orders);
            }

            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a specific order by its ID.
        /// </summary>
        /// <param name="id">The ID of the order to retrieve.</param>
        /// <returns>The order with the specified ID.</returns>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<OrderDTO>> GetOrderById(long id)
        {
            try
            {
                var order = await _orderService.GetOrderById(id);
                if (order == null)
                    return NotFound();

                return Ok(order);
            }

            catch (OrderNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        /// <summary>
        /// Cancels an existing order by its ID.
        /// </summary>
        /// <param name="id">The ID of the order to cancel.</param>
        /// <returns>A status indicating whether the cancellation was successful.</returns>
        [HttpPut("cancel/{id}")]
        [Authorize]
        public async Task<ActionResult> CancelOrder(long id)
        {
            try
            {
                var result = await _orderService.CancelOrder(id);
                if (!result)
                    return NotFound("Order not found or already cancelled.");
                return NoContent();
            }

            catch (OrderAlreadyCancelledException ex)
            {
                return Conflict(ex.Message);
            }

            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }

        }

        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrdersForUser()
        {
            try
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdString) || !long.TryParse(userIdString, out var userId))
                {
                    return Unauthorized();
                }

                var orders = await _orderService.GetOrdersByUserIdAsync(userId);
                return Ok(orders);
            }

            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

    }
}
