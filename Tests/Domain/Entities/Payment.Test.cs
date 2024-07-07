using System;
using Domain.Entities;
using Domain.ValueObjects;
using NUnit.Framework;

namespace Tests.Domain.Entities;

[TestFixture]
public class PaymentTests
{
    [Test]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange
        var amount = new Money(100m, "BRL");
        var paymentType = PaymentType.Debit;
        var invoiceId = Guid.NewGuid();

        // Act
        var payment = new Payment(amount, paymentType, invoiceId);

        // Assert
        Assert.That(payment.Amount, Is.EqualTo(amount));
        Assert.That(payment.PaymentType, Is.EqualTo(paymentType));
        Assert.That(payment.Status, Is.EqualTo(PaymentStatus.Pending));
        Assert.That(payment.InvoiceId, Is.EqualTo(invoiceId));
        Assert.That(payment.CreatedAt.Date, Is.EqualTo(DateTime.UtcNow.Date));
        Assert.That(payment.UpdatedAt.Date, Is.EqualTo(DateTime.UtcNow.Date));
    }

    [Test]
    public void Confirm_ShouldChangeStatusToCompleted()
    {
        // Arrange
        var payment = CreatePendingPayment();

        // Act
        payment.Confirm();

        // Assert
        Assert.That(payment.Status, Is.EqualTo(PaymentStatus.Completed));
        Assert.That(payment.UpdatedAt.Date, Is.EqualTo(DateTime.UtcNow.Date));
    }

    [Test]
    public void Cancel_ShouldChangeStatusToCanceled()
    {
        // Arrange
        var payment = CreatePendingPayment();

        // Act
        payment.Cancel();

        // Assert
        Assert.That(payment.Status, Is.EqualTo(PaymentStatus.Canceled));
        Assert.That(payment.UpdatedAt.Date, Is.EqualTo(DateTime.UtcNow.Date));
    }

    [Test]
    public void Refund_ShouldChangeStatusToRefunded()
    {
        // Arrange
        var payment = CreateCompletedPayment();

        // Act
        payment.Refund();

        // Assert
        Assert.That(payment.Status, Is.EqualTo(PaymentStatus.Refunded));
        Assert.That(payment.UpdatedAt.Date, Is.EqualTo(DateTime.UtcNow.Date));
    }

    [Test]
    public void Fail_ShouldChangeStatusToFailed()
    {
        // Arrange
        var payment = CreatePendingPayment();

        // Act
        payment.Fail();

        // Assert
        Assert.That(payment.Status, Is.EqualTo(PaymentStatus.Failed));
        Assert.That(payment.UpdatedAt.Date, Is.EqualTo(DateTime.UtcNow.Date));
    }

    [Test]
    public void Confirm_WhenNotPending_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var payment = CreateCompletedPayment();

        // Act & Assert
        Assert.That(() => payment.Confirm(), Throws.InvalidOperationException);
    }

    [Test]
    public void Cancel_WhenNotPending_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var payment = CreateCompletedPayment();

        // Act & Assert
        Assert.That(() => payment.Cancel(), Throws.InvalidOperationException);
    }

    [Test]
    public void Refund_WhenNotCompleted_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var payment = CreatePendingPayment();

        // Act & Assert
        Assert.That(() => payment.Refund(), Throws.InvalidOperationException);
    }

    [Test]
    public void Fail_WhenNotPending_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var payment = CreateCompletedPayment();

        // Act & Assert
        Assert.That(() => payment.Fail(), Throws.InvalidOperationException);
    }

    private Payment CreatePendingPayment()
    {
        var amount = new Money(100m, "BRL");
        var paymentType = PaymentType.Debit;
        var invoiceId = Guid.NewGuid();
        return new Payment(amount, paymentType, invoiceId);
    }

    private Payment CreateCompletedPayment()
    {
        var payment = CreatePendingPayment();
        payment.Confirm();
        return payment;
    }
}
