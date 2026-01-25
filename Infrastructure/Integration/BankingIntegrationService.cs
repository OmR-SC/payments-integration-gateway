using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Application.DTOs.Banking;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Integration;

public class BankingIntegrationService
{

    private readonly ILogger<BankingIntegrationService> _logger;
    public BankingIntegrationService(ILogger<BankingIntegrationService> logger)
    {
        _logger = logger;
    }

    public PaymentXmlDto MapToXmlDto(Payment payment)
    {
        _logger.LogInformation("Mapping Payment {PaymentId} to PaymentXmlDto.", payment.Id);
        return new PaymentXmlDto
        {
            Id = payment.MerchantOrderId,
            MerchantId = payment.MerchantId,
            TotalAmount = payment.Amount,
            Currency = payment.Currency,
            Date = payment.CreatedAt
        };
    }

    public string GenerateBankXml(PaymentXmlDto xmlDto)
    {
        _logger.LogInformation("Generating XML for xmlDto with Order ID: {MerchantOrderId}.", xmlDto.Id);

        try
        {
            var settings = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8,
                Indent = true,
                OmitXmlDeclaration = false
            };

            var serializer = new XmlSerializer(typeof(PaymentXmlDto));

            using var stringWriter = new StringWriter();
            using var xmlWriter = XmlWriter.Create(stringWriter, settings);

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            serializer.Serialize(xmlWriter, xmlDto, namespaces);

            var xmlOutput = stringWriter.ToString();
            _logger.LogInformation("XML Generated for Transaction {Id}.", xmlDto.Id);

            return xmlOutput;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating XML for Transaction {Id}.", xmlDto.Id);
            throw;
        }
    }

}
