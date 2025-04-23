using System.Threading.Tasks;
using PrestigeRentals.Application.Requests;
using PrestigeRentals.Domain.Entities;

namespace PrestigeRentals.Application.Services.Interfaces
{
    /// <summary>
    /// Defines the contract for interacting with the user repository, including retrieving and adding user data.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Asynchronously retrieves a user by their email address.
        /// </summary>
        /// <param name="email">The email address of the user to be retrieved.</param>
        /// <returns>A task representing the asynchronous operation, with the user data if found, otherwise null.</returns>
        Task<User> GetUserByEmail(string email);

        /// <summary>
        /// Asynchronously retrieves a user by their unique user ID.
        /// </summary>
        /// <param name="id">The ID of the user to be retrieved.</param>
        /// <returns>A task representing the asynchronous operation, with the user data if found, otherwise null.</returns>
        Task<User> GetUserById(int id);

        /// <summary>
        /// Asynchronously adds a new user to the repository.
        /// </summary>
        /// <param name="user">The user object containing the user's information to be added.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddAsync(User user);
    }
}
