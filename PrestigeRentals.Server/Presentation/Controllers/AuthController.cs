using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using PrestigeRentals.Application.Requests;
using PrestigeRentals.Application.Services.Interfaces;
using PrestigeRentals.Infrastructure.Persistence;
using LoginRequest = PrestigeRentals.Application.Requests.LoginRequest;
using RegisterRequest = PrestigeRentals.Application.Requests.RegisterRequest;

namespace PrestigeRentals.Presentation.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserManagementService _userManagementService;
        private readonly ApplicationDbContext _dbContext;

        public AuthController(IAuthService authService, IUserManagementService userManagementService, ApplicationDbContext dbContext)
        {
            _authService = authService;
            _userManagementService = userManagementService;
            _dbContext = dbContext;
        }

        [HttpPost("/register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            try
            {
                var token = await _authService.RegisterAsync(model);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpPost("/login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            try
            {
                var token = await _authService.AuthenticateAsync(model);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpPatch("{userId}/change-password")]
        public async Task<IActionResult> ChangePassword(int userId, string oldPassword, string newPassword)
        {
            try
            {
                bool isPasswordChanged = await _userManagementService.ChangePassword(userId, newPassword);
                if (isPasswordChanged)
                    return Ok("Password changed successfully.");
                return BadRequest("Error: Password could not be changed.");
            }

            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPatch("{userId}/change-email")]
        public async Task<IActionResult> ChangeEmail(int userId, string newEmail)
        {
            try
            {
                bool isEmailChanged = await _userManagementService.ChangeEmail(userId, newEmail);
                if (isEmailChanged)
                    return Ok("Email changed successfully.");
                return BadRequest("Error: Email could not be changed.");
            }

            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPatch("{userId}/set-inactive")]
        public async Task<IActionResult> DeactivateAccount(int userId)
        {
            try
            {
                bool isUserDeactivated = await _userManagementService.DeactivateAccount(userId);

                if (isUserDeactivated == true)
                    return Ok("User deactivated successfully.");
                return BadRequest("Error: User could not be deactivated.");
            }

            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPatch("{userId}/set-active")]
        public async Task<IActionResult> ActivateAccount(int userId)
        {
            try
            {
                bool isUserActivated = await _userManagementService.ActivateAccount(userId);

                if (isUserActivated == true)
                    return Ok("User activated successfully.");
                return BadRequest("Error: User could not be activated.");
            }

            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteAccount(int userId)
        {
            try
            {
                bool isUserDeleted = await _userManagementService.DeleteAccount(userId);

                if (isUserDeleted)
                    return Ok("User deleted successfully.");
                return BadRequest("Error: User could not be deleted.");
            }

            catch(Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPatch("{userId}/set-admin")]
        public async Task<IActionResult> SetAdmin(int userId)
        {
            try
            {
                bool isUserPromoted = await _userManagementService.MakeAdmin(userId);

                if (isUserPromoted)
                    return Ok("User successfully promoted to Admin.");
                return BadRequest("Error: User could not be promoted.");
            }

            catch(Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPatch("{userId}/set-user")]
        public async Task<IActionResult> SetUser(int userId)
        {
            try
            {
                bool isUserDemoted = await _userManagementService.RevertToUser(userId);

                if (isUserDemoted)
                    return Ok("User successfully demoted to User.");
                return BadRequest("Error: User could not be demoted.");
            }

            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPatch("{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, UpdateUserDetailsRequest updateUserDetailsRequest)
        {
            try
            {
                bool isUserUpdated = await _userManagementService.UpdateUserDetails(userId, updateUserDetailsRequest);

                if (isUserUpdated)
                    return Ok("User successfully updated.");
                return BadRequest("Error: User could not be updated.");
            }

            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
