using Domain.Entities;
using Domain.Repositories;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly IMongoCollection<Invoice> _collection;

    public InvoiceRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<Invoice>("Invoices");
    }

    public Invoice? GetById(Guid id)
    {
        return _collection.Find(x => x.Id == id).FirstOrDefault();
    }

    public async void Add(Invoice invoice)
    {
        await _collection.InsertOneAsync(invoice);
    }

    public void Update(Invoice invoice)
    {
        _collection.ReplaceOne(x => x.Id == invoice.Id, invoice);
    }

    public Task<Invoice> GetByOrderIdAsync(Guid id)
    {
        return _collection.Find(x => x.OrderId == id).FirstOrDefaultAsync();
    }
}
