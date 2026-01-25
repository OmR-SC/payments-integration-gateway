using Application.DTOs;
using Domain.Entities;
namespace Application.Interfaces;
public interface IPaymentService
{
    Task<PaymentResponseDto> ProcessPaymentAsync(decimal amount, string currency, string merchantId, string merchantOrderId, string cardNumber);
}