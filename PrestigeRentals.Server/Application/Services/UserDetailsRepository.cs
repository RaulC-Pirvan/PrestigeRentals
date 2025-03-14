using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrestigeRentals.Application.Services.Interfaces;
using PrestigeRentals.Domain.Entities;
using PrestigeRentals.Infrastructure.Persistence;

namespace PrestigeRentals.Application.Services
{
    public class UserDetailsRepository : IUserDetailsRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserDetailsRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(UserDetails userDetails)
        {
            await _dbContext.UsersDetails.AddAsync(userDetails);
            await _dbContext.SaveChangesAsync();
        }
    }
}
