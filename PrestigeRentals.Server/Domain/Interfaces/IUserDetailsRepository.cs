using System.Threading.Tasks;
using PrestigeRentals.Domain.Entities;

namespace PrestigeRentals.Application.Services.Interfaces.Repositories
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
        Task<UserDetails> GetUserDetailsById(long userid);

        /// <summary>
        /// Determines whether the user details with the specified ID are marked as active.
        /// </summary>
        /// <param name="id">The ID of the user details.</param>
        /// <returns>A task returning true if the user details are active; otherwise, false.</returns>
        Task<bool> IsAliveAsync(long id);

        /// <summary>
        /// Determines whether the user details with the specified ID are marked as deleted or inactive.
        /// </summary>
        /// <param name="id">The ID of the user details.</param>
        /// <returns>A task returning true if the user details are deleted/inactive; otherwise, false.</returns>
        Task<bool> IsDeadAsync(long id);

        /// <summary>
        /// Asynchronously updates the specified user details in the repository.
        /// </summary>
        /// <param name="details">The user details to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateAsync(UserDetails details);

        /// <summary>
        /// Asynchronously deletes the specified user details from the repository.
        /// </summary>
        /// <param name="details">The user details to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteAsync(UserDetails details);
    }
}
