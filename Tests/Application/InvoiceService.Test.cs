using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Application.Services;
using Domain.Boundaries.Payments.ProcessPayment;
using Domain.Entities;
using Domain.Repositories;
using Domain.Services;
using Domain.ValueObjects;
using Moq;

namespace Tests.Application;

[TestFixture]
public class PaymentServiceTests
{
    private Mock<IPaymentRepository> _mockPaymentRepository;
    private Mock<IInvoiceRepository> _mockInvoiceRepository;
    private Mock<IPaymentMethod> _mockPaymentMethod;
    private PaymentService _paymentService;

    [SetUp]
    public void SetUp()
    {
        _mockPaymentRepository = new Mock<IPaymentRepository>();
        _mockInvoiceRepository = new Mock<IInvoiceRepository>();
        _mockPaymentMethod = new Mock<IPaymentMethod>();
        _paymentService = new PaymentService(_mockPaymentRepository.Object, _mockInvoiceRepository.Object);
    }

    [Test]
    public void Process_InvoiceNotFound_ThrowsInvalidOperationException()
    {
        // Arrange
        var request = new ProcessPaymentRequest
        {
            InvoiceId = Guid.NewGuid(),
            PaymentType = PaymentType.Debit
        };

        _mockInvoiceRepository.Setup(x => x.GetById(request.InvoiceId)).Returns((Invoice)null);

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(async () => await _paymentService.Process(request));
    }

    [Test]
    public void Process_InvoiceAlreadyPaid_ThrowsInvalidOperationException()
    {
        // Arrange
        var request = new ProcessPaymentRequest
        {
            InvoiceId = Guid.NewGuid(),
            PaymentType = PaymentType.Debit
        };

        var invoice = new Invoice(
            new Money(100, "BRL"),
            DateTime.Now,
            Guid.NewGuid()
        );

        invoice.Pay(Guid.NewGuid());

        _mockInvoiceRepository.Setup(x => x.GetById(request.InvoiceId)).Returns(invoice);

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(async () => await _paymentService.Process(request));
    }
}
