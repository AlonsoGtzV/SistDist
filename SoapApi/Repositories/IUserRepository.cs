<<<<<<< HEAD
using SoapApi.Infrastructure.Entities;
=======
<<<<<<< HEAD
using SoapApi.Infrastructure.Entities;
=======
>>>>>>> main
>>>>>>> main
using SoapApi.Models;

namespace SoapApi.Repositories;

public interface IUserRepository
{
    Task<UserModel> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    
    Task<IList<UserModel>> GetAllAsync(CancellationToken cancellationToken);
    
    Task<IList<UserModel>> GetByEmailAsync(string email, CancellationToken cancellationToken);
<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> main

    public Task DeleteByIdAsync(UserModel user, CancellationToken cancellationToken);

    public Task<UserModel> CreateAsync(UserModel user, CancellationToken cancellationToken);

    public Task<bool> UpdateUser(UserModel user, CancellationToken cancellationToken);
<<<<<<< HEAD
=======
=======
>>>>>>> main
>>>>>>> main
}
