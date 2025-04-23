using System;
using System.Threading.Tasks;
using PrestigeRentals.Domain.Entities;

namespace PrestigeRentals.Application.Services.Interfaces
{
    /// <summary>
    /// Defines the contract for accessing and managing user details in the repository.
    /// </summary>
    public interface IUserDetailsRepository
    {
        /// <summary>
        /// Asynchronously adds new user details to the repository.
        /// </summary>
        /// <param name="userDetails">The user details to be added.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddAsync(UserDetails userDetails);

        /// <summary>
        /// Asynchronously retrieves user details by user ID.
        /// </summary>
        /// <param name="userid">The ID of the user whose details are to be retrieved.</param>
        /// <returns>A task representing the asynchronous operation, with a result of the user details.</returns>
        Task<UserDetails> GetUserDetailsById(int userid);
    }
}
