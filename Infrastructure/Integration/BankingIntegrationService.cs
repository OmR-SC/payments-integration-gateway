using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;
using Application.DTOs.Banking;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Integration;

public class BankingIntegrationService : IBankingIntegrationService
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

    public string TransformToLegacyFormat(string inputXml)
    {
        _logger.LogInformation("Transforming XML to legacy format.");

        try
        {
            var xsltPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Integration", "Templates", "LegacyBank.xslt");

            if (!File.Exists(xsltPath))
            {
                throw new FileNotFoundException($"XSLT Template not found at: {xsltPath}");
            }

            var xlst = new XslCompiledTransform();
            xlst.Load(xsltPath);

            var xsltArgs = new XsltArgumentList();
            xsltArgs.AddParam("CurrentDate", "", DateTime.UtcNow.ToString("s")); 

            using var stringReader = new StringReader(inputXml);
            using var xmlReader = XmlReader.Create(stringReader);

            using var stringWriter = new StringWriter();
            using var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { Indent = true});

            xlst.Transform(xmlReader, xsltArgs, xmlWriter);

            var transformedXml = stringWriter.ToString();
            _logger.LogInformation("XSLT Transformation completed successfully.");

            return transformedXml;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing XSLT transformation.");
            throw;
        }
    }
}
