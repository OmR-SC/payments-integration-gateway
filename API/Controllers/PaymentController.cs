

using Application.Interfaces;
using Infrastructure.Integration;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly BankingIntegrationService _bankingService;
    public PaymentController(IPaymentService paymentService, BankingIntegrationService bankingService)
    {
        _paymentService = paymentService;
        _bankingService = bankingService;
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

            // // --- CÓDIGO TEMPORAL PARA PROBAR XML ---
            // var xmlDto = _bankingService.MapToXmlDto(result); // result es el pago creado
            // var xmlString = _bankingService.GenerateBankXml(xmlDto);

            // // Lo imprimimos en la consola de la API para verlo
            // Console.WriteLine("============= XML GENERADO =============");
            // Console.WriteLine(xmlString);
            // Console.WriteLine("========================================");
            // // ---------------------------------------

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