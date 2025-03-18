using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PrestigeRentals.Application.Requests;

namespace PrestigeRentals.Application.Services.Interfaces
{
    public interface IUserManagementService
    {
        Task<bool> MakeAdmin(int userId);
        Task<bool> RevertToUser(int userId);
        Task<bool> ChangePassword(int userId, string newPassword);
        Task<IActionResult> ChangeEmail(int userId, string newEmail);
        Task<bool> DeactivateAccount(int userId);
        Task<bool> ActivateAccount(int userId);
        Task<bool> DeleteAccount(int userId);
        Task<IActionResult> UpdateUserDetails(UpdateUserRequest updateUserRequest);
     
    }
}
