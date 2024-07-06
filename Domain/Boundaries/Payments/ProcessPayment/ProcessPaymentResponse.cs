using Domain.ValueObjects;

namespace Domain.Boundaries.Payments.ProcessPayment;

public class ProcessPaymentResponse
{
    public Guid PaymentId { get; set; }
    public PaymentStatus Status { get; set; }
}
