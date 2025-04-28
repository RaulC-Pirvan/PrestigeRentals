using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PrestigeRentals.Application.Services.Interfaces.Repositories;
using PrestigeRentals.Domain.Entities;
using PrestigeRentals.Infrastructure.Persistence;

namespace PrestigeRentals.Application.Services
{
    /// <summary>
    /// Provides methods to interact with the UserDetails data in the database.
    /// </summary>
    public class UserDetailsRepository : IUserDetailsRepository
    {
        private readonly ApplicationDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDetailsRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context used for querying and saving data to the database.</param>
        public UserDetailsRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Adds the provided user details to the database.
        /// </summary>
        /// <param name="userDetails">The user details to add.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task AddAsync(UserDetails userDetails)
        {
            await _dbContext.UsersDetails.AddAsync(userDetails);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieves the user details for a specific user by their user ID.
        /// </summary>
        /// <param name="userId">The ID of the user whose details are to be retrieved.</param>
        /// <returns>A task that represents the asynchronous operation, with a result of the user details if found, or null if not.</returns>
        public async Task<UserDetails> GetUserDetailsById(int userId)
        {
            return await _dbContext.UsersDetails.FirstOrDefaultAsync(ud => ud.Id == userId);
        }
    }
}
