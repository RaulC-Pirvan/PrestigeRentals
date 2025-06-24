using Microsoft.AspNetCore.Mvc;
using PrestigeRentals.Application.Requests;
using PrestigeRentals.Application.Services.Interfaces;

namespace PrestigeRentals.Presentation.Controllers
{
    /// <summary>
    /// Handles payment-related API requests.
    /// </summary>
    [ApiController]
    [Route("api/payment")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentsController"/> class.
        /// </summary>
        /// <param name="paymentService">Service for processing payments.</param>
        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        /// <summary>
        /// Processes a mock payment request.
        /// </summary>
        /// <param name="request">The payment details.</param>
        /// <returns>
        /// Returns <see cref="OkObjectResult"/> with payment result on success,
        /// or <see cref="BadRequestObjectResult"/> with error message on failure.
        /// </returns>
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
