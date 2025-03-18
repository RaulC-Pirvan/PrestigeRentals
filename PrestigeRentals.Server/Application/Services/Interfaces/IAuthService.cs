using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrestigeRentals.Application.Requests;

namespace PrestigeRentals.Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterRequest registerRequest);
        Task<string> AuthenticateAsync(LoginRequest loginRequest);
        string HashPassword(string password);
    }
}
