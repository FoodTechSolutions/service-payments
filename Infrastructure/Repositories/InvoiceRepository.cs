using Domain.entities;
using Domain.Repositories;
using Infrastructure.Context;
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
        throw new NotImplementedException();
    }
}
