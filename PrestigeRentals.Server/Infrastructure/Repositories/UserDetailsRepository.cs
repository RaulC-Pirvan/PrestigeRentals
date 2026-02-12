using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PrestigeRentals.Application.Services.Interfaces.Repositories;
using PrestigeRentals.Domain.Entities;
using PrestigeRentals.Infrastructure.Persistence;

namespace PrestigeRentals.Application.Services
{
    /// <summary>
    /// Provides methods to interact with the UserDetails data in the database.
    /// Implements <see cref="IUserDetailsRepository"/>.
    /// </summary>
    public class UserDetailsRepository : IUserDetailsRepository
    {
        private readonly ApplicationDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDetailsRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context used for querying and saving data.</param>
        public UserDetailsRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Adds the provided user details to the database asynchronously.
        /// </summary>
        /// <param name="userDetails">The user details entity to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="Exception">Throws an exception if saving fails.</exception>
        public async Task AddAsync(UserDetails userDetails)
        {
            try
            {
                await _dbContext.UsersDetails.AddAsync(userDetails);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves the user details for a specific user by their user ID.
        /// </summary>
        /// <param name="userId">The ID of the user whose details to retrieve.</param>
        /// <returns>The user details if found; otherwise, null.</returns>
        public async Task<UserDetails> GetUserDetailsById(long userId)
        {
            return await _dbContext.UsersDetails.FirstOrDefaultAsync(ud => ud.UserID == userId);
        }

        /// <summary>
        /// Checks if the user details are active and not deleted.
        /// </summary>
        /// <param name="userId">The ID of the user to check.</param>
        /// <returns>True if user details are active and not deleted; otherwise, false.</returns>
        public async Task<bool> IsAliveAsync(long userId)
        {
            var userDetails = await _dbContext.UsersDetails
                .AsNoTracking()
                .FirstOrDefaultAsync(ud => ud.UserID == userId);

            return userDetails != null && userDetails.Active && !userDetails.Deleted;
        }

        /// <summary>
        /// Checks if the user details are inactive and marked as deleted.
        /// </summary>
        /// <param name="userId">The ID of the user to check.</param>
        /// <returns>True if user details are inactive and deleted; otherwise, false.</returns>
        public async Task<bool> IsDeadAsync(long userId)
        {
            var userDetails = await _dbContext.UsersDetails
                .AsNoTracking()
                .FirstOrDefaultAsync(ud => ud.UserID == userId);

            return userDetails != null && !userDetails.Active && userDetails.Deleted;
        }

        /// <summary>
        /// Updates existing user details in the database.
        /// </summary>
        /// <param name="userDetails">The user details entity with updated values.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task UpdateAsync(UserDetails userDetails)
        {
            _dbContext.UsersDetails.Update(userDetails);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes the specified user details from the database.
        /// </summary>
        /// <param name="userDetails">The user details entity to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeleteAsync(UserDetails userDetails)
        {
            _dbContext.UsersDetails.Remove(userDetails);
            await _dbContext.SaveChangesAsync();
        }
    }
}
