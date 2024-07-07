using Domain.Boundaries.Invoices.CreateInvoice;
using Domain.Entities;

namespace Domain.Services;

public interface IInvoiceService
{
    Task<CreateInvoiceResponse> CreateInvoiceAsync(CreateInvoiceRequest request);
    Task<Invoice> GetInvoiceByOrderIdAsync(Guid id);
}
