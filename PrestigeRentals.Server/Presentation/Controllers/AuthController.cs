using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using PrestigeRentals.Application.Requests;
using PrestigeRentals.Application.Services.Interfaces;
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

        public AuthController(IAuthService authService, IUserManagementService userManagementService)
        {
            _authService = authService;
            _userManagementService = userManagementService;
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

    }
}
