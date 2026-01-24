using System.Xml.Serialization;

namespace Application.DTOs.Banking;

[XmlRoot("BankingTransaction")]
public class PaymentXmlDto
{
    [XmlElement("TransactionId")]
    public string Id { get; set; } = string.Empty;

    [XmlElement("Remitter")]
    public string MerchantId { get; set; } = string.Empty;

    [XmlElement("Amount")]
    public decimal TotalAmount { get; set; }

    [XmlElement("CurrencyCode")]
    public string Currency { get; set; } = string.Empty;

    [XmlElement("ProcessDate")]
    public DateTime Date { get; set; }

    public PaymentXmlDto()
    {
    }
}