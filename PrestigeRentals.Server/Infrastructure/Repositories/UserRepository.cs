using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PrestigeRentals.Application.Services.Interfaces.Repositories;
using PrestigeRentals.Domain.Entities;
using PrestigeRentals.Infrastructure.Persistence;

namespace PrestigeRentals.Application.Services
{
    /// <summary>
    /// Repository responsible for user-related CRUD operations in the database.
    /// Implements <see cref="IUserRepository"/>.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IUserDetailsRepository _userDetailsRepository;
        private readonly ILogger<UserRepository> _logger;

        /// <summary>
        /// Constructs a new instance of the <see cref="UserRepository"/>.
        /// </summary>
        /// <param name="dbContext">The database context used for data operations.</param>
        /// <param name="userDetailsRepository">Repository for user details.</param>
        /// <param name="logger">Logger for logging repository actions.</param>
        /// <exception cref="ArgumentNullException">Thrown if any parameter is null.</exception>
        public UserRepository(
            ApplicationDbContext dbContext,
            IUserDetailsRepository userDetailsRepository,
            ILogger<UserRepository> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _userDetailsRepository = userDetailsRepository ?? throw new ArgumentNullException(nameof(userDetailsRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Retrieves a user by their email address.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <returns>The matching <see cref="User"/> if found; otherwise, null.</returns>
        /// <exception cref="InvalidOperationException">Thrown if there is an error during retrieval.</exception>
        public async Task<User> GetUserByEmail(string email)
        {
            try
            {
                _logger.LogInformation("Fetching user by email: {Email}", email);
                return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user by email: {Email}", email);
                throw new InvalidOperationException("Error fetching user by email.", ex);
            }
        }

        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="userId">The user's ID.</param>
        /// <returns>The matching <see cref="User"/> if found; otherwise, null.</returns>
        /// <exception cref="InvalidOperationException">Thrown if there is an error during retrieval.</exception>
        public async Task<User> GetUserById(long userId)
        {
            try
            {
                _logger.LogInformation("Fetching user by ID: {UserId}", userId);
                return await _dbContext.Users.FindAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user by ID: {UserId}", userId);
                throw new InvalidOperationException("Error fetching user by ID.", ex);
            }
        }

        /// <summary>
        /// Adds a new user to the data store.
        /// </summary>
        /// <param name="user">The user to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if user is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if there is an error adding the user.</exception>
        public async Task AddAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "User entity cannot be null.");

            try
            {
                _logger.LogInformation("Adding new user to database.");
                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("User added successfully. ID: {UserId}", user.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding user to database.");
                throw new InvalidOperationException("Error adding user.", ex);
            }
        }

        /// <summary>
        /// Updates an existing user in the database.
        /// </summary>
        /// <param name="user">The updated user entity.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if user is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the user does not exist or an error occurs.</exception>
        public async Task UpdateAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "Cannot update a null user.");

            try
            {
                var existingUser = await _dbContext.Users.FindAsync(user.Id);
                if (existingUser == null)
                {
                    _logger.LogWarning("Attempted to update non-existent user with ID {UserId}.", user.Id);
                    throw new InvalidOperationException("User not found.");
                }

                existingUser.EmailConfirmed = user.EmailConfirmed;
                existingUser.EmailVerificationCode = user.EmailVerificationCode;
                existingUser.VerificationCodeExpiry = user.VerificationCodeExpiry;

                _logger.LogInformation("Updating user ID {UserId}.", user.Id);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("User ID {UserId} updated successfully.", user.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user ID {UserId}.", user?.Id);
                throw new InvalidOperationException("Error updating user.", ex);
            }
        }

        /// <summary>
        /// Checks whether a user with the specified ID is active and not deleted.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>True if active and not deleted; otherwise, false.</returns>
        public async Task<bool> IsAliveAsync(long userId)
        {
            return await _dbContext.Users.AnyAsync(u => u.Id == userId && u.Active && !u.Deleted);
        }

        /// <summary>
        /// Checks whether a user with the specified ID is deleted or inactive.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>True if deleted or inactive; otherwise, false.</returns>
        public async Task<bool> IsDeadAsync(long userId)
        {
            return await _dbContext.Users.AnyAsync(u => u.Id == userId && !u.Active && u.Deleted);
        }

        /// <summary>
        /// Checks if an email address already exists in the data store.
        /// </summary>
        /// <param name="email">The email to check.</param>
        /// <returns>True if the email exists; otherwise, false.</returns>
        public async Task<bool> EmailExists(string email)
        {
            return await _dbContext.Users.AnyAsync(u => u.Email == email);
        }

        /// <summary>
        /// Deletes a user from the data store.
        /// </summary>
        /// <param name="user">The user entity to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if user is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if error occurs during deletion.</exception>
        public async Task DeleteAsync(User user)
        {
            if (user == null)
            {
                _logger.LogError("Attempted to delete a null user from the database.");
                throw new ArgumentNullException(nameof(user), "User cannot be null.");
            }
            try
            {
                _logger.LogInformation("Attempting to delete a user from the database.");
                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"User with ID {user.Id} has been successfully deleted.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting user: {ex.Message}");
                throw new InvalidOperationException("An error occurred while deleting the user.", ex);
            }
        }
    }
}
