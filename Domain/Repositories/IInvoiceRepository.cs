using Domain.entities;

namespace Domain.Repositories;

public interface IInvoiceRepository
{
    Invoice? GetById(Guid id);
    void Add(Invoice invoice);
    void Update(Invoice invoice);
    Task<Invoice> GetByOrderIdAsync(Guid id);
}
