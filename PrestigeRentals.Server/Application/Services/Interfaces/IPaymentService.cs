using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrestigeRentals.Application.DTO;
using PrestigeRentals.Application.Requests;

namespace PrestigeRentals.Application.Services.Interfaces
{
    public interface IPaymentService
    {
        public Task<PaymentResultDTO> ProcessMockPaymentAsync(CreatePaymentRequest request);
    }
}
