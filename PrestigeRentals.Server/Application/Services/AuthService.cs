using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using PrestigeRentals.Application.Services.Interfaces;
using PrestigeRentals.Domain.Entities;

namespace PrestigeRentals.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserDetailsRepository _userDetailsRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(IUserRepository userRepository, IUserDetailsRepository userDetailsRepository, IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _userDetailsRepository = userDetailsRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<string> RegisterAsync(string email, string password, string role, string firstName, string lastName, DateTime dateOfBirth)
        {
            var existingUser = await _userRepository.GetByEmailAsync(email);
            if(existingUser != null)
            {
                throw new Exception("User already exists.");
            }

            var hashedPassword = HashPassword(password);

            var user = new User
            {
                Email = email,
                Password = hashedPassword,
                Role = role,
            };

            await _userRepository.AddAsync(user);

            var userDetails = new UserDetails
            {
                UserID = user.Id,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth
            };

            await _userDetailsRepository.AddAsync(userDetails);

            var token = _jwtTokenGenerator.GenerateToken(user);

            return token;
        }

        public async Task<string> AuthenticateAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null || !VerifyPassword(password, user.Password))
            {
                throw new Exception("Invalid credentials.");
            }

            var token = _jwtTokenGenerator.GenerateToken(user);

            return token;
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashBytes);
            }
        }

        private bool VerifyPassword(string inputPassword, string storedHash)
        {
            var hashInputPassword = HashPassword(inputPassword);
            return hashInputPassword == storedHash;
        }

    }
}
