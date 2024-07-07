using Domain.Entities;
using Domain.ValueObjects;

namespace Tests.Domain.Entities;

[TestFixture]
public class InvoiceTests
{
    [Test]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange
        var amount = new Money(100, "BRL");
        var dueDate = new DateTime();
        var orderId = Guid.NewGuid();

        // Act
        var invoice = new Invoice(amount, dueDate, orderId);

        // Assert
        Assert.That(invoice.Amount, Is.EqualTo(amount));
        Assert.That(invoice.DueDate, Is.EqualTo(dueDate));
        Assert.That(invoice.OrderId, Is.EqualTo(orderId));
        Assert.That(invoice.Id, Is.Not.Empty);
        Assert.That(invoice.Status, Is.EqualTo(InvoiceStatus.Unpaid));
    }

    [Test]
    public void Pay_ShouldChangeStatusToPaid()
    {
        // Arrange
        var invoice = CreateUnpaidInvoice();
        var paymentId = Guid.NewGuid();

        // Act
        invoice.Pay(paymentId);

        // Assert
        Assert.That(invoice.Status, Is.EqualTo(InvoiceStatus.Paid));
    }

    [Test]
    public void Pay_ShouldLinkInvoiceToPayment()
    {
        // Arrange
        var invoice = CreateUnpaidInvoice();
        var paymentId = Guid.NewGuid();

        // Act
        invoice.Pay(paymentId);

        // Assert
        Assert.That(invoice.PaymentId, Is.EqualTo(paymentId));
    }

    [Test]
    public void Pay_ShouldThrowErrorIfInvoiceAlreadyPaid()
    {
        // Arrange
        var invoice = CreatePaidInvoice();
        var paymentId = Guid.NewGuid();

        // Act && Assert
        Assert.That(() => invoice.Pay(paymentId), Throws.InvalidOperationException);
    }

    private Invoice CreateUnpaidInvoice()
    {
        var amount = new Money(100, "BRL");
        var dueDate = new DateTime();
        var orderId = Guid.NewGuid();
        return new Invoice(amount, dueDate, orderId);
    }

    private Invoice CreatePaidInvoice()
    {
        var invoice = CreateUnpaidInvoice();
        var paymentId = Guid.NewGuid();
        invoice.Pay(paymentId);
        return invoice;
    }
}
