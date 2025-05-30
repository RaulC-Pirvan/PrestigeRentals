using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrestigeRentals.Application.DTO;
using PrestigeRentals.Application.Requests;
using PrestigeRentals.Application.Services.Interfaces;

namespace PrestigeRentals.Application.Services.Services
{
    public class PaymentService : IPaymentService
    {
        public Task<PaymentResultDTO> ProcessMockPaymentAsync(CreatePaymentRequest request)
        {
            var bookingRef = Guid.NewGuid().ToString();

            var qrData = $"https://prestigerentals.ro/booking/confirm/{bookingRef}";

            return Task.FromResult(new PaymentResultDTO
            {
                Success = true,
                BookingReference = bookingRef,
                QrCodeData = qrData
            });
        }
    }
}
