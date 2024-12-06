using SoapApi.Infrastructure.Entities;
using SoapApi.Models;

namespace SoapApi.Repositories;

public interface IUserRepository
{
    Task<UserModel> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    
    Task<IList<UserModel>> GetAllAsync(CancellationToken cancellationToken);
    
    Task<IList<UserModel>> GetByEmailAsync(string email, CancellationToken cancellationToken);

    public Task DeleteByIdAsync(UserModel user, CancellationToken cancellationToken);

    public Task<UserModel> CreateAsync(UserModel user, CancellationToken cancellationToken);

    public Task<bool> UpdateUser(UserModel user, CancellationToken cancellationToken);

}
