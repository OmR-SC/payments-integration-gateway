

using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost]
    public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequestDto request)
    {
        if (request == null)
        {
            return BadRequest("Invalid request data.");
        }

        try
        {
            var result = await _paymentService.ProcessPaymentAsync(
                request.Amount,
                request.Currency,
                request.MerchantId,
                request.MerchantOrderId,
                request.CardNumber
            );

            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }

        catch (Exception ex)
        {
            // Log the exception (not shown here for brevity)
            return StatusCode(500, new { Error = "Internal Server Error", devDetails = ex.Message });
        }
    }
}