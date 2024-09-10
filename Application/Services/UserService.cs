using Domain.Repositories;
using Domain.Services;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }


    public async Task Delete(Guid userId)
    {
        _userRepository.Delete(userId);
    }
}
