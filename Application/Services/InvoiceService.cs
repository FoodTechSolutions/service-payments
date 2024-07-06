using Domain.Boundaries.Invoices.CreateInvoice;
using Domain.entities;
using Domain.Repositories;
using Domain.Services;
using Domain.ValueObjects;

namespace Application.Services;

public class InvoiceService : IInvoiceService
{
    private readonly IInvoiceRepository _invoiceRepository;

    public InvoiceService(IInvoiceRepository invoiceRepository)
    {
        _invoiceRepository = invoiceRepository;
    }

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

        _invoiceRepository.Add(invoice);

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
