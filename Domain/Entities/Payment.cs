using Domain.Enums;

namespace Domain.Entities
{
    public class Payment
    {
        public Guid Id { get; private set; }
        public decimal Amount { get; private set; }
        public string Currency { get; private set; }

        public string MerchantId { get; private set; }
        public string MerchantOrderId { get; private set; }

        public string MaskedCardNumber { get; private set; }

        public DateTime CreatedAt { get; private set; }
        public PaymentStatus Status { get; private set; }


        public Payment(decimal amount, string currency, string merchantId, string merchantOrderId, string maskedCardNumber)
        {

            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero.", nameof(amount));

            if (string.IsNullOrWhiteSpace(currency) || currency.Length != 3)
                throw new ArgumentException("Currency must be a valid 3-character ISO code.", nameof(currency));

            if (string.IsNullOrWhiteSpace(merchantId))
                throw new ArgumentException("Merchant ID is required.", nameof(merchantId));

            if (string.IsNullOrWhiteSpace(merchantOrderId))
                throw new ArgumentException("Merchant Order ID is required.", nameof(merchantOrderId));

            if (string.IsNullOrWhiteSpace(maskedCardNumber))
                throw new ArgumentException("Masked card number is required.", nameof(maskedCardNumber));

            Id = Guid.NewGuid();
            Amount = amount;
            Currency = currency.ToUpper();
            MerchantId = merchantId;
            MerchantOrderId = merchantOrderId;
            MaskedCardNumber = maskedCardNumber;
            CreatedAt = DateTime.UtcNow;
            Status = PaymentStatus.Pending;

        }

        public void Approve()
        {
            if (Status != PaymentStatus.Processing)
            {
                throw new InvalidOperationException($"Cannot approve a payment that is in {Status} state.");
            }
        }

        public void Reject()
        {
            if (Status != PaymentStatus.Pending)
                throw new InvalidOperationException($"Cannot reject a payment that is in {Status} state.");

            Status = PaymentStatus.Rejected;
        }

    }
}