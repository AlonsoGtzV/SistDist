using System.ServiceModel;
using SoapApi.Contracts;
using SoapApi.Dtos;
using SoapApi.Repositories;
using SoapApi.Mappers;
<<<<<<< HEAD
using SoapApi.Infrastructure.Entities;
=======
>>>>>>> main

namespace SoapApi.Services;

public class UserService : IUserContract{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository UserRepository){
        _userRepository = UserRepository;
    }
<<<<<<< HEAD

    public async Task<UserResponseDto> CreateUser(UserCreateRequestDto userRequest, CancellationToken cancellationToken)
    {
        var user = userRequest.ToModel();
        var createdUser = await _userRepository.CreateAsync(user, cancellationToken);
        return createdUser.ToDto();
    }

    public async Task<bool> DeleteUserById(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if(user is null){
            throw new FaultException("User not found");
        }

        await _userRepository.DeleteByIdAsync(user, cancellationToken);
        return true;
    }

    public async Task<IList<UserResponseDto>> GetAll(CancellationToken cancellationToken)
=======
public async Task<IList<UserResponseDto>> GetAll(CancellationToken cancellationToken)
>>>>>>> main
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
<<<<<<< HEAD

public async Task<bool> UpdateUser(UserUpdateRequestDto userUpdate, CancellationToken cancellationToken)
{
    var user = userUpdate.ToModel(); 
    
    var existingUser = await _userRepository.GetByIdAsync(user.Id, cancellationToken);

    if (existingUser is null)
    {
        throw new FaultException("User not found");
    }
    else
    {
        return await _userRepository.UpdateUser(user, cancellationToken);
    }
}


=======
>>>>>>> main
}