using System.Threading.Tasks;
using PrestigeRentals.Application.Requests;

namespace PrestigeRentals.Application.Services.Interfaces
{
    /// <summary>
    /// Defines the contract for user management services, including user roles, account status, password/email changes, and user details updates.
    /// </summary>
    public interface IUserManagementService
    {
        /// <summary>
        /// Promotes a user to an admin role.
        /// </summary>
        /// <param name="userId">The ID of the user to be promoted.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean result indicating success.</returns>
        Task<bool> MakeAdmin(long userId);

        /// <summary>
        /// Reverts a user from an admin role to a regular user.
        /// </summary>
        /// <param name="userId">The ID of the user to be reverted.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean result indicating success.</returns>
        Task<bool> RevertToUser(long userId);

        /// <summary>
        /// Changes the password for a user.
        /// </summary>
        /// <param name="userId">The ID of the user whose password will be changed.</param>
        /// <param name="oldPassword">The user's current password.</param>
        /// <param name="newPassword">The new password to be set for the user.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean result indicating success.</returns>
        Task<bool> ChangePassword(long userId, string oldPassword, string newPassword);

        /// <summary>
        /// Changes the email address for a user.
        /// </summary>
        /// <param name="userId">The ID of the user whose email will be changed.</param>
        /// <param name="newEmail">The new email address to be set for the user.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean result indicating success.</returns>
        Task<bool> ChangeEmail(long userId, string newEmail);

        /// <summary>
        /// Deactivates the account of a user.
        /// </summary>
        /// <param name="userId">The ID of the user whose account will be deactivated.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean result indicating success.</returns>
        Task<bool> DeactivateAccount(long userId);

        /// <summary>
        /// Activates the account of a deactivated user.
        /// </summary>
        /// <param name="userId">The ID of the user whose account will be activated.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean result indicating success.</returns>
        Task<bool> ActivateAccount(long userId);

        /// <summary>
        /// Deletes the account of a user.
        /// </summary>
        /// <param name="userId">The ID of the user whose account will be deleted.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean result indicating success.</returns>
        Task<bool> DeleteAccount(long userId);

        /// <summary>
        /// Updates the details of a user (e.g., first name, last name).
        /// </summary>
        /// <param name="userId">The ID of the user whose details will be updated.</param>
        /// <param name="updateUserDetailsRequest">The request containing the new details for the user.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean result indicating success.</returns>
        Task<bool> UpdateUserDetails(long userId, UpdateUserDetailsRequest updateUserDetailsRequest);
    }
}
