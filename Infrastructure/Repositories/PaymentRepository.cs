using Domain.Repositories;
using Infrastructure.Context;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly IMongoCollection<Payment> _collection;

    public PaymentRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<Payment>("Payments");
    }

    public void Update(Payment payment)
    {
        throw new NotImplementedException();
    }

    public async void Add(Payment payment)
    {
        await _collection.InsertOneAsync(payment);
    }

    public Payment GetById(Guid id)
    {
        throw new NotImplementedException();
    }
}
