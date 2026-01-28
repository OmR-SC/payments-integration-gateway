using Application.Interfaces;
using Application.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests;

public class PaymentServiceTests
{
    private readonly Mock<IPaymentRepository> _mockRepository;
    private readonly Mock<IBankingIntegrationService> _mockBankingService;
    private readonly Mock<ILogger<PaymentService>> _mockLogger;

    private readonly PaymentService _paymentService;

    public PaymentServiceTests()
    {
        _mockRepository = new Mock<IPaymentRepository>();
        _mockBankingService = new Mock<IBankingIntegrationService>();
        _mockLogger = new Mock<ILogger<PaymentService>>();

        _paymentService = new PaymentService(
            _mockRepository.Object,
            _mockBankingService.Object,
            _mockLogger.Object);
    }

    [Fact]
    public async Task ProcessPayment_Should_SavePayment_When_OrderIsNew()
    {
        var amount = 500m;
        var currency = "DOP";
        var merchantId = "TEST_MERCHANT";
        var orderId = "ORD-TEST-001";
        var card = "4111111111111111";

        _mockRepository.Setup(repo => repo.GetByMerchantOrderIdAsync(orderId))
            .ReturnsAsync((Domain.Entities.Payment?)null);

        _mockBankingService.Setup(service => service.MapToXmlDto(It.IsAny<Domain.Entities.Payment>()))
            .Returns(new Application.DTOs.Banking.PaymentXmlDto());

        _mockBankingService.Setup(service => service.GenerateBankXml(It.IsAny<Application.DTOs.Banking.PaymentXmlDto>()))
            .Returns("<FakeXml />");

        _mockBankingService.Setup(service => service.TransformToLegacyFormat(It.IsAny<string>()))
            .Returns("<LegacyFake />");

        var result = await _paymentService.ProcessPaymentAsync(amount, currency, merchantId, orderId, card);

        Assert.NotNull(result);
        Assert.Equal("Pending", result.Status);
        Assert.Equal("****-****-****-1111", result.MaskedCardNumber);
        _mockRepository.Verify(repo => repo.SaveAsync(It.IsAny<Domain.Entities.Payment>()), Times.Once);
    }




}