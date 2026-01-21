using Domain.Entities;
namespace Application.Interfaces;
public interface IPaymentRepository
{
    Task<Payment?> GetByMerchantOrderIdAsync(string merchantOrderId);
    Task SaveAsync(Payment payment);
}