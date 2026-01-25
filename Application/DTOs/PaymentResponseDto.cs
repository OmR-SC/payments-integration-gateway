namespace Application.DTOs;

public class PaymentResponseDto
{
    public Guid Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string MerchantOrderId { get; set; } = string.Empty;
    public string MaskedCardNumber { get; set; } = string.Empty;

    // Auditoría: Trazabilidad del mensaje enviado al sistema Legacy (XML)
    public string? BankingTransactionXml { get; set; } 
}