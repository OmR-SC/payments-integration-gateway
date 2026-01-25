
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using System.Xml;

namespace Application.Services;

public class PaymentService : IPaymentService
{

    private readonly IPaymentRepository _repository;
    private readonly IBankingIntegrationService _bankingService;
    private readonly ILogger<PaymentService> _logger;
    public PaymentService(IPaymentRepository repository,
        IBankingIntegrationService bankingService,
        ILogger<PaymentService> logger)
    {
        _repository = repository;
        _bankingService = bankingService;
        _logger = logger;

    }

    public async Task<PaymentResponseDto> ProcessPaymentAsync(decimal amount, string currency, string merchantId, string merchantOrderId, string cardNumber)
    {
        _logger.LogInformation("Processing payment request for Order ID: {MerchantOrderId}", merchantOrderId);

        Payment paymentToProcess;

        var existingPayment = await _repository.GetByMerchantOrderIdAsync(merchantOrderId);
        if (existingPayment != null)
        {
            _logger.LogWarning("Duplicate payment detected for Order ID: {MerchantOrderId}. Returning existing transaction.", merchantOrderId);
            paymentToProcess = existingPayment;
        }

        else
        {
            var maskedCardNumber = $"****-****-****-{cardNumber.Substring(cardNumber.Length - 4)}";

            paymentToProcess = new Payment(amount, currency, merchantId, merchantOrderId, maskedCardNumber);

            await _repository.SaveAsync(paymentToProcess);

            _logger.LogInformation("Payment {PaymentId} created successfully.", paymentToProcess.Id);
        }

        string auditXml;
        try
        {
            var xmlDto = _bankingService.MapToXmlDto(paymentToProcess);
            var rawXml = _bankingService.GenerateBankXml(xmlDto);
            auditXml = _bankingService.TransformToLegacyFormat(rawXml);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating banking audit XML for Payment {PaymentId}", paymentToProcess.Id);
            auditXml = "Audit generation failed: " + ex.Message;
        }


        return new PaymentResponseDto
        {
            Id = paymentToProcess.Id,
            Status = paymentToProcess.Status.ToString(),
            Amount = paymentToProcess.Amount,
            Currency = paymentToProcess.Currency,
            MerchantOrderId = paymentToProcess.MerchantOrderId,
            MaskedCardNumber = paymentToProcess.MaskedCardNumber,
            BankingTransactionXml = auditXml
        };
    }
}