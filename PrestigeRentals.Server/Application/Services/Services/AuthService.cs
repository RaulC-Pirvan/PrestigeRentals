using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using PrestigeRentals.Application.Exceptions;
using PrestigeRentals.Application.Requests;
using PrestigeRentals.Application.Services.Interfaces;
using PrestigeRentals.Application.Services.Interfaces.Repositories;
using PrestigeRentals.Domain.Entities;

namespace PrestigeRentals.Application.Services.Services
{
    /// <summary>
    /// Provides authentication services, including user registration, login, and password hashing.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserDetailsRepository _userDetailsRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IEmailService _emailService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthService"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository for interacting with user data.</param>
        /// <param name="userDetailsRepository">The user details repository for storing user profile information.</param>
        /// <param name="jwtTokenGenerator">The JWT token generator for generating authentication tokens.</param>
        public AuthService(IUserRepository userRepository, IUserDetailsRepository userDetailsRepository, IJwtTokenGenerator jwtTokenGenerator, IEmailService emailService)
        {
            _userRepository = userRepository;
            _userDetailsRepository = userDetailsRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _emailService = emailService;
        }

        /// <summary>
        /// Registers a new user asynchronously.
        /// </summary>
        /// <param name="registerRequest">The registration request containing the user's information.</param>
        /// <returns>A task representing the asynchronous operation, containing the generated authentication token.</returns>
        /// <exception cref="EmailAlreadyExistsException">Thrown when a user with the provided email already exists.</exception>
        /// <exception cref="InvalidPhotoFormatException">Thrown when the provided photo format is invalid.</exception>
        public async Task<string> RegisterAsync(RegisterRequest registerRequest)
        {
            var existingUser = await _userRepository.GetUserByEmail(registerRequest.Email);
            if (existingUser != null)
            {
                throw new EmailAlreadyExistsException();
            }

            var hashedPassword = HashPassword(registerRequest.Password);

            var verificationCode = new Random().Next(100000, 999999).ToString();

            var user = new User
            {
                Email = registerRequest.Email,
                Password = hashedPassword,
                Role = "User",
                EmailVerificationCode = verificationCode,
                VerificationCodeExpiry = DateTime.UtcNow.AddMinutes(15),
                EmailConfirmed = false
            };

            await _userRepository.AddAsync(user);

            //await _emailService.SendVerificationEmailAsync(registerRequest.Email, verificationCode);

            var userDetails = new UserDetails
            {
                UserID = user.Id,
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                DateOfBirth = DateTime.SpecifyKind(registerRequest.DateOfBirth, DateTimeKind.Utc),
            };

            await _userDetailsRepository.AddAsync(userDetails);

            var token = _jwtTokenGenerator.GenerateToken(user);

            return token;
        }

        /// <summary>
        /// Authenticates a user asynchronously based on their login credentials.
        /// </summary>
        /// <param name="loginRequest">The login request containing the user's credentials.</param>
        /// <returns>A task representing the asynchronous operation, containing the generated authentication token.</returns>
        /// <exception cref="Exception">Thrown when the email or password is invalid.</exception>
        public async Task<string> AuthenticateAsync(LoginRequest loginRequest)
        {
            var user = await _userRepository.GetUserByEmail(loginRequest.Email);
            if (user == null || !VerifyPassword(loginRequest.Password, user.Password))
            {
                throw new Exception("Invalid credentials.");
            }

            var token = _jwtTokenGenerator.GenerateToken(user);

            return token;
        }

        /// <summary>
        /// Hashes the provided password using SHA-256.
        /// </summary>
        /// <param name="password">The password to be hashed.</param>
        /// <returns>The hashed password as a base64 encoded string.</returns>
        public string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashBytes);
            }
        }

        /// <summary>
        /// Verifies whether the provided password matches the stored hashed password.
        /// </summary>
        /// <param name="inputPassword">The input password to be verified.</param>
        /// <param name="storedHash">The stored hashed password.</param>
        /// <returns>True if the passwords match, otherwise false.</returns>
        public bool VerifyPassword(string inputPassword, string storedHash)
        {
            var hashInputPassword = HashPassword(inputPassword);
            return hashInputPassword == storedHash;
        }
    }
}