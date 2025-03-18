using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PrestigeRentals.Application.Requests;
using PrestigeRentals.Application.Services.Interfaces;
using PrestigeRentals.Domain.Entities;
using PrestigeRentals.Infrastructure.Persistence;

namespace PrestigeRentals.Application.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IUserDetailsRepository _userDetailsRepository;

        public UserRepository(ApplicationDbContext dbContext, IUserDetailsRepository userDetailsRepository)
        {
            _dbContext = dbContext;
            _userDetailsRepository = userDetailsRepository;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetUserById(int userId)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task AddAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsnyc(UpdateUserRequest updateUserRequest)
        {
            var existingUser = await _dbContext.Users.FindAsync(updateUserRequest.Id);
            if (existingUser == null)
            {
                throw new Exception("User not found.");
            }

            existingUser.Email = updateUserRequest.Email;
            existingUser.Password = updateUserRequest.Password;
            existingUser.Role = updateUserRequest.Role;
            
            _dbContext.Users.Update(existingUser);
            await _dbContext.SaveChangesAsync();

            var existingUserDetails = await _dbContext.UsersDetails.FindAsync(updateUserRequest.Id);

            if(existingUserDetails == null)
            {
                throw new Exception("User details not found.");
            }

            existingUserDetails.FirstName = updateUserRequest.FirstName;
            existingUserDetails.LastName = updateUserRequest.LastName;
            existingUserDetails.DateOfBirth = updateUserRequest.DateOfBirth;
            existingUserDetails.ImageData = updateUserRequest.ImageData;

            _dbContext.UsersDetails.Update(existingUserDetails);
            await _dbContext.SaveChangesAsync();
        }
    }
}
