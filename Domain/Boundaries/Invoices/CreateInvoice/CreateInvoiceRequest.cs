using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Boundaries.Invoices.CreateInvoice;

public record CreateInvoiceRequest(
    [Required] Guid OrderId,
    [Required] decimal Amount,
    [Required] DateTime DueDate
);
