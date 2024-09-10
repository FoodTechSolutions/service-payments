using Domain.Entities;
using Domain.Repositories;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<User> _collection;

    public UserRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<User>(nameof(User));
    }

    public void Delete(Guid userId)
    {
        _collection.DeleteOne(user => user.Id == userId);
    }
}
