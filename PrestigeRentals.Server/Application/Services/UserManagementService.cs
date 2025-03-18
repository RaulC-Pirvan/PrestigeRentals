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

        public UserManagementService(IUserRepository userRepository, IUserDetailsRepository userDetailsRepository, ApplicationDbContext dbContext, ILogger<UserManagementService> logger)
        {
            _userRepository = userRepository;
            _userDetailsRepository = userDetailsRepository;
            _dbContext = dbContext;
            _logger = logger;
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

        public Task<IActionResult> ChangeEmail(int userId, string newEmail)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> ChangePassword(int userId, string newPassword)
        {
            throw new NotImplementedException();
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

        public Task<bool> DeleteAccount(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> MakeAdmin(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ReactivateAccount(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> RevertToUser(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> UpdateUserDetails(UpdateUserRequest updateUserRequest)
        {
            throw new NotImplementedException();
        }
    }
}
