using System.Threading.Tasks;
using PrestigeRentals.Application.DTO;
using PrestigeRentals.Application.Requests;

namespace PrestigeRentals.Application.Services.Interfaces
{
    /// <summary>
    /// Defines methods for handling payment-related operations.
    /// </summary>
    public interface IPaymentService
    {
        /// <summary>
        /// Simulates processing a payment for an order.
        /// Typically used for testing or mock environments.
        /// </summary>
        /// <param name="request">The payment request containing order, user, and cost information.</param>
        /// <returns>A DTO representing the result of the payment operation.</returns>
        Task<PaymentResultDTO> ProcessMockPaymentAsync(CreatePaymentRequest request);
    }
}
