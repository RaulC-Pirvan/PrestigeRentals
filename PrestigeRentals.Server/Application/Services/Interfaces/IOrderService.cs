using PrestigeRentals.Application.DTO;
using PrestigeRentals.Application.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrestigeRentals.Application.Services.Interfaces
{
    /// <summary>
    /// Interface defining the service operations for managing orders.
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        /// Creates a new order based on the provided request data.
        /// </summary>
        /// <param name="createOrderRequest">The request containing the necessary data to create an order.</param>
        /// <returns>The created order as a Data Transfer Object (DTO).</returns>
        Task<OrderDTO> CreateOrder(CreateOrderRequest createOrderRequest);

        /// <summary>
        /// Retrieves all orders in the system.
        /// </summary>
        /// <returns>A collection of all orders represented as DTOs.</returns>
        Task<IEnumerable<OrderDTO>> GetAllOrders();

        /// <summary>
        /// Retrieves an order by its unique identifier.
        /// </summary>
        /// <param name="id">The ID of the order to retrieve.</param>
        /// <returns>The order matching the provided ID as a DTO, or null if not found.</returns>
        Task<OrderDTO> GetOrderById(long id);

        /// <summary>
        /// Cancels an order by its ID.
        /// </summary>
        /// <param name="id">The ID of the order to cancel.</param>
        /// <returns>A boolean value indicating whether the cancellation was successful.</returns>
        Task<bool> CancelOrder(long id);

        /// <summary>
        /// Retrieves all orders placed by a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user whose orders should be fetched.</param>
        /// <returns>A collection of order DTOs associated with the specified user.</returns>
        Task<IEnumerable<OrderDTO>> GetOrdersByUserIdAsync(long userId);
    }
}
