using Domain.Entities;
using MongoDB.Driver;

namespace Infrastructure.Context
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IMongoClient client, string databaseName)
        {
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<Invoice> Invoices => _database.GetCollection<Invoice>("Invoices");
        public IMongoCollection<Payment> Payments => _database.GetCollection<Payment>("Payments");
    }
}
