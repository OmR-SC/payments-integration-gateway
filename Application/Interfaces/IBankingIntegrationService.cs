using Application.DTOs.Banking;
using Domain.Entities;

namespace Application.Interfaces;
public interface IBankinIntegrationService
{
    PaymentXmlDto MapToXmlDto(Payment payment);
    string GenerateBankXml(PaymentXmlDto xmlDto);

    string TransformToLegacyFormat(string inputXml);
}