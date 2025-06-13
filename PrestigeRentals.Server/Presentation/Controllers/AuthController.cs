using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenCvSharp;
using PrestigeRentals.Application.DTO;
using PrestigeRentals.Application.Exceptions;
using PrestigeRentals.Application.Requests;
using PrestigeRentals.Application.Services;
using PrestigeRentals.Application.Services.Interfaces;
using PrestigeRentals.Application.Services.Interfaces.Repositories;
using PrestigeRentals.Application.Validators;
using PrestigeRentals.Infrastructure.Persistence;
using Tesseract;
using LoginRequest = PrestigeRentals.Application.Requests.LoginRequest;
using RegisterRequest = PrestigeRentals.Application.Requests.RegisterRequest;

namespace PrestigeRentals.Presentation.Controllers
{
    /// <summary>
    /// Handles authentication and user management related actions such as registration, login, and account management.
    /// </summary>
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserManagementService _userManagementService;
        private readonly ApplicationDbContext _dbContext;
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;

        private readonly IWebHostEnvironment _env;
        private string RootImages => Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "images");


        private readonly string tessDataPath = @"C:\Program Files\Tesseract-OCR\tessdata";

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="authService">The authentication service used for user registration and login.</param>
        /// <param name="userManagementService">The user management service for user account operations.</param>
        /// <param name="dbContext">The application's database context.</param>
        public AuthController(IAuthService authService, IUserManagementService userManagementService, ApplicationDbContext dbContext, IEmailService emailService, IUserRepository userRepository, IWebHostEnvironment env)
        {
            _authService = authService;
            _userManagementService = userManagementService;
            _dbContext = dbContext;
            _emailService = emailService;
            _userRepository = userRepository;
            _env = env;
        }

        /// <summary>
        /// Registers a new user in the system and returns a JWT token.
        /// </summary>
        /// <param name="model">The registration request containing the user's details.</param>
        /// <returns>A token upon successful registration.</returns>
        [HttpPost("/register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            try
            {
                // Validate email
                if (!RegisterValidator.ValidateEmail(model.Email))
                {
                    return BadRequest("Invalid email format.");
                }

                // Validate password
                if (!RegisterValidator.ValidatePassword(model.Password))
                {
                    return BadRequest("Password must be at least 6 characters long, contain at least one uppercase letter, one lowercase letter, one number, and one special character (!@#).");
                }

                // Validate first and last name
                if (!RegisterValidator.ValidateName(model.FirstName) || !RegisterValidator.ValidateName(model.LastName))
                {
                    return BadRequest("First name and last name are required.");
                }

                var token = await _authService.RegisterAsync(model);

                return Ok(new { Token = token, Message = "Verification email sent." });
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

        /// <summary>
        /// Verifies a user's email address based on the provided verification code.
        /// This endpoint is typically called when the user submits their email verification code.
        /// </summary>
        /// <param name="request">The request containing the email and verification code sent to the user.</param>
        /// <returns>
        /// Returns an `Ok` response with a success message if the email is successfully verified. 
        /// Returns `NotFound` if the user is not found in the system. 
        /// Returns `BadRequest` if the email is already confirmed or if the verification code is invalid or expired.
        /// </returns>
        /// <response code="200">Email successfully verified.</response>
        /// <response code="400">Invalid or expired verification code, or email already verified.</response>
        /// <response code="404">User not found.</response>

        [HttpPost("/verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequest request)
        {
            var user = await _userRepository.GetUserByEmail(request.Email);
            if (user == null)
                return NotFound("User not found.");

            if (user.EmailConfirmed)
                return BadRequest("Email already verified.");

            if (user.EmailVerificationCode != request.Code || user.VerificationCodeExpiry < DateTime.UtcNow)
                return BadRequest("Invalid or expired verification code.");

            user.EmailConfirmed = true;
            user.EmailVerificationCode = null;
            user.VerificationCodeExpiry = null;

            await _userRepository.UpdateAsync(user);

            return Ok(new { message = "Email successfully verified." });
        }

        [HttpPost("/resend-verification-code")]
        public async Task<IActionResult> ResendVerificationCode([FromBody] ResendCodeRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
                return BadRequest("Email is required.");

            var user = await _userRepository.GetUserByEmail(request.Email);
            if (user == null)
                return BadRequest("User not found.");

            if (user.EmailConfirmed)
                return BadRequest("Email is already verified.");

            var newCode = new Random().Next(100000, 999999).ToString();

            user.EmailVerificationCode = newCode;
            user.VerificationCodeExpiry = DateTime.UtcNow.AddMinutes(15);

            await _userRepository.UpdateAsync(user);

            await _emailService.SendVerificationEmailAsync(user.Email, newCode);
            return Ok(new { message = "Verification code resent successfully." });
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token if credentials are valid.
        /// </summary>
        /// <param name="model">The login request containing the user's credentials.</param>
        /// <returns>A token upon successful authentication.</returns>
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
            catch (UserNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userManagementService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Changes a user's password.
        /// </summary>
        /// <param name="userId">The ID of the user whose password is to be changed.</param>
        /// <param name="oldPassword">The user's current password.</param>
        /// <param name="newPassword">The new password.</param>
        /// <returns>A message indicating whether the password was successfully changed.</returns>
        [Authorize]
        [HttpPatch("{userId}/change-password")]
        public async Task<IActionResult> ChangePassword(int userId, string oldPassword, string newPassword)
        {
            try
            {
                bool isPasswordChanged = await _userManagementService.ChangePassword(userId, oldPassword, newPassword);
                return Ok(new { message = "Password changed successfully." });
            }
            catch (InvalidPasswordException ex)
            {
                return Unauthorized(ex.Message);
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

        /// <summary>
        /// Changes a user's email address.
        /// </summary>
        /// <param name="userId">The ID of the user whose email is to be changed.</param>
        /// <param name="newEmail">The new email address.</param>
        /// <returns>A message indicating whether the email was successfully changed.</returns>
        [Authorize]
        [HttpPatch("{userId}/change-email")]
        public async Task<IActionResult> ChangeEmail(int userId, string newEmail)
        {
            try
            {
                bool isEmailChanged = await _userManagementService.ChangeEmail(userId, newEmail);
                if (isEmailChanged)
                    return Ok(new { message = "User successfully updated." });
                return BadRequest("Error: Email could not be changed.");
            }
            catch (EmailAlreadyExistsException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        /// <summary>
        /// Deactivates a user's account.
        /// </summary>
        /// <param name="userId">The ID of the user to deactivate.</param>
        /// <returns>A message indicating whether the account was successfully deactivated.</returns>
        [Authorize]
        [HttpPatch("{userId}/set-inactive")]
        public async Task<IActionResult> DeactivateAccount(int userId)
        {
            try
            {
                bool isUserDeactivated = await _userManagementService.DeactivateAccount(userId);
                return Ok(new { message = "User deactivated successfully." });
            }
            catch (UserAlreadyDeactivatedException ex)
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

        /// <summary>
        /// Activates a user's account.
        /// </summary>
        /// <param name="userId">The ID of the user to activate.</param>
        /// <returns>A message indicating whether the account was successfully activated.</returns>
        [Authorize]
        [HttpPatch("{userId}/set-active")]
        public async Task<IActionResult> ActivateAccount(int userId)
        {
            try
            {
                bool isUserActivated = await _userManagementService.ActivateAccount(userId);
                return Ok("User activated successfully.");
            }
            catch (UserAlreadyActivatedException ex)
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

        /// <summary>
        /// Deletes a user's account from the system.
        /// </summary>
        /// <param name="userId">The ID of the user to delete.</param>
        /// <returns>A message indicating whether the account was successfully deleted.</returns>
        [Authorize]
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteAccount(int userId)
        {
            try
            {
                bool isUserDeleted = await _userManagementService.DeleteAccount(userId);
                return Ok(new { message = "User deleted successfully." });
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

        /// <summary>
        /// Promotes a user to an Admin role.
        /// </summary>
        /// <param name="userId">The ID of the user to promote.</param>
        /// <returns>A message indicating whether the promotion was successful.</returns>
        [Authorize(Roles = "Admin")]
        [HttpPatch("{userId}/set-admin")]
        public async Task<IActionResult> SetAdmin(int userId)
        {
            try
            {
                bool isUserPromoted = await _userManagementService.MakeAdmin(userId);
                return Ok(new { message = "User successfully promoted to Admin." });

            }
            catch (UserAlreadyAdminException ex)
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

        /// <summary>
        /// Demotes a user from Admin to a regular user.
        /// </summary>
        /// <param name="userId">The ID of the user to demote.</param>
        /// <returns>A message indicating whether the demotion was successful.</returns>
        [Authorize(Roles = "Admin")]
        [HttpPatch("{userId}/set-user")]
        public async Task<IActionResult> SetUser(int userId)
        {
            try
            {
                bool isUserDemoted = await _userManagementService.RevertToUser(userId);
                return Ok(new { message = "User successfully demoted to User." });
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
        [Authorize(Roles = "Admin")]
        [HttpPost("{userId}/ban")]
        public async Task<IActionResult> BanUser(long userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null) return NotFound();

            user.Banned = true;
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("{userId}/unban")]
        public async Task<IActionResult> UnbanUser(long userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null) return NotFound();

            user.Banned = false;
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Updates the details of a user's account.
        /// </summary>
        /// <param name="userId">The ID of the user whose details are to be updated.</param>
        /// <param name="updateUserDetailsRequest">The new user details to be updated.</param>
        /// <returns>A message indicating whether the user details were successfully updated.</returns>
        [Authorize]
        [HttpPatch("{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, UpdateUserDetailsRequest updateUserDetailsRequest)
        {
            try
            {
                bool isUserUpdated = await _userManagementService.UpdateUserDetails(userId, updateUserDetailsRequest);
                return Ok(new { message = "User successfully updated." });
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

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString) || !long.TryParse(userIdString, out var userId))
                return Unauthorized();

            var profile = await _userManagementService.GetUserProfile(userId);
            return Ok(profile);
        }

        [HttpGet("profile/{userId}")]
        public async Task<IActionResult> GetProfileByUserId(long userId)
        {
            try
            {
                var profile = await _userManagementService.GetUserProfile(userId);

                if (profile == null)
                    return NotFound();

                return Ok(profile);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving user profile: {ex.Message}");
            }
        }

        [HttpPost("extract-cnp/{userId}")]
        public IActionResult ExtractCnp(string userId)
        {
            // Construim calea către poza buletinului
            var idCardPath = Path.Combine(RootImages, "user", userId, "idcard", "idcard.jpg");

            if (!System.IO.File.Exists(idCardPath))
                return NotFound("Imaginea buletinului nu a fost găsită pentru acest user.");

            try
            {
                // Prelucrare imagine cu OpenCV
                var preprocessedPath = PreprocessImage(idCardPath);

                // OCR cu Tesseract
                using var engine = new TesseractEngine(tessDataPath, "ron", EngineMode.Default);
                using var pix = Pix.LoadFromFile(preprocessedPath);
                using var page = engine.Process(pix);

                string ocrText = page.GetText();

                // Caută CNP (13 cifre)
                var match = Regex.Match(ocrText, @"\b\d{13}\b");

                // Șterge fișierul ID card original
                try
                {
                    System.IO.File.Delete(idCardPath);
                }
                catch (Exception deleteEx)
                {
                    // Opțional: log, dar nu bloca flow-ul dacă nu se poate șterge
                    Console.WriteLine($"Eroare la ștergerea fișierului: {deleteEx.Message}");
                }

                if (match.Success)
                {
                    bool isAdult = IsAdultBasedOnCnp(match.Value);
                    return Ok(new { isAdult });
                }
                else
                {
                    return Ok(new { isAdult = false, message = "CNP nu a fost detectat." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "A apărut o eroare: " + ex.Message);
            }
        }

        private string PreprocessImage(string inputPath)
        {
            var outputPath = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(inputPath) + "_preprocessed.jpg");
            var src = Cv2.ImRead(inputPath, ImreadModes.Grayscale);
            Cv2.Resize(src, src, new OpenCvSharp.Size(src.Width * 3, src.Height * 3));
            Cv2.GaussianBlur(src, src, new OpenCvSharp.Size(3, 3), 0);
            Cv2.Threshold(src, src, 0, 255, ThresholdTypes.Binary | ThresholdTypes.Otsu);
            Cv2.ImWrite(outputPath, src);
            return outputPath;
        }

        private bool IsAdultBasedOnCnp(string cnp)
        {
            if (string.IsNullOrWhiteSpace(cnp) || cnp.Length != 13)
                return false;

            char sexDigit = cnp[0];

            int yearPrefix;
            switch (sexDigit)
            {
                case '1':
                case '2':
                    yearPrefix = 1900;
                    break;
                case '3':
                case '4':
                    yearPrefix = 1800;
                    break;
                case '5':
                case '6':
                    yearPrefix = 2000;
                    break;
                default:
                    return false;
            }

            if (!int.TryParse(cnp.Substring(1, 2), out int year))
                return false;
            year += yearPrefix;

            if (!int.TryParse(cnp.Substring(3, 2), out int month) || month < 1 || month > 12)
                return false;

            if (!int.TryParse(cnp.Substring(5, 2), out int day) || day < 1 || day > 31)
                return false;

            DateTime birthDate;
            try
            {
                birthDate = new DateTime(year, month, day);
            }
            catch
            {
                return false;
            }

            var today = DateTime.Today;
            int age = today.Year - birthDate.Year;
            if (birthDate > today.AddYears(-age)) age--;

            return age >= 18;
        }
    }
}
