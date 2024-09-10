namespace Domain.Repositories;

public interface IUserRepository
{
    void Delete(Guid userId);
}
