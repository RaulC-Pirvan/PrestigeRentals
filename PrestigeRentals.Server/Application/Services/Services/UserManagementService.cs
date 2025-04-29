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
        public UserManagementService(IUserRepository userRepository, IAuthService authService, IUserDetailsRepository userDetailsRepository, ILogger<UserManagementService> logger)
        {
            _userRepository = userRepository;
            _userDetailsRepository = userDetailsRepository;
            _logger = logger;
            _authService = authService;
        }

        /// <summary>
        /// Changes the user's email address.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="newEmail">The new email address.</param>
        /// <returns>A task that represents the asynchronous operation, with a result indicating success.</returns>
        public async Task<bool> ChangeEmail(long userId, string newEmail)
        {
            User user = await _userRepository.GetUserById(userId);
            if (user == null)
            {
                throw new UserNotFoundException();
            }
                
            if(await _userRepository.EmailExists(newEmail))
            {
                throw new EmailAlreadyExistsException();
            }

            user.Email = newEmail;
            await _userRepository.UpdateAsync(user);

            return true;
        }

        /// <summary>
        /// Changes the user's password.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="oldPassword">The current password of the user.</param>
        /// <param name="newPassword">The new password to set for the user.</param>
        /// <returns>A task that represents the asynchronous operation, with a result indicating success.</returns>
        public async Task<bool> ChangePassword(long userId, string oldPassword, string newPassword)
        {
            User user = await _userRepository.GetUserById(userId);
            if (user == null)
                throw new UserNotFoundException();

            if (!_authService.VerifyPassword(oldPassword, user.Password))
                throw new InvalidPasswordException();

            user.Password = _authService.HashPassword(newPassword);
            await _userRepository.UpdateAsync(user);

            return true;
        }

        /// <summary>
        /// Deactivates the user's account by setting the account and user details to inactive and deleted.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A task that represents the asynchronous operation, with a result indicating success.</returns>
        public async Task<bool> DeactivateAccount(long userId)
        {
            if (!await _userRepository.IsAliveAsync(userId) &&
                !await _userDetailsRepository.IsAliveAsync(userId))
            {
                _logger.LogWarning($"User with ID {userId} was not found or is already deactivated.");
                throw new UserAlreadyDeactivatedException();
            }

            var user = await _userRepository.GetUserById(userId);
            var userDetails = await _userDetailsRepository.GetUserDetailsById(userId);

            user.Active = false;
            user.Deleted = true;
            userDetails.Active = false;
            userDetails.Deleted = true;

            await _userRepository.UpdateAsync(user);
            await _userDetailsRepository.UpdateAsync(userDetails);
            _logger.LogInformation($"User with ID {userId} has been deactivated.");
            return true;
        }

        /// <summary>
        /// Deletes the user's account and details permanently from the database.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A task that represents the asynchronous operation, with a result indicating success.</returns>
        public async Task<bool> DeleteAccount(long userId)
        {
            var user = await _userRepository.GetUserById(userId);
            var userDetails = await _userDetailsRepository.GetUserDetailsById(userId);

            if (user == null && userDetails == null)
            {
                _logger.LogWarning($"User with ID {userId} not found or already deleted.");
                throw new UserNotFoundException();
            }

            if (userDetails != null)
                await _userDetailsRepository.DeleteAsync(userDetails);

            if (user != null)
                await _userRepository.DeleteAsync(user);

            _logger.LogInformation($"User with ID {userId} has been deleted.");
            return true;
        }

        /// <summary>
        /// Promotes the user to an admin role.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A task that represents the asynchronous operation, with a result indicating success.</returns>
        public async Task<bool> MakeAdmin(long userId)
        {
            await EnsureUserExists(userId);

            var user = await _userRepository.GetUserById(userId);

            if (user.Role == "Admin")
            {
                _logger.LogWarning($"User with ID {userId} is already an Admin.");
                throw new UserAlreadyAdminException();
            }

            user.Role = "Admin";
            await _userRepository.UpdateAsync(user);
            _logger.LogInformation($"User with ID {userId} promoted to Admin.");
            return true;
        }

        /// <summary>
        /// Reverts the user to a standard user role.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A task that represents the asynchronous operation, with a result indicating success.</returns>
        public async Task<bool> RevertToUser(long userId)
        {
            await EnsureUserExists(userId);

            var user = await _userRepository.GetUserById(userId);

            if (user.Role == "User")
            {
                _logger.LogWarning($"User with ID {userId} is already a User.");
                throw new UserAlreadyUserException();
            }

            user.Role = "User";
            await _userRepository.UpdateAsync(user);
            _logger.LogInformation($"User with ID {userId} demoted to User.");
            return true;
        }

        /// <summary>
        /// Activates the user's account by setting the account and user details to active and undeleted.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A task that represents the asynchronous operation, with a result indicating success.</returns>
        public async Task<bool> ActivateAccount(long userId)
        {
            if (!await _userRepository.IsDeadAsync(userId) ||
                !await _userDetailsRepository.IsDeadAsync(userId))
            {
                _logger.LogWarning($"User with ID {userId} is already active.");
                throw new UserAlreadyActivatedException();
            }

            var user = await _userRepository.GetUserById(userId);
            var userDetails = await _userDetailsRepository.GetUserDetailsById(userId);

            user.Active = true;
            user.Deleted = false;
            userDetails.Active = true;
            userDetails.Deleted = false;

            await _userRepository.UpdateAsync(user);
            await _userDetailsRepository.UpdateAsync(userDetails);
            _logger.LogInformation($"User with ID {userId} has been activated.");
            return true;
        }

        /// <summary>
        /// Updates the user's personal details, such as first and last name.
        /// </summary>  
        /// <param name="userId">The ID of the user.</param>
        /// <param name="updateUserDetailsRequest">The request containing the updated user details.</param>
        /// <returns>A task that represents the asynchronous operation, with a result indicating success.</returns>
        public async Task<bool> UpdateUserDetails(long userId, UpdateUserDetailsRequest request)
        {
            await EnsureUserExists(userId);

            var userDetails = await _userDetailsRepository.GetUserDetailsById(userId);

            userDetails.FirstName = string.IsNullOrWhiteSpace(request.FirstName) ? userDetails.FirstName : request.FirstName;
            userDetails.LastName = string.IsNullOrWhiteSpace(request.LastName) ? userDetails.LastName : request.LastName;

            await _userDetailsRepository.UpdateAsync(userDetails);
            _logger.LogInformation($"User with ID {userId} has been updated.");
            return true;
        }
        
        private async Task EnsureUserExists(long userId)
        {
            bool userAlive = await _userRepository.IsAliveAsync(userId);
            bool detailsAlive = await _userDetailsRepository.IsAliveAsync(userId);

            if (!userAlive || !detailsAlive)
            {
                _logger.LogWarning($"User with ID {userId} was not found.");
                throw new UserNotFoundException();
            }
        }
    }
}
