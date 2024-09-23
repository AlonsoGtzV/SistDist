using System.ServiceModel;
using SoapApi.Contracts;
using SoapApi.Dtos;
using SoapApi.Repositories;
using SoapApi.Mappers;

namespace SoapApi.Services;

public class UserService : IUserContract{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository UserRepository){
        _userRepository = UserRepository;
    }
public async Task<IList<UserResponseDto>> GetAll(CancellationToken cancellationToken)
{
    var users = await _userRepository.GetAllAsync(cancellationToken);
    return users.Select(user => user.ToDto()).ToList();
}

public async Task<IList<UserResponseDto>> GetAllByEmail(string email, CancellationToken cancellationToken)
{
    var users = await _userRepository.GetByEmailAsync(email, cancellationToken);
    return users.Select(user => user.ToDto()).ToList();
}

public async Task<UserResponseDto> GetUserById(Guid userId, CancellationToken cancellationToken)
{
    var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
    if (user is not null)
    {
        return user.ToDto();
    }
    throw new FaultException("User Not Found");
}
}