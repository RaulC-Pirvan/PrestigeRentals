using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PrestigeRentals.Application.Requests;
using PrestigeRentals.Application.Services.Interfaces;
using PrestigeRentals.Domain.Entities;
using PrestigeRentals.Infrastructure.Persistence;

namespace PrestigeRentals.Application.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserDetailsRepository _userDetailsRepository;
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<UserManagementService> _logger;
        private readonly IAuthService _authService;

        public UserManagementService(IUserRepository userRepository, IAuthService authService, IUserDetailsRepository userDetailsRepository, ApplicationDbContext dbContext, ILogger<UserManagementService> logger)
        {
            _userRepository = userRepository;
            _userDetailsRepository = userDetailsRepository;
            _dbContext = dbContext;
            _logger = logger;
            _authService = authService;
        }
        private async Task<bool> IsUserAlive(int userId)
        {
            bool isUserAlive = await _dbContext.Users.AnyAsync(u => u.Id == userId && u.Active && !u.Deleted);

            return isUserAlive;
        }

        private async Task<bool> IsUserDetailsAlive(int userId)
        {
            bool isUserDetailsAlive = await _dbContext.UsersDetails.AnyAsync(ud => ud.Id == userId && ud.Active && !ud.Deleted);

            return isUserDetailsAlive;
        }

        private async Task<bool> IsUserDead(int userId)
        {
            bool isUserDead = await _dbContext.Users.AnyAsync(u => u.Id == userId && !u.Active && u.Deleted);

            return isUserDead;
        }

        private async Task<bool> IsUserDetailsDead(int userId)
        {
            bool isUserDetailsDead = await _dbContext.UsersDetails.AnyAsync(ud => ud.Id == userId && !ud.Active && ud.Deleted);

            return isUserDetailsDead;
        }

        public async Task<bool> ChangeEmail(int userId, string newEmail)
        {
            var user = await _dbContext.Users.FindAsync(userId);

            if (user == null)
                return false;

            user.Email = newEmail;
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ChangePassword(int userId, string newPassword)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
                return false;

            user.Password = _authService.HashPassword(newPassword);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeactivateAccount(int userId)
        {
            bool isUserAlive = await IsUserAlive(userId);
            bool isUserDetailsAlive = await IsUserDetailsAlive(userId);

            if(isUserAlive && isUserDetailsAlive)
            {
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

            _logger.LogWarning($"User with ID {userId} was not found or is already dead.");
            return false;
        }

        public async Task<bool> DeleteAccount(int userId)
        {
            User user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            UserDetails userDetails = await _dbContext.UsersDetails.FirstOrDefaultAsync(ud => ud.UserID == userId);

            if (user != null && userDetails != null)
            {
                _dbContext.UsersDetails.Remove(userDetails);
                await _dbContext.SaveChangesAsync();

                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"User with ID {userId} has been deleted.");
                return true;
            }

            _logger.LogWarning($"User with ID {userId} not found or has been already deleted.");
            return false;
        }

        public async Task<bool> MakeAdmin(int userId)
        {
            bool isUserAlive = await IsUserAlive(userId);
            bool isUserDetailsAlive = await IsUserDetailsAlive(userId);

            if(isUserAlive && isUserDetailsAlive)
            {
                User user = await _userRepository.GetUserById(userId);
                if (user.Role != "Admin")
                {
                    user.Role = "Admin";
                    await _dbContext.SaveChangesAsync();
                    _logger.LogInformation($"User with ID {userId} successfully promoted to Admin.");

                    return true;
                }

                _logger.LogWarning($"User with ID {userId} is already an Admin.");
                return false;
            }

            _logger.LogWarning($"User with ID {userId} could not be found.");
            return false;
        }

        public async Task<bool> RevertToUser(int userId)
        {
            bool isUserAlive = await IsUserAlive(userId);
            bool isUserDetailsAlive = await IsUserDetailsAlive(userId);

            if (isUserAlive && isUserDetailsAlive)
            {
                User user = await _userRepository.GetUserById(userId);
                if (user.Role != "User")
                {
                    user.Role = "User";
                    await _dbContext.SaveChangesAsync();
                    _logger.LogInformation($"User with ID {userId} successfully demoted to User.");

                    return true;
                }

                _logger.LogWarning($"User with ID {userId} is already an User.");
                return false;
            }

            _logger.LogWarning($"User with ID {userId} could not be found.");
            return false;
        }

        public async Task<bool> ActivateAccount(int userId)
        {
            bool isUserDead = await IsUserDead(userId);
            bool isUserDetailsDead = await IsUserDetailsDead(userId);

            if (isUserDead && isUserDetailsDead)
            {
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

            _logger.LogWarning($"User with ID {userId} was not found or is already alive.");
            return false;
        }

        public async Task<bool> UpdateUserDetails(int userId, UpdateUserDetailsRequest updateUserDetailsRequest)
        {
            bool isUserAlive = await IsUserAlive(userId);
            bool isUserDetailsAlive = await IsUserDetailsAlive(userId);

            if(isUserAlive && isUserDetailsAlive)
            {
                UserDetails userDetails = await _userDetailsRepository.GetUserDetailsById(userId);
                userDetails.FirstName = string.IsNullOrWhiteSpace(updateUserDetailsRequest.FirstName) ? userDetails.FirstName : updateUserDetailsRequest.FirstName;
                userDetails.LastName = string.IsNullOrWhiteSpace(updateUserDetailsRequest.LastName) ? userDetails.LastName : updateUserDetailsRequest.LastName;

                await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"User with ID {userId} has been updated.");
                return true;
            }

            _logger.LogWarning($"User with ID {userId} was not found.");
            return false;
        }
    }
}
