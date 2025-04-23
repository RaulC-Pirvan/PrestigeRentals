using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PrestigeRentals.Application.Services.Interfaces;
using PrestigeRentals.Domain.Entities;
using PrestigeRentals.Infrastructure.Persistence;

namespace PrestigeRentals.Application.Services
{
    /// <summary>
    /// Repository class responsible for performing CRUD operations on User entities in the database.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IUserDetailsRepository _userDetailsRepository;
        private readonly ILogger<UserRepository> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The application database context.</param>
        /// <param name="userDetailsRepository">The repository for accessing user details.</param>
        /// <param name="logger">Logger instance to log operation details.</param>
        public UserRepository(ApplicationDbContext dbContext, IUserDetailsRepository userDetailsRepository, ILogger<UserRepository> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext), "Database context cannot be null.");
            _userDetailsRepository = userDetailsRepository ?? throw new ArgumentNullException(nameof(userDetailsRepository), "User details repository cannot be null.");
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null.");
        }

        /// <summary>
        /// Fetches a user by their email address.
        /// </summary>
        /// <param name="email">The email address of the user to fetch.</param>
        /// <returns>The <see cref="User"/> with the given email, or null if not found.</returns>
        /// <exception cref="InvalidOperationException">Thrown if an error occurs while querying the database.</exception>
        public async Task<User> GetUserByEmail(string email)
        {
            try
            {
                _logger.LogInformation("Attempting to fetch user by email: {Email}", email);
                var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

                if (user == null)
                {
                    _logger.LogWarning("User with email {Email} was not found.", email);
                }

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while retrieving the user by email {Email}: {Error}", email, ex.Message);
                throw new InvalidOperationException("An error occurred while fetching the user by email.", ex);
            }
        }

        /// <summary>
        /// Fetches a user by their unique identifier (ID).
        /// </summary>
        /// <param name="userId">The ID of the user to fetch.</param>
        /// <returns>The <see cref="User"/> with the given ID, or null if not found.</returns>
        /// <exception cref="InvalidOperationException">Thrown if an error occurs while querying the database.</exception>
        public async Task<User> GetUserById(int userId)
        {
            try
            {
                _logger.LogInformation("Attempting to fetch user by ID: {UserId}", userId);
                var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    _logger.LogWarning("User with ID {UserId} was not found.", userId);
                }

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while retrieving the user by ID {UserId}: {Error}", userId, ex.Message);
                throw new InvalidOperationException("An error occurred while fetching the user by ID.", ex);
            }
        }

        /// <summary>
        /// Adds a new user to the database.
        /// </summary>
        /// <param name="user">The user entity to add to the database.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="user"/> is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if an error occurs while adding the user to the database.</exception>
        public async Task AddAsync(User user)
        {
            if (user == null)
            {
                _logger.LogError("Attempted to add a null user to the database.");
                throw new ArgumentNullException(nameof(user), "User cannot be null.");
            }

            try
            {
                _logger.LogInformation("Attempting to add a new user to the database.");
                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("User with ID {UserId} has been successfully added.", user.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while adding the user: {Error}", ex.Message);
                throw new InvalidOperationException("An error occurred while adding the user.", ex);
            }
        }
    }
}
