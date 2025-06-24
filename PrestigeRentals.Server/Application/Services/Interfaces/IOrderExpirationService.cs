using System;
using System.Threading;
using System.Threading.Tasks;

namespace PrestigeRentals.Application.Services.Interfaces
{
    /// <summary>
    /// Defines a service responsible for updating the status of expired orders.
    /// Typically used in background jobs to invalidate past-due reservations.
    /// </summary>
    public interface IOrderExpirationService
    {
        /// <summary>
        /// Checks for and updates orders that have passed their rental period
        /// and marks them as expired or completed, depending on business logic.
        /// </summary>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        Task UpdateExpiredOrderAsync(CancellationToken cancellationToken);
    }
}
