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
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IUserDetailsRepository _userDetailsRepository;
        private readonly ILogger<UserRepository> _logger;

        /// <summary>
        /// Constructs a new instance of the <see cref="UserRepository"/>.
        /// </summary>
        public UserRepository(
            ApplicationDbContext dbContext,
            IUserDetailsRepository userDetailsRepository,
            ILogger<UserRepository> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _userDetailsRepository = userDetailsRepository ?? throw new ArgumentNullException(nameof(userDetailsRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

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

        public async Task<User> GetUserById(int userId)
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
        /// Updates the user record in the database.
        /// </summary>
        /// <param name="user">The updated user entity with changed values.</param>
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
    }
}
