
using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class PaymentService : IPaymentService
{

    private readonly IPaymentRepository _paymentRepository;
    private readonly ILogger<PaymentService> _logger;
    public PaymentService(IPaymentRepository paymentRepository, ILogger<PaymentService> logger)
    {
        _paymentRepository = paymentRepository;
        _logger = logger;

    }

    public async Task<Payment> ProcessPaymentAsync(decimal amount, string currency, string merchantId, string merchantOrderId, string cardNumber)
    {
        _logger.LogInformation("Processing payment request for Order ID: {MerchantOrderId}", merchantOrderId);

        var existingPayment = await _paymentRepository.GetByMerchantOrderIdAsync(merchantOrderId);
        if (existingPayment != null)
        {
            _logger.LogWarning("Duplicate payment detected for Order ID: {MerchantOrderId}. Returning existing transaction.", merchantOrderId);
            return existingPayment;
        }

        var maskedCardNumber = $"****-****-****-{cardNumber.Substring(cardNumber.Length - 4)}";

        var newPayment = new Payment(amount, currency, merchantId, merchantOrderId, maskedCardNumber);

        await _paymentRepository.SaveAsync(newPayment);

        _logger.LogInformation("Payment {PaymentId} created successfully.", newPayment.Id);

        return newPayment;
    }
}