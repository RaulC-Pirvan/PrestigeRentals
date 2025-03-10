using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigeRentals.Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(string email, string password, string role, string firstName, string lastName, DateTime dateOfBirth);
        Task<string> AuthenticateAsync(string email, string password);

    }
}
