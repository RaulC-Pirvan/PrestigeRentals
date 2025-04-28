using PrestigeRentals.Application.DTO;
using PrestigeRentals.Application.Requests;
using PrestigeRentals.Application.Services.Interfaces;
using PrestigeRentals.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrestigeRentals.Application.Services
{
    /// <summary>
    /// Service class for managing order operations like creation, retrieval, cancellation, etc.
    /// Implements the IOrderService interface.
    /// </summary>
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        /// <summary>
        /// Constructor to initialize the service with the order repository.
        /// </summary>
        /// <param name="orderRepository">The repository used to interact with the order data.</param>
        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        /// <summary>
        /// Creates a new order from the provided request and saves it to the database.
        /// </summary>
        /// <param name="createOrderRequest">The request containing order details.</param>
        /// <returns>A DTO containing the created order's details.</returns>
        public async Task<OrderDTO> CreateOrder(CreateOrderRequest createOrderRequest)
        {
            // Create a new order entity
            Order order = new Order
            {
                UserId = createOrderRequest.UserId,
                VehicleId = createOrderRequest.VehicleId,
                StartTime = createOrderRequest.StartTime,
                EndTime = createOrderRequest.EndTime
            };

            // Save the new order in the database
            await _orderRepository.AddAsync(order);

            // Return a DTO representation of the created order
            return new OrderDTO
            {
                Id = order.Id,
                UserId = order.UserId,
                VehicleId = order.VehicleId,
                StartTime = order.StartTime,
                EndTime = order.EndTime
            };
        }

        /// <summary>
        /// Retrieves all orders from the repository and returns them as a list of DTOs.
        /// </summary>
        /// <returns>A collection of order DTOs.</returns>
        public async Task<IEnumerable<OrderDTO>> GetAllOrders()
        {
            // Retrieve all orders from the repository
            IEnumerable<Order> orders = await _orderRepository.GetAllAsync();
            List<OrderDTO> orderDTOs = new List<OrderDTO>();

            // Convert each order entity into an OrderDTO
            foreach (var order in orders)
            {
                orderDTOs.Add(new OrderDTO
                {
                    Id = order.Id,
                    UserId = order.UserId,
                    VehicleId = order.VehicleId,
                    StartTime = order.StartTime,
                    EndTime = order.EndTime,
                    IsCancelled = order.IsCancelled
                });
            }

            // Return the collection of order DTOs
            return orderDTOs;
        }

        /// <summary>
        /// Retrieves an order by its ID and returns it as a DTO.
        /// </summary>
        /// <param name="id">The ID of the order to retrieve.</param>
        /// <returns>The order DTO if found, otherwise null.</returns>
        public async Task<OrderDTO> GetOrderById(long id)
        {
            // Retrieve the order by its ID
            Order order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                return null;

            // Return the order details as a DTO
            return new OrderDTO
            {
                Id = order.Id,
                UserId = order.UserId,
                VehicleId = order.VehicleId,
                StartTime = order.StartTime,
                EndTime = order.EndTime,
                IsCancelled = order.IsCancelled
            };
        }

        /// <summary>
        /// Cancels an order by setting its IsCancelled flag to true and saving the changes.
        /// </summary>
        /// <param name="id">The ID of the order to cancel.</param>
        /// <returns>True if the order was successfully cancelled, otherwise false.</returns>
        public async Task<bool> CancelOrder(long id)
        {
            // Retrieve the order by its ID
            Order order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                return false;

            // Set the order as cancelled
            order.IsCancelled = true;

            // Update the order in the repository
            await _orderRepository.UpdateAsync(order);

            // Return true indicating the order was successfully cancelled
            return true;
        }
    }
}
