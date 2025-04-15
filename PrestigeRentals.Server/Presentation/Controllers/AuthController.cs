using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using PrestigeRentals.Application.Exceptions;
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

            catch (EmailAlreadyExistsException ex)
            {
                return Conflict(ex.Message);
            }

            catch (InvalidPhotoFormatException ex)
            {
                return BadRequest(ex.Message);
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

            catch (InvalidPasswordException ex)
            {
                return Unauthorized(ex.Message);
            }

            catch(UserNotFoundException ex)
            {
                return NotFound(ex.Message);
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
                bool isPasswordChanged = await _userManagementService.ChangePassword(userId, oldPassword, newPassword);
                return Ok("Password changed successfully.");
            }

            catch(InvalidPasswordException ex)
            {
                return Unauthorized(ex.Message);
            }

            catch(UserNotFoundException ex)
            {
                return NotFound(ex.Message);
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

            catch(EmailAlreadyExistsException ex)
            {
                return Conflict(ex.Message);
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
                return Ok("User deactivated successfully.");
            }

            catch(UserAlreadyDeactivatedException ex)
            {
                return Conflict(ex.Message);
            }

            catch (UserNotFoundException ex)
            {
                return NotFound(ex.Message);
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
                return Ok("User activated successfully.");
            }

            catch(UserAlreadyActivatedException ex)
            {
                return Conflict(ex.Message);
            }

            catch (UserNotFoundException ex)
            {
                return NotFound(ex.Message);
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
                return Ok("User deleted successfully.");
            }

            catch (UserNotFoundException ex)
            {
                return NotFound(ex.Message);
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
                return Ok("User successfully promoted to Admin.");
            }

            catch(UserAlreadyAdminException ex)
            {
                return Conflict(ex.Message);
            }

            catch (UserNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

            catch (Exception ex)
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
                return Ok("User successfully demoted to User.");

            }

            catch (UserAlreadyUserException ex)
            {
                return Conflict(ex.Message);
            }

            catch (UserNotFoundException ex)
            {
                return NotFound(ex.Message);
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
                return Ok("User successfully updated.");

            }

            catch(UserNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
