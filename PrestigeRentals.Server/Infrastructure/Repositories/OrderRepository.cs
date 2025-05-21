using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PrestigeRentals.Application.Services.Interfaces.Repositories;
using PrestigeRentals.Domain.Entities;
using PrestigeRentals.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrestigeRentals.Application.Services
{
    /// <summary>
    /// Repository for managing order data in the database.
    /// Implements the IOrderRepository interface.
    /// </summary>
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<OrderRepository> _logger;

        /// <summary>
        /// Constructor to initialize the repository with a database context.
        /// </summary>
        /// <param name="dbContext">The database context used to interact with the database.</param>
        /// <param name="logger">Logger instance to log repository activities.</param>
        public OrderRepository(ApplicationDbContext dbContext, ILogger<OrderRepository> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext), "Database context cannot be null.");
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null.");
        }

        /// <summary>
        /// Retrieves an order by its unique identifier.
        /// </summary>
        /// <param name="id">The ID of the order to retrieve.</param>
        /// <returns>The order if found, otherwise null.</returns>
        public async Task<Order> GetByIdAsync(long id)
        {
            return await _dbContext.Orders.FindAsync(id);
        }

        /// <summary>
        /// Retrieves all orders from the database.
        /// </summary>
        /// <returns>A list of all orders.</returns>
        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _dbContext.Orders.ToListAsync();
        }

        /// <summary>
        /// Adds a new order to the database.
        /// </summary>
        /// <param name="order">The order entity to be added.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task AddAsync(Order order)
        {
            if(order == null)
            {
                _logger.LogError("Attempted to add a null order to the database.");
                throw new ArgumentNullException(nameof(order), "Order cannot be null.");
            }

            try
            {
                _logger.LogInformation("Attempting to add a new order to the database.");
                _dbContext.Orders.Add(order);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Order with ID {order.Id} has been successfully added.");
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error saving order: {ex.Message}");
                throw new InvalidOperationException("An error occurred while saving the order.", ex);
            }
        }

        /// <summary>
        /// Updates an existing order in the database.
        /// </summary>
        /// <param name="order">The order entity with updated values.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task UpdateAsync(Order order)
        {
            if (order == null)
            {
                _logger.LogError("Attempted to update a null order to the database.");
                throw new ArgumentNullException(nameof(order), "Order cannot be null.");
            }
            try
            {
                _logger.LogInformation("Attempting to update an order to the database.");
                _dbContext.Orders.Update(order);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Order with ID {order.Id} has been successfully updated.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating order: {ex.Message}");
                throw new InvalidOperationException("An error occurred while updating the order.", ex);
            }
        }

        /// <summary>
        /// Removes an order from the database.
        /// </summary>
        /// <param name="order">The order entity to be removed.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeleteAsync(Order order)
        {
            if (order == null)
            {
                _logger.LogError("Attempted to delete a null order from the database.");
                throw new ArgumentNullException(nameof(order), "Order cannot be null.");
            }
            try
            {
                _logger.LogInformation("Attempting to delete an order from the database.");
                _dbContext.Orders.Remove(order);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Order with ID {order.Id} has been successfully deleted.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting order: {ex.Message}");
                throw new InvalidOperationException("An error occurred while deleting the order.", ex);
            }
        }

        public async Task<List<Order>> GetActiveOrdersForVehicleAsync(long vehicleId, DateTime from, DateTime to)
        {
            return await _dbContext.Orders.Where(o => o.VehicleId == vehicleId && !o.IsCancelled && o.StartTime <= from && o.EndTime >= to).ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(long userId)
        {
            return await _dbContext.Orders.Where(o => o.UserId == userId).ToListAsync();
        }
    }   
}
