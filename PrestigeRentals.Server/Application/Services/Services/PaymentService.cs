using System;
using System.Threading.Tasks;
using PrestigeRentals.Application.DTO;
using PrestigeRentals.Application.Requests;
using PrestigeRentals.Application.Services.Interfaces;

namespace PrestigeRentals.Application.Services.Services
{
    /// <summary>
    /// A mock implementation of <see cref="IPaymentService"/> used to simulate payment processing.
    /// This service is typically used for development or testing environments.
    /// </summary>
    public class PaymentService : IPaymentService
    {
        /// <summary>
        /// Simulates payment processing and generates a mock booking reference.
        /// </summary>
        /// <param name="request">The payment request data including order and user details.</param>
        /// <returns>
        /// A task that represents the asynchronous operation, 
        /// containing a <see cref="PaymentResultDTO"/> indicating success and a booking reference.
        /// </returns>
        public Task<PaymentResultDTO> ProcessMockPaymentAsync(CreatePaymentRequest request)
        {
            var bookingRef = Guid.NewGuid().ToString();
            var qrData = $"https://prestigerentals.ro/booking/confirm/{bookingRef}";

            return Task.FromResult(new PaymentResultDTO
            {
                Success = true,
                BookingReference = bookingRef,
            });
        }
    }
}
