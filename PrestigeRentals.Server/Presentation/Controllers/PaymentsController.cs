using Microsoft.AspNetCore.Mvc;
using PrestigeRentals.Application.Requests;
using PrestigeRentals.Application.Services.Interfaces;

namespace PrestigeRentals.Presentation.Controllers
{
    [ApiController]
    [Route("api/payment")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("mockpay")]
        public async Task<IActionResult> MockPay([FromBody] CreatePaymentRequest request)
        {
            var paymentResult = await _paymentService.ProcessMockPaymentAsync(request);

            if (!paymentResult.Success)
                return BadRequest(paymentResult.ErrorMessage);

            return Ok(paymentResult);
        }
    }
}
