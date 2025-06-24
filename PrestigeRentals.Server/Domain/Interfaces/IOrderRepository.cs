using PrestigeRentals.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PrestigeRentals.Application.Services.Interfaces.Repositories
{
    /// <summary>
    /// Interface for performing CRUD operations and queries on <see cref="Order"/> entities.
    /// </summary>
    public interface IOrderRepository
    {
        /// <summary>
        /// Retrieves an order by its unique identifier.
        /// </summary>
        /// <param name="id">The ID of the order to retrieve.</param>
        /// <returns>The order with the specified ID, or null if not found.</returns>
        Task<Order> GetByIdAsync(long id);

        /// <summary>
        /// Retrieves all orders.
        /// </summary>
        /// <returns>A collection of all orders.</returns>
        Task<IEnumerable<Order>> GetAllAsync();

        /// <summary>
        /// Adds a new order to the data store.
        /// </summary>
        /// <param name="order">The order to add.</param>
        Task AddAsync(Order order);

        /// <summary>
        /// Updates an existing order in the data store.
        /// </summary>
        /// <param name="order">The order to update.</param>
        Task UpdateAsync(Order order);

        /// <summary>
        /// Deletes an existing order from the data store.
        /// </summary>
        /// <param name="order">The order to delete.</param>
        Task DeleteAsync(Order order);

        /// <summary>
        /// Retrieves all active (non-cancelled and ongoing) orders for a given vehicle in a time range.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle.</param>
        /// <param name="from">The start of the time range.</param>
        /// <param name="to">The end of the time range.</param>
        /// <returns>A list of active orders for the specified vehicle.</returns>
        Task<List<Order>> GetActiveOrdersForVehicleAsync(long vehicleId, DateTime from, DateTime to);

        /// <summary>
        /// Retrieves all orders placed by a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A collection of orders associated with the specified user.</returns>
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(long userId);

        /// <summary>
        /// Checks if any orders match the specified condition.
        /// </summary>
        /// <param name="predicate">The expression used to filter orders.</param>
        /// <returns>True if any matching orders exist; otherwise, false.</returns>
        Task<bool> AnyAsync(Expression<Func<Order, bool>> predicate);

        /// <summary>
        /// Retrieves all active orders for a given vehicle ID.
        /// </summary>
        /// <param name="vehicleId">The vehicle ID to query.</param>
        /// <returns>A list of currently active orders.</returns>
        Task<List<Order>> GetActiveOrdersByVehicle(int vehicleId);
    }
}
