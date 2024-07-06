using Domain.ValueObjects;

public class Payment
{
    public Guid Id { get; private set; }
    public Money Amount { get; private set; }
    public PaymentType PaymentType { get; private set; }
    public PaymentStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public Guid InvoiceId { get; private set; }

    public Payment(Money amount, PaymentType paymentType, Guid invoiceId)
    {
        Id = Guid.NewGuid();
        Amount = amount;
        PaymentType = paymentType;
        Status = PaymentStatus.Pending;
        InvoiceId = invoiceId;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Confirm()
    {
        if (Status != PaymentStatus.Pending)
            throw new InvalidOperationException("Payment is not in a valid state to be confirmed.");

        Status = PaymentStatus.Completed;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        if (Status != PaymentStatus.Pending)
            throw new InvalidOperationException("Payment is not in a valid state to be confirmed.");

        Status = PaymentStatus.Canceled;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Refund()
    {
        if (Status != PaymentStatus.Completed)
            throw new InvalidOperationException("Only completed payments can be refunded.");

        Status = PaymentStatus.Refunded;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Fail()
    {
        if (Status != PaymentStatus.Pending)
            throw new InvalidOperationException("Payment is not in a valid state to be failed.");

        Status = PaymentStatus.Failed;
        UpdatedAt = DateTime.UtcNow;
    }
}

public enum PaymentType
{
    Debit,
    Credit,
    Pix
}

public enum PaymentStatus
{
    Pending,
    Completed,
    Canceled,
    Refunded,
    Failed
}
