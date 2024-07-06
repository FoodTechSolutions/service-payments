using Domain.ValueObjects;

namespace Domain.Boundaries.Invoices.CreateInvoice;

public record CreateInvoiceResponse(
    string InvoiceNumber,
    InvoiceStatus Status,
    Guid OrderId,
    Money Amount,
    DateTime DueDate,
    DateTime? PaidDate
);
