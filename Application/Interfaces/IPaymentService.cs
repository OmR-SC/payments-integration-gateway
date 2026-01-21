using Domain.Entities;
namespace Application.Interfaces;
public interface IPaymentService
{
    Task<Payment> ProcessPaymentAsync(decimal amount, string currency, string merchantId, string merchantOrderId, string cardNumber);
}