using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigeRentals.Application.Services.Interfaces
{
    public interface IOrderExpirationService
    {
        Task UpdateExpiredOrderAsync(CancellationToken cancellationToken);
    }
}
