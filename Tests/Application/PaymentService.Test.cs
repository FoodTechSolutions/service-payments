using System;
using System.Threading.Tasks;
using Application.Factories;
using Application.Services;
using Domain.Boundaries.Payments.ProcessPayment;
using Domain.Entities;
using Domain.Repositories;
using Domain.Services;
using Domain.ValueObjects;
using Moq;

namespace Application.Tests.Services;

public class PaymentServiceTests
{
    private Mock<IPaymentRepository> _paymentRepositoryMock;
    private Mock<IInvoiceRepository> _invoiceRepositoryMock;
    private PaymentService _paymentService;

    [SetUp]
    public void SetUp()
    {
        _paymentRepositoryMock = new Mock<IPaymentRepository>();
        _invoiceRepositoryMock = new Mock<IInvoiceRepository>();
        _paymentService = new PaymentService(_paymentRepositoryMock.Object, _invoiceRepositoryMock.Object);
    }

    [Test]
    public async Task Process_ShouldProcessPayment_WhenInvoiceIsValidAndUnpaid()
    {
        // Arrange
        var invoiceId = Guid.NewGuid();
        var paymentId = Guid.NewGuid();
        var request = new ProcessPaymentRequest
        {
            InvoiceId = invoiceId,
            PaymentType = PaymentType.Debit,
            Amount = 100,
            DebitCardCvv = "555",
            DebitCardExpiration = "10-2024",
            DebitCardNumber = "12345678910"
        };

        var invoice = new Invoice(new Money(100, "BRL"), DateTime.UtcNow.AddDays(30), Guid.NewGuid());
        _invoiceRepositoryMock.Setup(repo => repo.GetById(invoiceId)).Returns(invoice);

        var paymentMethodMock = new Mock<IPaymentMethod>();
        paymentMethodMock.Setup(m => m.ProcessPaymentAsync(It.IsAny<ProcessPaymentRequest>()))
            .ReturnsAsync(new Payment(new Money(100, "BRL"), PaymentType.Debit, invoice.Id));

        PaymentMethodFactory.CreatePaymentMethod(PaymentType.Debit);

        // Act
        var response = await _paymentService.Process(request);

        // Assert
        Assert.NotNull(response);
        Assert.AreEqual(PaymentStatus.Completed, response.Status);
        Assert.AreEqual(InvoiceStatus.Paid, invoice.Status);

        _invoiceRepositoryMock.Verify(repo => repo.Update(invoice), Times.Once);
        _paymentRepositoryMock.Verify(repo => repo.Add(It.IsAny<Payment>()), Times.Once);
    }

    [Test]
    public void Process_ShouldThrowException_WhenInvoiceIsNotFound()
    {
        // Arrange
        var request = new ProcessPaymentRequest
        {
            InvoiceId = Guid.NewGuid(),
            PaymentType = PaymentType.Debit
        };

        _invoiceRepositoryMock.Setup(repo => repo.GetById(request.InvoiceId)).Returns((Invoice)null);

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await _paymentService.Process(request));
        Assert.AreEqual("Invoice not found.", ex.Message);
    }

    [Test]
    public void Process_ShouldThrowException_WhenInvoiceIsAlreadyPaid()
    {
        // Arrange
        var invoiceId = Guid.NewGuid();
        var request = new ProcessPaymentRequest
        {
            InvoiceId = invoiceId,
            PaymentType = PaymentType.Debit
        };

        var invoice = new Invoice(new Money(100, "BRL"), DateTime.UtcNow.AddDays(30), Guid.NewGuid());
        invoice.Pay(Guid.NewGuid()); // Set the invoice as paid
        _invoiceRepositoryMock.Setup(repo => repo.GetById(invoiceId)).Returns(invoice);

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await _paymentService.Process(request));
        Assert.AreEqual("Invoice is already paid.", ex.Message);
    }
}
