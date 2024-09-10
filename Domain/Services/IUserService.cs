namespace Domain.Services;

public interface IUserService
{
    Task Delete(Guid userId);
}
