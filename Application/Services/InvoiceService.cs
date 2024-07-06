using Domain.Boundaries.Invoices.CreateInvoice;
using Domain.entities;
using Domain.Services;
using Domain.ValueObjects;

namespace Application.Services;

public class InvoiceService : IInvoiceService
{
    public Task<CreateInvoiceResponse> CreateInvoiceAsync(CreateInvoiceRequest request)
    {
        var invoice = new Invoice(
            new Money(
                request.Amount,
                "BRL"
            ),
            request.DueDate,
            request.OrderId
        );

        return Task.FromResult(
            new CreateInvoiceResponse(
                invoice.Id.ToString(),
                invoice.Status,
                invoice.OrderId,
                invoice.Amount,
                invoice.DueDate,
                invoice.PaidDate
            )
        );
    }
}
