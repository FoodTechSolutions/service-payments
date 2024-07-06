using Domain.ValueObjects;

namespace Domain.entities;

public class Invoice
{
    public Guid Id { get; private set; }
    public Money Amount { get; private set; }
    public DateTime DueDate { get; private set; }
    public DateTime PaidDate { get; private set; }
    public InvoiceStatus Status { get; private set; }
    public Guid PaymentId { get; private set; }
    public Guid OrderId { get; private set; }

    public Invoice(
        Money amount,
        DateTime dueDate,
        Guid orderId
    )
    {
        Id = Guid.NewGuid();
        Amount = amount;
        DueDate = dueDate;
        OrderId = orderId;
        Status = InvoiceStatus.Unpaid;
    }

    public void Pay(Guid paymentId)
    {
        if (Status != InvoiceStatus.Unpaid)
            throw new InvalidOperationException("Invoice is not in a valid state to be paid.");

        Status = InvoiceStatus.Paid;
        PaidDate = DateTime.UtcNow;
        PaymentId = paymentId;
    }

    public void MarkAsOverdue()
    {
        if (Status != InvoiceStatus.Unpaid)
            throw new InvalidOperationException("Invoice is not in a valid state to be marked as overdue.");

        Status = InvoiceStatus.Overdue;
    }
}
