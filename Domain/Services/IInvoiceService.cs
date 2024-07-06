using Domain.Boundaries.Invoices.CreateInvoice;

namespace Domain.Services;

public interface IInvoiceService
{
    Task<CreateInvoiceResponse> CreateInvoiceAsync(CreateInvoiceRequest request);
}
