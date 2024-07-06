using Domain.ValueObjects;

public class Payment
{
    public Guid Id { get; private set; }
    public Money Amount { get; private set; }
    public PaymentMethod PaymentMethod { get; private set; }
    public PaymentStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public Payment(Guid id, Money amount, PaymentMethod paymentMethod)
    {
        Id = id;
        Amount = amount;
        PaymentMethod = paymentMethod;
        Status = PaymentStatus.Pending;
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

public class PaymentMethod
{
}

public enum PaymentStatus
{
    Pending,
    Completed,
    Canceled,
    Refunded,
    Failed
}
