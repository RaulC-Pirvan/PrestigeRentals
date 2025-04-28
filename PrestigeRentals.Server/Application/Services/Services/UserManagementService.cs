using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PrestigeRentals.Application.Exceptions;
using PrestigeRentals.Application.Requests;
using PrestigeRentals.Application.Services.Interfaces;
using PrestigeRentals.Application.Services.Interfaces.Repositories;
using PrestigeRentals.Domain.Entities;
using PrestigeRentals.Infrastructure.Persistence;

namespace PrestigeRentals.Application.Services.Services
{
    /// <summary>
    /// Provides services for managing user accounts, including actions like changing email, password, 
    /// deactivating/activating accounts, promoting users to admin, and updating user details.
    /// </summary>
    public class UserManagementService : IUserManagementService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserDetailsRepository _userDetailsRepository;
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<UserManagementService> _logger;
        private readonly IAuthService _authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserManagementService"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository for interacting with the users.</param>
        /// <param name="authService">The authentication service for password hashing and verification.</param>
        /// <param name="userDetailsRepository">The repository for interacting with user details.</param>
        /// <param name="dbContext">The database context used for querying and saving data.</param>
        /// <param name="logger">The logger for logging operations.</param>
        public UserManagementService(IUserRepository userRepository, IAuthService authService, IUserDetailsRepository userDetailsRepository, ApplicationDbContext dbContext, ILogger<UserManagementService> logger)
        {
            _userRepository = userRepository;
            _userDetailsRepository = userDetailsRepository;
            _dbContext = dbContext;
            _logger = logger;
            _authService = authService;
        }

        /// <summary>
        /// Checks whether the user is active and not deleted.
        /// </summary>
        private async Task<bool> IsUserAlive(int userId)
        {
            bool isUserAlive = await _dbContext.Users.AnyAsync(u => u.Id == userId && u.Active && !u.Deleted);
            return isUserAlive;
        }

        /// <summary>
        /// Checks whether the user details are active and not deleted.
        /// </summary>
        private async Task<bool> IsUserDetailsAlive(int userId)
        {
            bool isUserDetailsAlive = await _dbContext.UsersDetails.AnyAsync(ud => ud.Id == userId && ud.Active && !ud.Deleted);
            return isUserDetailsAlive;
        }

        /// <summary>
        /// Checks whether the user is deactivated and deleted.
        /// </summary>
        private async Task<bool> IsUserDead(int userId)
        {
            bool isUserDead = await _dbContext.Users.AnyAsync(u => u.Id == userId && !u.Active && u.Deleted);
            return isUserDead;
        }

        /// <summary>
        /// Checks whether the user details are deactivated and deleted.
        /// </summary>
        private async Task<bool> IsUserDetailsDead(int userId)
        {
            bool isUserDetailsDead = await _dbContext.UsersDetails.AnyAsync(ud => ud.Id == userId && !ud.Active && ud.Deleted);
            return isUserDetailsDead;
        }

        /// <summary>
        /// Changes the user's email address.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="newEmail">The new email address.</param>
        /// <returns>A task that represents the asynchronous operation, with a result indicating success.</returns>
        public async Task<bool> ChangeEmail(int userId, string newEmail)
        {
            User user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
                throw new UserNotFoundException();

            user.Email = newEmail;
            await _dbContext.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Changes the user's password.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="oldPassword">The current password of the user.</param>
        /// <param name="newPassword">The new password to set for the user.</param>
        /// <returns>A task that represents the asynchronous operation, with a result indicating success.</returns>
        public async Task<bool> ChangePassword(int userId, string oldPassword, string newPassword)
        {
            User user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
                throw new UserNotFoundException();

            if (!_authService.VerifyPassword(oldPassword, user.Password))
                throw new InvalidPasswordException();

            user.Password = _authService.HashPassword(newPassword);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Deactivates the user's account by setting the account and user details to inactive and deleted.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A task that represents the asynchronous operation, with a result indicating success.</returns>
        public async Task<bool> DeactivateAccount(int userId)
        {
            bool isUserAlive = await IsUserAlive(userId);
            bool isUserDetailsAlive = await IsUserDetailsAlive(userId);

            if (!isUserAlive && !isUserDetailsAlive)
            {
                _logger.LogWarning($"User with ID {userId} was not found or is already dead.");
                throw new UserAlreadyDeactivatedException();
            }

            User user = await _userRepository.GetUserById(userId);
            user.Active = false;
            user.Deleted = true;

            UserDetails userDetails = await _userDetailsRepository.GetUserDetailsById(userId);
            userDetails.Active = false;
            userDetails.Deleted = true;

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"User with ID {userId} has been deactivated.");
            return true;
        }

        /// <summary>
        /// Deletes the user's account and details permanently from the database.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A task that represents the asynchronous operation, with a result indicating success.</returns>
        public async Task<bool> DeleteAccount(int userId)
        {
            User user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            UserDetails userDetails = await _dbContext.UsersDetails.FirstOrDefaultAsync(ud => ud.UserID == userId);

            if (user == null && userDetails == null)
            {
                _logger.LogWarning($"User with ID {userId} not found or has been already deleted.");
                throw new UserNotFoundException();
            }

            _dbContext.UsersDetails.Remove(userDetails);
            await _dbContext.SaveChangesAsync();

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"User with ID {userId} has been deleted.");
            return true;
        }

        /// <summary>
        /// Promotes the user to an admin role.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A task that represents the asynchronous operation, with a result indicating success.</returns>
        public async Task<bool> MakeAdmin(int userId)
        {
            bool isUserAlive = await IsUserAlive(userId);
            bool isUserDetailsAlive = await IsUserDetailsAlive(userId);

            if (!isUserAlive || !isUserDetailsAlive)
            {
                _logger.LogWarning($"User with ID {userId} could not be found.");
                throw new UserNotFoundException();
            }

            User user = await _userRepository.GetUserById(userId);

            if (user.Role == "Admin")
            {
                _logger.LogWarning($"User with ID {userId} is already an Admin.");
                throw new UserAlreadyAdminException();
            }

            user.Role = "Admin";
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"User with ID {userId} successfully promoted to Admin.");
            return true;
        }

        /// <summary>
        /// Reverts the user to a standard user role.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A task that represents the asynchronous operation, with a result indicating success.</returns>
        public async Task<bool> RevertToUser(int userId)
        {
            bool isUserAlive = await IsUserAlive(userId);
            bool isUserDetailsAlive = await IsUserDetailsAlive(userId);

            if (!isUserAlive || !isUserDetailsAlive)
            {
                _logger.LogWarning($"User with ID {userId} could not be found.");
                throw new UserNotFoundException();
            }

            User user = await _userRepository.GetUserById(userId);

            if (user.Role == "User")
            {
                _logger.LogWarning($"User with ID {userId} is already a User.");
                throw new UserAlreadyUserException();
            }

            user.Role = "User";
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"User with ID {userId} successfully demoted to User.");

            return true;
        }

        /// <summary>
        /// Activates the user's account by setting the account and user details to active and undeleted.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A task that represents the asynchronous operation, with a result indicating success.</returns>
        public async Task<bool> ActivateAccount(int userId)
        {
            bool isUserDead = await IsUserDead(userId);
            bool isUserDetailsDead = await IsUserDetailsDead(userId);

            if (!isUserDead || !isUserDetailsDead)
            {
                _logger.LogWarning($"User with ID {userId} was not found or is already alive.");
                throw new UserAlreadyActivatedException();
            }

            User user = await _userRepository.GetUserById(userId);
            user.Active = true;
            user.Deleted = false;

            UserDetails userDetails = await _userDetailsRepository.GetUserDetailsById(userId);
            userDetails.Active = true;
            userDetails.Deleted = false;

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"User with ID {userId} has been activated.");
            return true;
        }

        /// <summary>
        /// Updates the user's personal details, such as first and last name.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="updateUserDetailsRequest">The request containing the updated user details.</param>
        /// <returns>A task that represents the asynchronous operation, with a result indicating success.</returns>
        public async Task<bool> UpdateUserDetails(int userId, UpdateUserDetailsRequest updateUserDetailsRequest)
        {
            bool isUserAlive = await IsUserAlive(userId);
            bool isUserDetailsAlive = await IsUserDetailsAlive(userId);

            if (!isUserAlive || !isUserDetailsAlive)
            {
                _logger.LogWarning($"User with ID {userId} was not found.");
                throw new UserNotFoundException();
            }

            UserDetails userDetails = await _userDetailsRepository.GetUserDetailsById(userId);
            userDetails.FirstName = string.IsNullOrWhiteSpace(updateUserDetailsRequest.FirstName) ? userDetails.FirstName : updateUserDetailsRequest.FirstName;
            userDetails.LastName = string.IsNullOrWhiteSpace(updateUserDetailsRequest.LastName) ? userDetails.LastName : updateUserDetailsRequest.LastName;

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"User with ID {userId} has been updated.");
            return true;
        }
    }
}
