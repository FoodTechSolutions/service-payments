using Domain.ValueObjects;

namespace Domain.Boundaries.Payments.ProcessPayment;

public class ProcessPaymentRequest
{
    public decimal Amount { get; set; }
    public PaymentType PaymentType { get; set; }
    public Guid InvoiceId { get; set; }

    // Debit
    public string? DebitCardNumber { get; set; }
    public string? DebitCardExpiration { get; set; }
    public string? DebitCardCvv { get; set; }
}
