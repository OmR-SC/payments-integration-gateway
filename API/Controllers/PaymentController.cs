

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

            // // 2. GENERAR XML Y TRANSFORMAR
            // var xmlDto = _bankingService.MapToXmlDto(result);
            // var originalXml = _bankingService.GenerateBankXml(xmlDto);
            
            // // Transformar usando XSLT
            // var legacyXml = _bankingService.TransformToLegacyFormat(originalXml);

            // Console.WriteLine("\n============= XML ORIGINAL (C#) =============");
            // Console.WriteLine(originalXml);
            
            // Console.WriteLine("\n============= XML TRANSFORMADO =============");
            // Console.WriteLine(legacyXml);
            // Console.WriteLine("========================================================\n");

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