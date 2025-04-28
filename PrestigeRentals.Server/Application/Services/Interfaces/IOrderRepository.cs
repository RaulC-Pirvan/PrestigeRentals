using PrestigeRentals.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrestigeRentals.Application.Services.Interfaces
{
    /// <summary>
    /// Interface for performing CRUD operations on Order entities.
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
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddAsync(Order order);

        /// <summary>
        /// Updates an existing order in the data store.
        /// </summary>
        /// <param name="order">The order to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateAsync(Order order);

        /// <summary>
        /// Deletes an existing order from the data store.
        /// </summary>
        /// <param name="order">The order to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteAsync(Order order);
    }
}
