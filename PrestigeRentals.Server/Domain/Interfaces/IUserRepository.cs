using System.Threading.Tasks;
using PrestigeRentals.Domain.Entities;

namespace PrestigeRentals.Application.Services.Interfaces.Repositories
{
    /// <summary>
    /// Defines the contract for user data operations, including retrieval and persistence.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Retrieves a user by their email address.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <returns>The matching <see cref="User"/> if found; otherwise, null.</returns>
        Task<User> GetUserByEmail(string email);

        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="id">The user's ID.</param>
        /// <returns>The matching <see cref="User"/> if found; otherwise, null.</returns>
        Task<User> GetUserById(long id);

        /// <summary>
        /// Adds a new user to the data store.
        /// </summary>
        /// <param name="user">The user to add.</param>
        Task AddAsync(User user);

        /// <summary>
        /// Updates an existing user's data.
        /// </summary>
        /// <param name="user">The user object with updated information.</param>
        Task UpdateAsync(User user);

        /// <summary>
        /// Determines whether a user with the specified ID is active (not deleted).
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>True if the user is active; otherwise, false.</returns>
        Task<bool> IsAliveAsync(long userId);

        /// <summary>
        /// Determines whether a user with the specified ID is marked as deleted.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>True if the user is deleted; otherwise, false.</returns>
        Task<bool> IsDeadAsync(long userId);

        /// <summary>
        /// Checks if an email address already exists in the data store.
        /// </summary>
        /// <param name="email">The email address to check.</param>
        /// <returns>True if the email exists; otherwise, false.</returns>
        Task<bool> EmailExists(string email);

        /// <summary>
        /// Deletes a user from the data store.
        /// </summary>
        /// <param name="user">The user to delete.</param>
        Task DeleteAsync(User user);
    }
}
