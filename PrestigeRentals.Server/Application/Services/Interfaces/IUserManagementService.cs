using System.Collections.Generic;
using System.Threading.Tasks;
using PrestigeRentals.Application.DTO;
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
        /// <returns>True if the operation was successful; otherwise, false.</returns>
        Task<bool> MakeAdmin(long userId);

        /// <summary>
        /// Reverts a user from an admin role to a regular user.
        /// </summary>
        /// <param name="userId">The ID of the user to be reverted.</param>
        /// <returns>True if the operation was successful; otherwise, false.</returns>
        Task<bool> RevertToUser(long userId);

        /// <summary>
        /// Changes the password for a user.
        /// </summary>
        /// <param name="userId">The ID of the user whose password will be changed.</param>
        /// <param name="oldPassword">The user's current password.</param>
        /// <param name="newPassword">The new password to be set.</param>
        /// <returns>True if the password change was successful; otherwise, false.</returns>
        Task<bool> ChangePassword(long userId, string oldPassword, string newPassword);

        /// <summary>
        /// Changes the email address for a user.
        /// </summary>
        /// <param name="userId">The ID of the user whose email will be updated.</param>
        /// <param name="newEmail">The new email address to set.</param>
        /// <returns>True if the email change was successful; otherwise, false.</returns>
        Task<bool> ChangeEmail(long userId, string newEmail);

        /// <summary>
        /// Deactivates the account of a user (soft ban).
        /// </summary>
        /// <param name="userId">The ID of the user to deactivate.</param>
        /// <returns>True if the account was successfully deactivated; otherwise, false.</returns>
        Task<bool> DeactivateAccount(long userId);

        /// <summary>
        /// Reactivates the account of a previously deactivated user.
        /// </summary>
        /// <param name="userId">The ID of the user to reactivate.</param>
        /// <returns>True if the account was successfully reactivated; otherwise, false.</returns>
        Task<bool> ActivateAccount(long userId);

        /// <summary>
        /// Permanently deletes the account of a user.
        /// </summary>
        /// <param name="userId">The ID of the user to delete.</param>
        /// <returns>True if the account was successfully deleted; otherwise, false.</returns>
        Task<bool> DeleteAccount(long userId);

        /// <summary>
        /// Updates profile information for a user, such as name and other editable details.
        /// </summary>
        /// <param name="userId">The ID of the user being updated.</param>
        /// <param name="updateUserDetailsRequest">The updated profile information.</param>
        /// <returns>True if the update was successful; otherwise, false.</returns>
        Task<bool> UpdateUserDetails(long userId, UpdateUserDetailsRequest updateUserDetailsRequest);

        /// <summary>
        /// Retrieves the profile information of a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user whose profile is requested.</param>
        /// <returns>A <see cref="UserProfileDTO"/> containing the user's profile data.</returns>
        Task<UserProfileDTO> GetUserProfile(long userId);

        /// <summary>
        /// Retrieves a list of all users registered in the system.
        /// </summary>
        /// <returns>A list of users represented as DTOs.</returns>
        Task<List<UserDTO>> GetAllUsersAsync();
    }
}
